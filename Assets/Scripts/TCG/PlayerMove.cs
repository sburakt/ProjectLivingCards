using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCG
{  
    public abstract class PlayerMove 
     { 
         public int PlayerIndex { get; protected set; }
     }

     public class PlayCardMove : PlayerMove
     {
        public int CardInstanceId { get; }
        public int LaneIndex { get; }
        public bool IsFront { get; }

        public PlayCardMove(int playerIndex, int cardId, int lane, bool isFront)
        {
            PlayerIndex = playerIndex;
            CardInstanceId = cardId;
            LaneIndex = lane;
            IsFront = isFront;
        }
     }

     public class EndTurnMove : PlayerMove
     {
        public EndTurnMove(int playerIndex)
        {
            PlayerIndex = playerIndex;
        }
     }
}