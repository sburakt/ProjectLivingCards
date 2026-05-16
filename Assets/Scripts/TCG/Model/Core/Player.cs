using System.Collections.Generic;
using TCG.Model.Cards;

namespace TCG.Model.Core
{
    public class Player
    {
        public string PlayerName;
        public List<PersistentCard> PersistentDeck;
    }
}