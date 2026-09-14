using System;
using System.Collections;
using System.Collections.Generic;
using TCG.Model.Cards;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GridField
{
    public RuntimeCard[,] RuntimeCards{get; private set;}

    public void initRuntimeCards()
    {
        RuntimeCards = new RuntimeCard[2,3];
    }

    public void playCard(RuntimeCard card, Tuple<int,int> coordinates)
    {
        if (RuntimeCards[coordinates.Item1,coordinates.Item2] == null)
            RuntimeCards[coordinates.Item1,coordinates.Item2] = card;
    }
}

