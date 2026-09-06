using System.Collections.Generic;
using TCG.Model.Cards;
using TCG.Model.Core;
using TCG.Model.Effects;

namespace TCG
{
    public class CardSnapshot
    {
        public int InstanceId;
        public string CardName;
        public int Attack;
        public int CurrentHealth;
        public int BaseHealth;
        public List<BuffSnapshot> Buffs;

        public CardSnapshot(RuntimeCard card, Match match)
        {
            InstanceId = card.InstanceId;
            CardName = card.StaticCard.CardId;
            Attack = card.CalculateStat(match,RuntimeCard.StatType.Attack);
            BaseHealth = card.GetBaseStat(RuntimeCard.StatType.Health);
            CurrentHealth = card.CurrentHealth;
            Buffs = new List<BuffSnapshot>();
        }
        
        public CardSnapshot(int cardId, Match match)
        {
            RuntimeCard card = match.FindRuntimeCardById(cardId);
            InstanceId = card.InstanceId;
            CardName = card.StaticCard.CardId;
            Attack = card.CalculateStat(match,RuntimeCard.StatType.Attack);
            BaseHealth = card.GetBaseStat(RuntimeCard.StatType.Health);
            CurrentHealth = card.CurrentHealth;
            Buffs = new List<BuffSnapshot>();
        }
    }

    public class BuffSnapshot
    {
        public int SourceId;
        public int TargetId;
        public int BuffId;
        public int? StackCount;

    }

    public class SideSnapshot
    {
        public int LifePoints;
        public List<int> Hand;
        public int[] Field;

        public SideSnapshot(Side side)
        {
            LifePoints = side.LifePoints;
            Hand = new List<int>();
            for (int i = 0; i < side.Hand.Count; i++)
            {
                Hand.Add(side.Hand[i].InstanceId);
            }
            Field = new int[6];
            int j = 0;
            foreach (var cell in side.Field.GetCells())
            {
                if (cell.IsFull)
                    Field[j] = cell.Card.InstanceId;
                j++;
            }
        }
        
        public List<int> GetCardIds()
        {
            List<int> ids = new List<int>();
            ids.AddRange(Hand);
            foreach (int id in Field)
            {
                if (id != 0)
                    ids.Add(id);
            }
            return ids;
        }
    }
    
    
    
    public class BoardSnapshot
    {
        public SideSnapshot[] Sides;
        public int ActiveSideIndex;
        public List<BuffSnapshot> Buffs;
        public Dictionary<int, CardSnapshot> Cards;

        public BoardSnapshot(Match match)
        {
            Sides = new SideSnapshot[]
            {
                new SideSnapshot(match.Sides[0]),
                new SideSnapshot(match.Sides[1])
            };
            ActiveSideIndex = match.ActiveSideIndex;
            Buffs = new List<BuffSnapshot>();
            Cards = new Dictionary<int, CardSnapshot>();
            for (int i = 0; i < 2; i++)
            {
                foreach (int id in Sides[i].GetCardIds())
                {
                    Cards[id] = new CardSnapshot(match.FindRuntimeCardById(id), match);
                }
            }
            foreach (Effect effect in match.EffectList)
            {
                if (effect is IBuff buff)
                {
                    Buffs.Add(new BuffSnapshot()
                    {
                        BuffId = buff.BuffId,
                        SourceId = buff.SourceInstanceId,
                        TargetId = buff.TargetInstanceId,
                        StackCount = buff.Stack
                    });
                }
            }
            //AddBuffs(match);
        }

        private void AddBuffs(Match match)
        {
            foreach (Effect effect in match.EffectList)
            {
                if (effect is IBuff buff)
                {
                    Cards[buff.SourceInstanceId].Buffs.Add(new BuffSnapshot()
                    {
                        BuffId = buff.BuffId,
                        StackCount = buff.Stack
                    });
                }
            }
        }
    }
}