using TCG.Model.Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace TCG.View
{
    public class CellView: MonoBehaviour, ICardViewContainer
    {
        [FormerlySerializedAs("Position")] [SerializeField] private Position position;
        public Position Position => position;
        public CardView CardView { get; private set; }
        
        public bool occupied = false;

        public void AddCard(CardView cardView)
        {
            CardView = cardView;
            occupied = true;
            cardView.transform.SetParent(transform);
        }

        public void RemoveCard(CardView cardView)
        {
            if (CardView == cardView)
            {
                CardView = null;
                occupied = false;
            }
        }
    
    }
}