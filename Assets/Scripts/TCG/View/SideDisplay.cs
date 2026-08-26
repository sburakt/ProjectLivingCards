using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace TCG.View
{
    public class SideDisplay : MonoBehaviour
    {
        [SerializeField] private HandDisplay handDisplay;
        [SerializeField] private TextMeshPro lifePoint;
        [SerializeField] private FieldDisplay fieldDisplay;

        public void Display(SideDisplayData data)
        {
            lifePoint.text = data.LifePoints.ToString();
            handDisplay.Display(data.Hand);
            fieldDisplay.Display(data.Field);
        }
    }
}
