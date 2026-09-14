using System.Collections.Generic;
using TCG.Model.Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace TCG.View
{
    public class FieldDisplay : MonoBehaviour
    {
        [SerializeField] Transform[] sloths;
        [SerializeField] CellView[] cellViews;


        public CellView GetCell(Position position)
        {
            int cellNumber = position.IsFront? (position.LaneIndex * 2) : (position.LaneIndex * 2) + 1;
            return cellViews[cellNumber];
        }
    }
}
