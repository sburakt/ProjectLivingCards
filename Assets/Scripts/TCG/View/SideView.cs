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

    }
}
