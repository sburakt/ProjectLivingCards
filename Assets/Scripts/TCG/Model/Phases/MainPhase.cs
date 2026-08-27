using System.Collections.Generic;
using TCG.Model.Cards;
using TCG.Model.Core;
using TCG.Model.Actions;

using UnityEngine;
using System.Linq;

namespace TCG.Model.Phases
{
    public class MainPhase : TurnPhase
    {
        public static readonly MainPhase Instance = new MainPhase();

        private MainPhase() : base()
        { }
    
        public override void Enter(Match match)
        {
            Debug.Log("Main1 Phase Enter");
        }
    
        public override void Execute(Match match)
        {
            PlayerMove move = match.ConsumeMove();
            
            if (move == null)
            {
                match.RequestInput(BuildInputRequest(match));
                return;
            }
            if (move is PlayCardMove playCardMove)
            {
                PlayCardFromHand(match, playCardMove.CardInstanceId, playCardMove.Position);
                return;
            }
            if (move is EndTurnMove endTurnMove)
            {
                match.AdvancePhase();
                return;
            }
            //if (move is ActivateEffectMove activateEffectMove) {}
        }

        private InputRequest BuildInputRequest(Match match)
        {
            InputRequest inputRequest = new InputRequest()
            {
                DisplayMessage = "Main Phase Input",
                LegalMoves = GetLegalMoves(match)
            };
            return inputRequest;
        }
        
        private List<PlayerMove> GetLegalMoves(Match match)
        {
            List<PlayerMove> moves = new List<PlayerMove>();
            // normal summon from hand
            foreach (RuntimeCard card in match.ActiveSide.Hand)
            {
                foreach (Cell cell in match.ActiveSide.Field.GetCells())
                {   
                    Position position = cell.Position;
                    int cardId = card.InstanceId;
                    CanSummonToPosition(match,cardId, position);
                    moves.Add(new PlayCardMove(match.ActiveSideIndex, cardId, position));
                }
            }
            // end turn
            moves.Add(new EndTurnMove(match.ActiveSideIndex));
            return moves;
        }
        
        // will be make into a  another class for validating moves once the rules for the game finalized and will be more complex
        private bool CanSummonToPosition(Match match, int cardId, Position position)
        {
            // check if the card is still in hand
            RuntimeCard card = match.ActiveSide.Hand.Find(card => card.InstanceId == cardId);
            if (card == null)
                return false;
            // check if the card can be played in the side
            // later some card might be played for the enemy side
            if (position.SideIndex != match.ActiveSideIndex)
                return false;
            Lane summonLane = match.Sides[position.SideIndex].Field.Lanes[position.LaneIndex];
            // if lane is full cannot summon 
            if(summonLane.IsFull)
                return false;
            return true;
        }

        public override void Exit(Match match)
        {
            Debug.Log("Main Phase Exit");
        }
        
        public override TurnPhase GetNextPhase()
        {
            return BattlePhase.Instance;
        }
        
        private void PlayCardFromHand(Match match, int instanceId, Position position)
        {
            Side activeSide = match.Sides[match.ActiveSideIndex];

            // 1. Validate the card exists in hand
            RuntimeCard cardToPlay = activeSide.Hand.Find(c => c.InstanceId == instanceId);
            if (cardToPlay == null)
            {
                // Validation failed! 
                // Send the error to the UI/Console queue
               
                //match.RequestStringInput($"Instance ID {instanceId} is not found in your hand. there are {activeSide.Hand.Count}" + $"\n cards in your hand. {string.Join(", ", activeSide.Hand.Select(p => p.InstanceId))}" );
                
                // just fizzle instead
                return;
            }
            // 2. Validation passed! Pass the execution down to the mechanic.
            match.PushAction( new NormalSummonAction(match.ActiveSideIndex, instanceId, position.LaneIndex, position.IsFront));
        }
    }
}



