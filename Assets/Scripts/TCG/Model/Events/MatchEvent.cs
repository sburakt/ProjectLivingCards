using TCG.Model.Core;

namespace TCG.Model.Events
{
    public struct MatchEvent
    {
        public MatchEventType Type;

        public int PrimarySideIndex;
        public int SecondarySideIndex;
        
        public int PrimaryCardId;
        public int SecondaryCardId;
        public int ThirdCardId;
        public int ForthCardId;
        
        public Position PrimaryPosition;
        public Position SecondaryPosition;

        public int EffectId;
        public int Value;

        public int EnumAsInt;
        
    }

    public enum MatchEventType
    {
        CardAttackedCard,
        CardAttackedSide,
        CardDestroyedCard,
        CardEnteredField,
        CardDamagedCard,
        SideCardDrawn,
        CardPushedCard,
        CardMoved,
        CardDamagedSide,
        SideTurnStarted,
    }
}

