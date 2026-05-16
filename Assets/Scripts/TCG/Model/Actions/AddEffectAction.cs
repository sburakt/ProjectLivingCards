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
        }
    }
    
    // public class dsadasd : MatchAction
    // {
    //     public override void Execute(Match match)
    //     {
    //         // 1. Look for an existing buff of the exact same type on this card
    //         var existingBuff = match.ActiveEffects.FirstOrDefault(e =>
    //             e is ScavengerStatBuffEffect buff && buff.TargetInstanceId == _targetId);
    //
    //         if (existingBuff != null)
    //         {
    //             // 2. Just increment the stack! No new objects created!
    //             ((ScavengerStatBuffEffect)existingBuff).Stacks++;
    //         }
    //         else
    //         {
    //             // 3. Create it for the first time
    //             match.ActiveEffects.Add(new ScavengerStatBuffEffect(_targetId));
    //         }
    //     }
    // }
}