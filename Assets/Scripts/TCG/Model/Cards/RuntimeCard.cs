using System.Collections.Generic;
using TCG.Model.Core;
using TCG.Model.Effects;

namespace TCG.Model.Cards
{
    //[System.Serializable]
    public class RuntimeCard 
    {    
        public int InstanceId { get; private set; }
        public StaticCard StaticCard { get; private set; }
        public enum CardState
        {
            InDeck,
            InHand,
            OnBoard,
            InGraveyard,
            Exiled
        }
        
        public enum StatType
        {
            Attack,
            Health
        }

        public CardState State { get; private set; }
        public List<Effect> CardEffects { get; private set; }
        public Side Owner { get; private set; }
        public Side Controller { get; private set; }
        public Position Position { get; private set; }
    
        // stats
        public int BaseAttack { get; private set; }
        public int BaseDefense { get; private set; }
        public int BaseHealth { get; private set; }
        
        public int GetBaseStat(StatType type)
        {
            return type switch
            {
                StatType.Attack => BaseAttack,
                StatType.Health => BaseHealth,
                _ => 0
            };
        }

        public int CalculateStat(Match match,StatType type)
        {
            int stat = GetBaseStat(type);
            foreach (Effect effect in match.EffectList)
            {
                if (effect is IStatModifier statModifier)
                {
                    stat = statModifier.ModifyStat(match, type, stat);
                }
            }
            return stat;
        }
        
        public int CurrentAttack { get; private set; }
        public int CurrentHealth { get; private set; }
    
        //functions

        public RuntimeCard(int instanceID, PersistentCard persistentCard, Side owner, Side controller, CardState state = CardState.InDeck)
        {
            this.InstanceId = instanceID;
            this.StaticCard = StaticCardLibrary.CardLibrary[persistentCard.staticCardId];
            this.Owner = owner;
            this.Controller = controller;
            this.State = state;
            BaseAttack = StaticCard.BaseAttack;
            BaseDefense = StaticCard.BaseDefense;
            BaseHealth = StaticCard.BaseHealth;
            CurrentAttack = BaseAttack;
            CurrentHealth = BaseHealth;        
            CardEffects = new List<Effect>();
            foreach (string effectId in StaticCard.EffectIDs)
            {
                CardEffects.Add(EffectLibrary.Create(effectId, instanceID));
            }
        }
        // each instance of an effect is new and in the card but they are activated from by match
        // can make them static that takes constructor paramters in struct but then in the list i dont have class with polymorphsim and i check flags in struct for that?
        // clone matches can use the same instances but some has local var? i can store local vars for effect in match like phases but dont really want to do that
        
        public void IncreaseHealth(int amount)
        {
            CurrentHealth += amount;
        }

        public int DecreaseHealth(int amount)
        {
            int surplus = amount - CurrentHealth;
            if (surplus > 0)
            {
                CurrentHealth = 0;
                return surplus;
            }
            CurrentHealth -= amount;
            return 0;
        }


        public void SetState(CardState state)
        {
            State = state;
        }

        public void SetPosition(Position position)
        {
            Position = position;
        }
    }
}