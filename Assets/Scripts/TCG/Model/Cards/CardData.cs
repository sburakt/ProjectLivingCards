using UnityEngine;

namespace TCG.Model.Cards
{
    [CreateAssetMenu(fileName = "NewCard", menuName = "LivingCards/CardData")]
    public class CardData : ScriptableObject 
    {
        [Header("Identity")]
        public string cardName;
        public Sprite artwork;

        [Header("Base Stats")]
        public int baseHealth;
        public int baseArmor;
        public int attackPower;

        [Header("Abilities")]
        [TextArea] public string frontAbilityDescription;
        [TextArea] public string backAbilityDescription;
    
    }
}