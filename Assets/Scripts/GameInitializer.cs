using System;
using System.Collections;
using System.Collections.Generic;
using TCG.Model.Cards;
using TCG.Model.Core;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private List<PersistentCard> PlayerDeck;
    [SerializeField] private Text _text;
    [SerializeField] private Text _StateText;
    [SerializeField] private InputField _inputField;
    //[SerializeField] private List<PersistentCard> EnemyDeck;
    Queue<string> stateQueue = new Queue<string>();
    Queue<string> outputQueue = new Queue<string>();
    Queue<string> inputQueue = new Queue<string>();
    bool isWaitingForInput = false;
    Match _match;
    private void Start()
    {
        Debug.Log("pre Game initialized");
        Initialize();
    }


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
        _match = new Match(gamePlayer,enemyPlayer,outputQueue,inputQueue, stateQueue);

        // start match
    }

    public void Update()
    {
        if (stateQueue.Count > 0)
        {
            string output = stateQueue.Dequeue();
            _StateText.text = output;
        }
        if (outputQueue.Count > 0 && !isWaitingForInput)
        {
            isWaitingForInput = true;
            string output = outputQueue.Dequeue();
            _text.text = output;
        }
        else if (isWaitingForInput)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                inputQueue.Enqueue(_inputField.text);
                isWaitingForInput = false;
                _match.LogicLoop();
            }
        }
    }
}
