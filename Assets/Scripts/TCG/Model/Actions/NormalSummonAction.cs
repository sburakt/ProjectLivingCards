using TCG.Model.Cards;
using TCG.Model.Core;
using TCG.Model.Effects;
using TCG.Model.Events;
using UnityEngine;

namespace TCG.Model.Actions
{
    public class NormalSummonAction : MatchAction
    {
        // todo: rewrite the whole thing add simple summon

        private readonly int _cardId;
        private readonly Position _position;
        private int _summonerSideIndex;
        //private int _laneIndex;
        //private bool _playToFront;

        public NormalSummonAction(int summonerSideIndex, int cardId, int laneIndex, bool playToFront)
        {
            _summonerSideIndex = summonerSideIndex;
            _cardId = cardId;
            //_laneIndex = laneIndex;
            //playToFront = playToFront;
        }

        public NormalSummonAction(int summonerSideIndex, int cardId, Position position)
        {
            _summonerSideIndex = summonerSideIndex;
            _cardId = cardId;
            _position = position;
        }

        public override void Execute(Match match)
        {
            // Side side = match.Sides[_summonerSideIndex];
            // Lane targetLane = side.Field.Lanes[_position.LaneIndex];
            //
            // // fizzle check if the lane is completely full
            // if (targetLane.IsFull)
            // {
            //     Debug.Log($"ERROR: Lane {_position.LaneIndex} is completely full!");
            //     return;
            // }
            // // todo fizzle check if the card is not in hand
            //
            // RuntimeCard card = match.FindRuntimeCardById(_cardId, _summonerSideIndex);
            //
            //
            // if (_position.IsFront)
            // {
            //     //summon card
            //     //push card to back
            //     if (targetLane.FrontCard != null)
            //     {
            //         // Push existing front card to the back
            //         targetLane.BackCard = targetLane.FrontCard;
            //         targetLane.BackCard.SetPosition(new Position()
            //             { SideIndex = _summonerSideIndex, LaneIndex = _laneIndex, IsFront = false });
            //         Debug.Log($"Notice: {targetLane.BackCard.StaticCard.CardId} was pushed to the Back row.");
            //     }
            //
            //     targetLane.FrontCard = card;
            //     targetLane.FrontCard.SetPosition(new Position()
            //         { SideIndex = _summonerSideIndex, LaneIndex = _laneIndex, IsFront = true });
            //
            // }
            // else // Playing to the back
            // {
            //     if (targetLane.BackCard != null)
            //     {
            //         // Push existing back card to the front
            //         targetLane.FrontCard = targetLane.BackCard;
            //         targetLane.FrontCard.SetPosition(new Position()
            //             { SideIndex = _summonerSideIndex, LaneIndex = _laneIndex, IsFront = true });
            //         Debug.Log($"Notice: {targetLane.FrontCard.StaticCard.CardId} was pushed to the Front row.");
            //     }
            //
            //     targetLane.BackCard = card;
            //     targetLane.BackCard.SetPosition(new Position()
            //         { SideIndex = _summonerSideIndex, LaneIndex = _laneIndex, IsFront = false });
            // }
            //
            // // 5. Finalize the play
            // summonerSide.Hand.Remove(card);
            // card.SetState(RuntimeCard.CardState.OnBoard);
            // match.EnqueueEvent(new MatchEvent()
            // {
            //     Type = MatchEventType.CardPlayed,
            //     SourceId = _cardId,
            // });
            //
            // // 6. Add card's effect to the match's effect list
            // // dont know yet if all effect be active when a card summoned some card have effect from hand like winged kuriboh
            // // might need activate on summon interface future
            // foreach (Effect effect in card.CardEffects)
            // {
            //     match.AddEffect(effect);
            //     Debug.Log($"Notice: {card.StaticCard.EffectIDs[0]} was added to the Card effect.");
            // }
            //
            // string positionStr = _playToFront ? "Front" : "Back";
            // Debug.Log(
            //     $"ACTION: Player {_summonerSideIndex} played {card.StaticCard.CardId} to Lane {_laneIndex} ({positionStr})");
        }
    }
}