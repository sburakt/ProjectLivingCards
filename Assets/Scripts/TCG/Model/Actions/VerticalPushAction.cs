using TCG.Model.Cards;
using TCG.Model.Core;
using TCG.Model.Enums;
using TCG.Model.Events;

namespace TCG.Model.Actions
{
    // push targets card ID not cell
    // edge case if an effect moves the pushed card to a different lane
    // before this action resolves push follows the card across lanes
    // this requires a very specific chain and is acceptable at current scope
    // revisit if a card mechanic exploits this
    // or add cell based vertical push only for cascade effects
    public class VerticalPushAction : MatchAction
    {
        private readonly int _pusherCardId;
        private readonly int _pushedCardId;
        private readonly VerticalPushDirection _direction;
        
        public VerticalPushAction(int pusherCardId, int pushedCardId, VerticalPushDirection direction, int groupId)
        {
            _pusherCardId = pusherCardId;
            _pushedCardId = pushedCardId;
            _direction = direction;
            GroupId = groupId;
        }
        
        // todo code repeats cascading push part
        public override void Execute(Match match)
        {
            base.UpdateGroupID(match);
            RuntimeCard pushedCard = match.FindRuntimeCardById(_pushedCardId);
            Position beforePushPosition = pushedCard.Position;
            
            //fizzle check
            if (pushedCard.State != RuntimeCard.CardState.OnBoard)
            {
                return;
            }
            
            if (_direction == VerticalPushDirection.Front)
            {
                if (beforePushPosition.IsBack)
                {
                    Position afterPushPosition = new Position()
                    {
                        SideIndex = beforePushPosition.SideIndex,
                        LaneIndex = beforePushPosition.LaneIndex,
                        IsFront = true
                    };
                    // move to new position
                    MoveCardAction moveCardToFrontAction = new MoveCardAction(_pushedCardId, afterPushPosition, GroupId);
                    match.PushAction(moveCardToFrontAction);
                    //check if the front is full
                    Cell afterPushCell = match.GetCell(afterPushPosition);
                    if (afterPushCell.IsFull)
                    {
                        int frontCardId = afterPushCell.Card.InstanceId;
                        VerticalPushAction cascadedVerticalPushAction = new VerticalPushAction(_pushedCardId, frontCardId, _direction,GroupId);
                        match.PushAction(cascadedVerticalPushAction);
                    }
                }
                else // if (beforePushPosition.IsFront)
                {
                    DestroyCardAction destroyCardByPushAction = new DestroyCardAction(_pusherCardId, _pushedCardId, DestroyCause.Push,GroupId);
                    match.PushAction(destroyCardByPushAction);
                }
                // note for myself bc i forget: its stack so move + push actually first pushes then removes in the game
            }
            else // if (_direction == VerticalPushDirection.Back)
            {
                if (beforePushPosition.IsBack)
                {
                    //todo maybe cards that are pushed back should return to hand instead of dying?
                    //so if never played to front they can be saved
                    //gives incentive to play back which is not a good move but idk
                    //i should test the game once finished
                    DestroyCardAction destroyCardByPushAction = new DestroyCardAction(_pusherCardId, _pushedCardId, DestroyCause.Push,GroupId);
                    match.PushAction(destroyCardByPushAction);
                }
                else // if (beforePushPosition.IsFront)
                {
                    Position afterPushPosition = new Position()
                    {
                        SideIndex = beforePushPosition.SideIndex,
                        LaneIndex = beforePushPosition.LaneIndex,
                        IsBack = true
                    };
                    // move to new position
                    MoveCardAction moveCardToFrontAction = new MoveCardAction(_pushedCardId, afterPushPosition, GroupId);
                    match.PushAction(moveCardToFrontAction);
                    //check if the front is full
                    Cell afterPushCell = match.GetCell(afterPushPosition);
                    if (afterPushCell.IsFull)
                    {
                        int frontCardId = afterPushCell.Card.InstanceId;
                        VerticalPushAction cascadedVerticalPushAction = new VerticalPushAction(_pushedCardId, frontCardId, _direction, GroupId);
                        match.PushAction(cascadedVerticalPushAction);
                    }
                }
            }
            // TODO: cascade push uses _pushedCardId as pusher for visual continuity
            // but _pusherCardId should be for gameplay logic
            // for "when this card pushes another card out" effects
            // view will need both: VisualPusher the physically moving card
            // and LogicalPusher the original initiator

            //Event
            match.EnqueueEvent(new PushedEvent(_pushedCardId, _pusherCardId, GroupId));
        }
        
        
    }
}