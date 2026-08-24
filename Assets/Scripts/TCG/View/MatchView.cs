using System;
using UnityEngine;

namespace TCG.View
{
    public class MatchView : MonoBehaviour
    {
        [SerializeField] private HandDisplay handDisplay;
        
        public Action<PlayerMove> OnMoveSubmitted;
        
        private void Start()
        {

        }

        private void Update()
        {

        }
    }
}