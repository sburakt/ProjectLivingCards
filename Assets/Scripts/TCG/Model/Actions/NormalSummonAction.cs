using TCG.Model.Cards;
using TCG.Model.Core;
using TCG.Model.Effects;
using TCG.Model.Events;
using UnityEngine;

namespace TCG.Model.Actions
{
    public class NormalSummonAction : MatchAction
    {
        // todo: extract push mechanic to be its own action 
        
        private int _summonerSideIndex;
        private int _cardId;
        private int _laneIndex;
        private bool _playToFront;

        public NormalSummonAction(int summonerSideIndex, int cardId, int laneIndex, bool playToFront)
        {
            _summonerSideIndex = summonerSideIndex;
            _cardId = cardId;
            _laneIndex = laneIndex;
            _playToFront = playToFront;
        }

        public override void Execute(Match match)
        {
            Side summonerSide = match.Sides[_summonerSideIndex];
            // 2. Validate the lane bounds
            if (_laneIndex < 0 || _laneIndex >= summonerSide.Field.Lanes.Length)
            {
                Debug.Log("lane bound is not valid");
                return;
            }

            Lane targetLane = summonerSide.Field.Lanes[_laneIndex];

            // 3. Check if the lane is completely full
            if (targetLane.IsFull())
            {
                Debug.Log($"ERROR: Lane {_laneIndex} is completely full!");
                return;
            }

            RuntimeCard card = match.FindRuntimeCardById(_cardId, _summonerSideIndex);
            // 4. Handle the push mechanic!
            if (_playToFront)
            {
                if (targetLane.FrontCard != null)
                {
                    // Push existing front card to the back
                    targetLane.BackCard = targetLane.FrontCard;
                    targetLane.BackCard.SetPosition(new Position()
                        { SideIndex = _summonerSideIndex, LaneIndex = _laneIndex, IsFront = false });
                    Debug.Log($"Notice: {targetLane.BackCard.StaticCard.CardId} was pushed to the Back row.");
                }

                targetLane.FrontCard = card;
                targetLane.FrontCard.SetPosition(new Position()
                    { SideIndex = _summonerSideIndex, LaneIndex = _laneIndex, IsFront = true });

            }
            else // Playing to the back
            {
                if (targetLane.BackCard != null)
                {
                    // Push existing back card to the front
                    targetLane.FrontCard = targetLane.BackCard;
                    targetLane.FrontCard.SetPosition(new Position()
                        { SideIndex = _summonerSideIndex, LaneIndex = _laneIndex, IsFront = true });
                    Debug.Log($"Notice: {targetLane.FrontCard.StaticCard.CardId} was pushed to the Front row.");
                }

                targetLane.BackCard = card;
                targetLane.BackCard.SetPosition(new Position()
                    { SideIndex = _summonerSideIndex, LaneIndex = _laneIndex, IsFront = false });
            }

            // 5. Finalize the play
            summonerSide.Hand.Remove(card);
            card.State = RuntimeCard.CardState.OnBoard;
            match.EnqueueEvent(new MatchEvent()
            {
                Type = MatchEventType.CardPlayed,
                SourceId = _cardId,
            });
            
            // 6. Add card's effect to the match's effect list
            // dont know yet if all effect be active when a card summoned some card have effect from hand like winged kuriboh
            // might need activate on summon interface future
            foreach (Effect effect in card.CardEffects)
            {
                match.AddEffect(effect);
                Debug.Log($"Notice: {card.StaticCard.EffectIDs[0]} was added to the Card effect.");
            }

            string positionStr = _playToFront ? "Front" : "Back";
            Debug.Log(
                $"ACTION: Player {_summonerSideIndex} played {card.StaticCard.CardId} to Lane {_laneIndex} ({positionStr})");
        }
    }
}