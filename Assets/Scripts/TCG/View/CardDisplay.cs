using TMPro;
using UnityEditor;
using UnityEngine;

namespace TCG.View
{
    public class CardDisplay : MonoBehaviour
    {
        [SerializeField] private CardDisplay cardDisplayPrefab;
        [SerializeField] private TextMeshPro cardName;
        [SerializeField] private TextMeshPro cardId;
        [SerializeField] private TextMeshPro buffs;
        [SerializeField] private TextMeshPro attack;
        [SerializeField] private TextMeshPro health;
        [SerializeField] private TextMeshPro baseHealth;
        
        public CardDisplayData CardDisplayData;

        public void Display(CardDisplayData data)
        {
            CardDisplayData = data;
            cardName.text = data.CardName;
            cardId.text =  data.InstanceId.ToString();
            buffs.text = data.Buffs.Count.ToString();
            attack.text = data.Attack.ToString();
            health.text = data.CurrentHealth.ToString();
            baseHealth.text = data.BaseHealth.ToString();
        }


    }
}