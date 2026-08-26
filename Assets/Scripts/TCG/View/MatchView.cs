using System;
using System.Collections.Generic;
using TCG.Model.Core;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TCG.View
{
    public class MatchView : MonoBehaviour
    {
        [FormerlySerializedAs("_text")] [SerializeField] private Text text;
        [FormerlySerializedAs("_StateText")] [SerializeField] private Text stateText;
        [FormerlySerializedAs("_logs")] [SerializeField] private Text logs;
        [FormerlySerializedAs("_moveButtonContainer")] [SerializeField] private Transform moveButtonContainer;
        [FormerlySerializedAs("_buttonPrefab")] [SerializeField] private Button buttonPrefab;
        
        public Action<PlayerInput> OnInputSubmitted;
        private List<PlayerMove> _moves;
        private bool _isWaitingForInput = false;
        
        
        [SerializeField] private GameDisplay gameDisplay;
        
        public void ShowState(string state)
        {
            stateText.text = state;
        }

        public void ShowMoves(List<PlayerMove> moves)
        {
            for (int i = 0; i < moves.Count; i++)
            {
                PlayerMove move = moves[i];
                int index = i;
                Button btn = Instantiate(buttonPrefab, moveButtonContainer);
                btn.GetComponentInChildren<Text>().text = move.ToDisplayString();
                btn.onClick.AddListener(() => SendInput(new PlayerInput(index)));
            }
        }
    
        public void AddLog(string log)
        {
            logs.text += "\n" +log;
        }

        public void DisplayGameDisplay(GameDisplayData gameDisplayData)
        {
            gameDisplay.Display(gameDisplayData);
            //gameDisplayData.LegalMoves
        }

        public void ShowOutput(string output)
        {
            text.text = output;
            _isWaitingForInput = true;
        }

        private void SendInput(PlayerInput input)
        {
            if (_isWaitingForInput)
            {
                _isWaitingForInput = false;
                foreach (Transform child in moveButtonContainer)
                {
                    Destroy(child.gameObject);
                }
                OnInputSubmitted?.Invoke(input);
            }
        }
    }
}