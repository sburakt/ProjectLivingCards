using TCG.Model.Cards;
using TCG.Model.Core;
using TCG.Model.Events;

namespace TCG.Model.Effects
{
    public class ScavengerEffect  : Effect, IReactiveEffect, IBuff, IStatModifier
    {
        public int? Stack { get; private set; } 
        public int BuffId { get; }
        public int SourceInstanceId { get; }
        public int TargetInstanceId { get; }

        public ScavengerEffect(int ownerInstanceId) : base(ownerInstanceId)
        {
            BuffId = 1; // hard coded
            Stack = 0;
            SourceInstanceId = ownerInstanceId;
            TargetInstanceId = ownerInstanceId;
        }
        
        public void React(Match match, MatchEvent e)
        {
            // condition check here its only 1 line so its single if statement
            if (e.Type == MatchEventType.CardDestroyed)
            {
                RuntimeCard card = match.FindRuntimeCardById(OwnerInstanceId);
                if (card == null || card.State != RuntimeCard.CardState.OnBoard)
                    return; // fizzle safely
                // todo add event effect triggered both for gameplay and ui
                Stack++;
            }
        }

        public int ModifyStat(Match match, RuntimeCard.StatType statType, int currentValue)
        {
            if (statType == RuntimeCard.StatType.Attack)
            {
                return currentValue + (1 * Stack ?? 0);
            }
            return currentValue;
        }

        public override bool ShouldTerminate(Match match)
        {
            RuntimeCard card = match.FindRuntimeCardById(OwnerInstanceId);
            if (card == null || card.State != RuntimeCard.CardState.OnBoard)
                return true;
            return false;
        }
        
    }
}