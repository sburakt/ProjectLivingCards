using System;
using System.Collections;
using DG.Tweening;
using TCG.View.Events;
using UnityEngine;

namespace TCG.View.Animations
{
    public class SimpleSummonAnimation : MonoBehaviour, IAnimation<SimpleSummonEvent>
    {
        [Header("Summon Animation")] 
        [SerializeField] public float summonDuration = 0.5f;
        [SerializeField] public float turnCardDuration = 0.3f;

        
        [Header("Dependencies")]
        [SerializeField] private CardViewRegistry cardViewRegistry;
        [SerializeField] private BoardView boardManager;

        public void Play(SimpleSummonEvent summonEvent, Action onComplete)
        {
            StartCoroutine(PlaySimpleSummon(summonEvent, onComplete));
            
        }

        private IEnumerator PlaySimpleSummon(SimpleSummonEvent summonEvent, Action onComplete)
        {
            FieldDisplay summonedField = boardManager.sideViews[summonEvent.SummoningSideIndex].fieldDisplay;
            CellView summonedCell = summonedField.GetCell(summonEvent.SummonedPosition);
            CardView summonedCard = cardViewRegistry.GetCard(summonEvent.SummonedCardId);
            summonedCell.AddCard(summonedCard);
            yield return DOTween.Sequence()
                .Join(summonedCard.transform.DOLocalMove(Vector3.zero, summonDuration))
                .Join(summonedCard.transform.DOLocalRotateQuaternion(Quaternion.identity, turnCardDuration))
                .WaitForCompletion();
            onComplete?.Invoke();
        }
    }
}