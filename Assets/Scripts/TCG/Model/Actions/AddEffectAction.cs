using TCG.Model.Core;
using TCG.Model.Effects;
using TCG.Model.Events;

namespace TCG.Model.Actions
{
    public class AddEffectAction : MatchAction
    {
        private readonly Effect _effectToAdd;

        public AddEffectAction(Effect effect)
        {
            _effectToAdd = effect;
        }

        public override void Execute(Match match)
        {
            match.AddEffect(_effectToAdd);
            match.EnqueueEvent(new MatchEvent()
            {
                Type = MatchEventType.EffectAdded,
                // insufficent
            });
        }
    }
}