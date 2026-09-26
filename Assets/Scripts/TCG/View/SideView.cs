using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace TCG.View
{
    public class SideView : MonoBehaviour
    {
        [FormerlySerializedAs("handDisplay")] [SerializeField] public HandView handView;
        [SerializeField] public DeckView deckView;
        [SerializeField] private TextMeshPro lifePoint;
        [SerializeField] public FieldDisplay fieldDisplay;

        private int _lifePoint = 0;
        private int LifePointValue
        {
            set
            {
                if (value > 0)
                    _lifePoint = value;
                else
                    _lifePoint = 0;
                lifePoint.text = value.ToString();
            }
            get { return _lifePoint; }
        }


        public void SetLP(int value)
        {
            LifePointValue = value;
        }

        public void DecreaseLP(int value)
        {
            LifePointValue -= value;
        }

        public void IncreaseLP(int value)
        {
            LifePointValue += value;
        }
    }
}
