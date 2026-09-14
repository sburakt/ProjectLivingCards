using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace TCG.View
{
    public class CardView : MonoBehaviour
    {
        [FormerlySerializedAs("cardDisplayPrefab")] [SerializeField] private CardView cardViewPrefab;
        [SerializeField] private TextMeshPro cardName;
        [SerializeField] private TextMeshPro cardId;
        [SerializeField] private TextMeshPro buffs;
        [SerializeField] private TextMeshPro attack;
        [SerializeField] private TextMeshPro health;
        [SerializeField] private TextMeshPro baseHealth;
        
        public CardSnapshot CardSnapshot;
        public ICardContainer CurrentContainer;
        
        public void MoveTo(ICardContainer newContainer)
        {
            CurrentContainer?.RemoveCard(this);

            newContainer.AddCard(this);

            CurrentContainer = newContainer;
        }
        

        public void SetData(CardSnapshot data)
        {
            
            CardSnapshot = data;
            cardName.text = data.CardName;
            cardId.text =  data.InstanceId.ToString();
            //buffs.text = data.Buffs.Count.ToString();
            attack.text = data.Attack.ToString();
            health.text = data.CurrentHealth.ToString();
            baseHealth.text = data.BaseHealth.ToString();
        }
    }

    public interface ICardContainer
    {
        public void AddCard(CardView cardView);
        public void RemoveCard(CardView cardView);
    }
}