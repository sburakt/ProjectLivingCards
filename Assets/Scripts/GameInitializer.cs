using System;
using System.Collections;
using System.Collections.Generic;
using TCG;
using TCG.Model.Cards;
using TCG.Model.Core;
using TCG.Presenter;
using TCG.View;
using UnityEngine;
using UnityEngine.UI;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private List<PersistentCard> PlayerDeck;
    [SerializeField] private MatchView _view;
    
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
        _presenter = new MatchPresenter(_match,_view);

        // start match
        _presenter.StartMatch();
    }
}
