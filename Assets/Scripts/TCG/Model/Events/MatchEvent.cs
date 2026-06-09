namespace TCG.Model.Events
{
    public struct MatchEvent
    {
        public MatchEventType Type;
        public int SourceId;
        public int TargetId;
        public int Value;
    }

    public enum MatchEventType
    {
        CardDrawn,
        CardPlayed,
        CardDamaged,
        CardDestroyed,
        CardMoved,
        PlayerDamaged,
        TurnStarted,
    }
}

