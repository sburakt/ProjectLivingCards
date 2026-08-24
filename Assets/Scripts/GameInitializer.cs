using System;
using System.Collections;
using System.Collections.Generic;
using TCG;
using TCG.Model.Cards;
using TCG.Model.Core;
using TCG.Presenter;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private List<PersistentCard> PlayerDeck;
    [SerializeField] private Text _text;
    [SerializeField] private Text _StateText;
    [SerializeField] private InputField _inputField;
    [SerializeField] private Text _logs;
    [SerializeField] private Transform _moveButtonContainer;
    [SerializeField] private Button _buttonPrefab;
    
    private List<PlayerMove> _moves;
    private bool _isWaitingForInput = false;
    Match _match;
    MatchPresenter _presenter;
    private void Start()
    {
        Debug.Log("pre Game initialized");
        Initialize();
    }

    public Action<PlayerInput> OnInputSubmitted;
    public void Initialize()
    {
        // get deck json create static cards
        JsonLoader.Load();

        // create players
        Player gamePlayer = new Player();
        Player enemyPlayer = new Player();
        gamePlayer.PersistentDeck = PlayerDeck;
        enemyPlayer.PersistentDeck = PlayerDeck;

        // create match
        _match = new Match(gamePlayer, enemyPlayer);
        // create presenter
        _presenter = new MatchPresenter(_match,this);

        // start match
        _presenter.StartMatch();
    }

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
