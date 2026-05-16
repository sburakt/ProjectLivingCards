using System.Collections.Generic;

namespace TCG.Model.Cards
{
    [System.Serializable]
    public class StaticCard
    {
        public string CardId;
        public int BaseAttack;
        public int BaseDefense;
        public int BaseHealth;
        public List<string> EffectIDs;
    }
}
