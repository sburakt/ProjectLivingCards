using System.Collections;
using System.Collections.Generic;
using TCG.Model.Core;
using UnityEngine;

namespace TCG
{  
    public abstract class PlayerMove 
     { 
         public int PlayerIndex { get; protected set; }
         public abstract string ToDisplayString();

     }

     public class PlayCardMove : PlayerMove
     {
        public int CardInstanceId { get; }
        public Position Position;
        public PlayCardMove(int playerIndex, int cardId, Position position)
        {
            PlayerIndex = playerIndex;
            CardInstanceId = cardId;
            Position = position;
        }

        public override string ToDisplayString()
        {
            string backOfFront = Position.IsFront ? "front" : "back";
            return $"  Play {CardInstanceId} to lane {Position.LaneIndex}, {backOfFront}";
        }

     }

     public class EndTurnMove : PlayerMove
     {
        public EndTurnMove(int playerIndex)
        {
            PlayerIndex = playerIndex;
        }

        public override string ToDisplayString()
        {
            return "end turn";
        }
     }
}