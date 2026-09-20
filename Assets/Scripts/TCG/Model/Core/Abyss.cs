using TCG.Model.Cards;
using UnityEngine;

namespace TCG.Model.Core
{
    public class Abyss
    {
        public AbyssCell[][] AbyssCell { get; private set; }
        

        public Abyss()
        {
            AbyssCell[] sideAbyss0 = new AbyssCell[3]
            {
                new AbyssCell(0,0),
                new AbyssCell(0,1),
                new AbyssCell(0, 2)
            };
            AbyssCell[] sideAbyss1 = new AbyssCell[3]{
                new AbyssCell(1,0),
                new AbyssCell(1,1),
                new AbyssCell(1,2)
            };
            AbyssCell[] midAbyss = new AbyssCell[3]
            {
                new AbyssCell(2,0),
                new AbyssCell(2,1),
                new AbyssCell(2,2)
            };
            AbyssCell = new AbyssCell[3][] { sideAbyss0, sideAbyss1, midAbyss};
        }
        
    }

    public class AbyssCell
    {
        public RuntimeCard Card { get; private set; }
        public int AbyssIndex { get; private set; }
        public int LaneIndex { get; private set; }
        public AbyssCell(int abyssIndex, int laneIndex)
        {
            AbyssIndex = abyssIndex;
            LaneIndex = laneIndex;
        }
        
        public RuntimeCard RemoveCard()
        {
            RuntimeCard card = Card;
            Card = null;
            return card;
        }
        
        public void SetCard(RuntimeCard card)
        {
            if (Card == null)
                Card = card;
            else
                Debug.LogWarning("A Card is Possibly Destroyed");
        }
    }
    
}