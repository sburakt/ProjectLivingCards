using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

namespace TCG.View
{
    public class HandDisplay : MonoBehaviour
    {
        [SerializeField] private CardDisplay cardDisplay;
        HashSet<int> _inHandIds = new HashSet<int>();
        private List<CardDisplay> _cardDisplays = new List<CardDisplay>();
        
        public void Display(List<CardSnapshot> cards)
        {
            ClearDisplay(cards); 
            foreach (CardSnapshot cardDisplayData in cards)
            {
                if (_inHandIds.Add(cardDisplayData.InstanceId))
                {
                    Instantiate(cardDisplay, gameObject.transform).Display(cardDisplayData);
                }                
            }    
        }

        //Clear all card in _cardDisplays that are not in the cards
        public void ClearDisplay(List<CardSnapshot> cards)
        {
            foreach (CardDisplay cardDisplay in _cardDisplays)
            {
                if (!_inHandIds.Contains(cardDisplay.CardSnapshot.InstanceId))
                {
                    Destroy(cardDisplay.gameObject);
                }
            }
        }
    }
}