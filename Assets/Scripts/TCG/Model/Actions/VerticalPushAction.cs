using TCG.Model.Cards;
using TCG.Model.Core;

namespace TCG.Model.Actions
{
    public class VerticalPushAction : MatchAction
    {
        private readonly int _pusherCardId;
        private readonly int _pushedCardId;
        private readonly VerticalPushDirection _direction;
        
        public VerticalPushAction(int pusherCardId, int pushedCardId, VerticalPushDirection direction)
        {
            _pusherCardId = pusherCardId;
            _pushedCardId = pushedCardId;
            _direction = direction;
        }
        
        // todo code repeats cascading push part
        public override void Execute(Match match)
        {
            RuntimeCard pushedCard = match.FindRuntimeCardById(_pushedCardId);
            Position beforePushPosition = pushedCard.Position;
            Cell beforePushCell = match.GetCell(beforePushPosition);
            
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
                    MoveCardAction moveCardToFrontAction = new MoveCardAction(_pushedCardId, afterPushPosition);
                    match.PushAction(moveCardToFrontAction);
                    //check if the front is full
                    Cell afterPushCell = match.GetCell(afterPushPosition);
                    if (afterPushCell.IsFull)
                    {
                        int frontCardId = afterPushCell.Card.InstanceId;
                        VerticalPushAction cascadedVerticalPushAction = new VerticalPushAction(_pushedCardId, frontCardId, _direction);
                        match.PushAction(cascadedVerticalPushAction);
                    }
                }
                else // if (beforePushPosition.IsFront)
                {
                    DestroyCardAction destroyCardByPushAction = new DestroyCardAction(_pushedCardId, beforePushPosition.SideIndex);
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
                    DestroyCardAction destroyCardByPushAction = new DestroyCardAction(_pushedCardId, beforePushPosition.SideIndex);
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
                    MoveCardAction moveCardToFrontAction = new MoveCardAction(_pushedCardId, afterPushPosition);
                    match.PushAction(moveCardToFrontAction);
                    //check if the front is full
                    Cell afterPushCell = match.GetCell(afterPushPosition);
                    if (afterPushCell.IsFull)
                    {
                        int frontCardId = afterPushCell.Card.InstanceId;
                        VerticalPushAction cascadedVerticalPushAction = new VerticalPushAction(_pushedCardId, frontCardId, _direction);
                        match.PushAction(cascadedVerticalPushAction);
                    }
                }
            }
            //Event
            // todo emit events
        }
        
        public enum VerticalPushDirection
        {
            Front,
            Back
        }
    }
}