using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TCG.View
{
    public class SideDisplay : MonoBehaviour
    {
        [SerializeField] private HandDisplay handDisplay;
        [SerializeField] private TextMeshPro lifePoint;
        [SerializeField] private FieldDisplay fieldDisplay;

        public void Display(SideSnapshot data, BoardSnapshot board)
        {
            lifePoint.text = data.LifePoints.ToString();
            List<CardSnapshot> hand = new List<CardSnapshot>();
            List<CardSnapshot> field = new List<CardSnapshot>();             
            foreach (int id in data.Hand)
            {
                hand.Add(board.Cards[id]);
            }
            foreach (int id  in data.Field)
            {
                if (id > 0)
                    field.Add(board.Cards[id]);
                else
                    field.Add(null);
            }
            handDisplay.Display(hand);
            fieldDisplay.Display(field.ToArray());
        }
    }
}
