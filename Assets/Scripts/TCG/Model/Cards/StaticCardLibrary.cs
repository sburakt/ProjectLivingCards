using System.Collections.Generic;

namespace TCG.Model.Cards
{
    public static class StaticCardLibrary
    {
        static StaticCardLibrary()
        {
            CardLibrary = new Dictionary<string, StaticCard>();
        }
    
        public static Dictionary<string, StaticCard> CardLibrary { get; private set; }

        public static void AddCard(StaticCard card)
        {
            CardLibrary[card.CardId] = card;
        }
    }
}