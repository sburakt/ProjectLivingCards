using System.Collections.Generic;
using TCG.Model.Cards;
using TCG.Model.Core;
using TCG.Presenter;
using TCG.View;
using UnityEngine;

public class MatchInitializer : MonoBehaviour
{
    [SerializeField] private List<PersistentCard> playerDeck;
    [SerializeField] private MatchView view;
    
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
        gamePlayer.PersistentDeck = playerDeck;
        enemyPlayer.PersistentDeck = playerDeck;

        // create match
        Match match = new Match(gamePlayer, enemyPlayer);
        
        // create presenter
        MatchPresenter presenter = new MatchPresenter(match,view);

        // start match
        presenter.StartMatch();
    }
}
