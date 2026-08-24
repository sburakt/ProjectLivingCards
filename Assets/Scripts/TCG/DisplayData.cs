using System.Collections.Generic;

namespace TCG
{
    public class CardDisplayData
    {
        public int InstanceId;
        public string CardName;
        public int Attack;
        public int CurrentHealth;
        public int BaseHealth;
        public List<BuffDisplayData> Buffs;
    }

    public class BuffDisplayData
    {
        public int BuffId;
        public int? StackCount;
    }

    public class SideDisplayData
    {
        public int LifePoints;
        public List<CardDisplayData> Hand;
        public CardDisplayData[] Field; // 6 slots, null if empty
    }

    public class GameDisplayData
    {
        public SideDisplayData[] Sides;
        public int ActiveSideIndex;
        public List<string> EventLog;
        public List<PlayerMove> LegalMoves;
    }
}