namespace TCG.Model.Events
{
    public struct MatchEvent
    {
        public MatchEventType Type;

        public int SideIndex;
        
        public int SourceId;
        public int TargetId;
        public int Value;
        
        public override string ToString()
        {
            return $"{Type} | Source: {SourceId} | Target: {TargetId} | Value: {Value}";
        }
    }

    public enum MatchEventType
    {
        CardAttackCard,
        CardAttackSide,
        CardDrawn,
        CardPlayed,
        CardDamaged,
        CardDestroyed,
        CardMoved,
        PlayerDamaged,
        TurnStarted,
        EffectAdded,
    }
}

