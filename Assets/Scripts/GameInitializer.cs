using System;
using System.Collections;
using System.Collections.Generic;
using TCG.Model.Cards;
using TCG.Model.Core;
using TCG.Presenter;
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
    //[SerializeField] private List<PersistentCard> EnemyDeck;
    //Queue<string> stateQueue = new Queue<string>();
    //Queue<string> outputQueue = new Queue<string>();
    //Queue<string> inputQueue = new Queue<string>();
    private bool _isWaitingForInput = false;
    Match _match;
    MatchPresenter _presenter;
    private void Start()
    {
        Debug.Log("pre Game initialized");
        Initialize();
    }

    public Action<string> OnMoveSubmitted;
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
        _match = new Match(gamePlayer, enemyPlayer);  //,outputQueue,inputQueue, stateQueue);
        // create presenter
        _presenter = new MatchPresenter(_match,this);

        // start match
        _presenter.StartMatch();
    }

    public void ShowState(string state)
    {
        _StateText.text = state;
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

    public void SendMove(string move)
    {
        OnMoveSubmitted?.Invoke(move);
    }

    public void Update()
    {
        if (_isWaitingForInput)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                _isWaitingForInput = false;
                SendMove(_inputField.text);
            }
        }
    }
}
