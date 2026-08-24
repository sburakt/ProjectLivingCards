using System;
using System.Collections.Generic;
using TCG.Model.Core;
using UnityEngine;
using UnityEngine.UI;

namespace TCG.View
{
    public class MatchView : MonoBehaviour
    {
        [SerializeField] private Text _text;
        [SerializeField] private Text _StateText;
        [SerializeField] private Text _logs;
        [SerializeField] private Transform _moveButtonContainer;
        [SerializeField] private Button _buttonPrefab;
        
        public Action<PlayerInput> OnInputSubmitted;
        private List<PlayerMove> _moves;
        private bool _isWaitingForInput = false;
        
        
        [SerializeField] private HandDisplay handDisplay;
        
        public void ShowState(string state)
        {
            _StateText.text = state;
        }

        public void ShowMoves(List<PlayerMove> moves)
        {
            for (int i = 0; i < moves.Count; i++)
            {
                PlayerMove move = moves[i];
                int index = i;
                Button btn = Instantiate(_buttonPrefab, _moveButtonContainer);
                btn.GetComponentInChildren<Text>().text = move.ToDisplayString();
                btn.onClick.AddListener(() => SendInput(new PlayerInput(index)));
            }
        }
    
        public void AddLog(string log)
        {
            _logs.text += "\n" +log;
        }

        public void ShowOutput(string output)
        {
            _text.text = output;
            _isWaitingForInput = true;
        }

        private void SendInput(PlayerInput input)
        {
            if (_isWaitingForInput)
            {
                _isWaitingForInput = false;
                foreach (Transform child in _moveButtonContainer)
                {
                    Destroy(child.gameObject);
                }
                OnInputSubmitted?.Invoke(input);
            }
        }
    }
}