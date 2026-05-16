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
            Defense,
            Health
        }

        public CardState State; // { get; private set; }
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
                StatType.Defense => BaseDefense,
                _ => 0
            };
        }
        public int CurrentAttack { get; private set; }
        public int CurrentDefense { get; private set; }
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
            CurrentDefense = BaseDefense;
            CurrentHealth = BaseHealth;        
            CardEffects = new List<Effect>();
            // get the effect from effect dictionary
        }

        public void ChangeCurrentHealth(int value)
        {
            CurrentHealth = value;
        }

        public void ChangeCurrentAttack(int value)
        {
            CurrentAttack = value;
        }

        public void ChangeCurrentDefense(int value)
        {
            CurrentDefense = value;
        }

        public void ChangePosition(Position position)
        {
            Position = position;
        }
    }

    public struct Position
    {
        public int  SideIndex;
        public int LaneIndex;
        public bool IsFront;
    }

}