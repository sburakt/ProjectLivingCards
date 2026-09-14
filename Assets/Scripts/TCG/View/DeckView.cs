using UnityEngine;

namespace TCG.View
{
    public class DeckView : MonoBehaviour
    {
        public void AddCardToTop(CardView cardView)
        {
            cardView.transform.SetParent(transform);
            cardView.transform.localPosition = Vector3.zero;
            cardView.transform.localRotation = Quaternion.Euler(0,0,180);
        }
    }
    
}