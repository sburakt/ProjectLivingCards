using TCG.Model.Cards;
using TCG.Model.Events;
using TCG.Model.Core;

namespace TCG.Model.Effects
{
    public interface IReactiveEffect
    {
        public void React(Match match, MatchEvent e);
    }

    public interface IBuff
    {
        public int BuffId { get; }
        public int SourceInstanceId { get; }
        public int TargetInstanceId { get; }
        int? Stack { get; }
    }
    
    
    public interface IStatModifier
    {
        int ModifyStat(Match match, RuntimeCard.StatType statType, int currentValue);
    }

    public interface ICheckEveryTurn
    {
        void Process(Match match);
    }
}