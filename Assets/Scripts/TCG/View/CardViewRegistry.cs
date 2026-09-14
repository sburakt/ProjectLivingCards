using System.Collections.Generic;
using UnityEngine;

namespace TCG.View
{
    public class CardViewRegistry: MonoBehaviour
    {
        [SerializeField] private CardView cardViewPrefab;
        private Dictionary<int, CardView> cards = new Dictionary<int, CardView>();
        
        public CardView GetCard(int cardId)
        {
            cards.TryGetValue(cardId, out CardView cardView);
            return cardView;
        }

        public CardView GetOrCreateCard(int cardId, CardSnapshot snapshot)
        {
            cards.TryGetValue(cardId, out CardView cardView);
            if (cardView != null)
                return cardView;
            CardView newCardView = Instantiate(cardViewPrefab, transform);
            newCardView.SetData(snapshot);
            cards.Add(cardId, newCardView);
            return newCardView;
        }

        public void Remove(CardView cardView)
        {
            throw new System.NotImplementedException();
        }
    }
}