/*
using TCG.Model.Cards;
using TCG.Model.Core;
using UnityEngine;

namespace TCG.Model.Actions
{
    public class RestoreSideDefenseAction : MatchAction
    {
        private readonly int _sideIndex;
        public RestoreSideDefenseAction(int sideIndex)
        {
            _sideIndex = sideIndex;
        }
        public override void Execute(Match match)
        {
            Side side = match.Sides[_sideIndex];

            // Sweep through every lane on this player's field
            foreach (Lane lane in side.Field.Lanes)
            {
                if (lane.FrontCard != null)
                    match.PushAction(new RestoreCardDefense(lane.FrontCard.InstanceId));
                if (lane.BackCard != null)
                    match.PushAction(new RestoreCardDefense(lane.BackCard.InstanceId));
            }
            
            Debug.Log($"ACTION: Restored all defense values for Player {_sideIndex}.");
        }
    }

    public class RestoreCardDefense : MatchAction
    {
        private readonly int _cardId;

        public RestoreCardDefense(int cardId)
        {
            _cardId = cardId;
        }

        public override void Execute(Match match)
        {
            RuntimeCard card = match.FindRuntimeCardById(_cardId, match.ActiveSideIndex);
            card.SetDefense(card.BaseDefense);
        }
    }
}
*/