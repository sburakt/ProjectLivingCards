using TCG.Model.Cards;
using UnityEngine;

namespace TCG.Model.Core
{
    public class Cell
    {
        public readonly Position Position;
        public RuntimeCard Card {get; private set;}
        
        public bool IsEmpty => Card == null;
        public bool IsFull => Card != null;

        public Cell(Position position)
        {
            Position = position;
        }

        public RuntimeCard RemoveCard()
        {
            RuntimeCard card = Card;
            Card = null;
            return card;
        }
        
        public void SetCard(RuntimeCard card)
        {
            if (Card == null)
                Card = card;
            else
                Debug.LogWarning("A Card is Possibly Destroyed");
        }
    
    }
}