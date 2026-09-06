using System.Collections.Generic;
using UnityEngine;

namespace TCG.View
{
    public class FieldDisplay : MonoBehaviour
    {
        [SerializeField] CardDisplay cardDisplay;
        [SerializeField] Transform[] sloths;
        
        private CardDisplay[] _occupants = new CardDisplay[6];
        private CardDisplay _cardPrefab;
        
        public void Display(CardSnapshot[] data)
        {
            ClearDisplay();
            for (int i = 0; i < 6; i++)
            {
                if (data[i] != null)
                {
                    CardDisplay card = Instantiate(cardDisplay, sloths[i]);
                    card.Display(data[i]);
                }
            }
        }

        public void ClearDisplay()
        {
            foreach (Transform t in sloths)
            {
                if (t.childCount>0)
                    Destroy(t.GetChild(0).gameObject);
            }
        }
    }
}
