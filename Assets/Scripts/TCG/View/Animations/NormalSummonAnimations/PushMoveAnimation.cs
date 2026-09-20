using System;
using System.Collections;
using DG.Tweening;
using TCG.View.Events;
using UnityEngine;

namespace TCG.View.Animations
{
    public class PushMoveAnimation: MonoBehaviour, IAnimation<PushMoveEvent>
    {
        [Header("Push Animation")]
        [SerializeField] private float pushDuration = 0.3f;

        [Header("Dependencies")]
        [SerializeField] private CardViewRegistry cardViewRegistry;
        [SerializeField] private BoardView boardManager;

        public void Play(PushMoveEvent damageSideEvent, Action onComplete)
        {
            StartCoroutine(PlayPushMove(damageSideEvent, onComplete));
        }

        private IEnumerator PlayPushMove(PushMoveEvent pushMoveEvent, Action onComplete)
        {
            CardView cardView =
                cardViewRegistry.GetCard(pushMoveEvent.MovedCardId);

            FieldDisplay field =
                boardManager.sideViews[pushMoveEvent.NewPosition.SideIndex].fieldDisplay;

            CellView targetCell =
                field.GetCell(pushMoveEvent.NewPosition);

            targetCell.AddCard(cardView);

            yield return cardView.transform
                .DOLocalMove(Vector3.zero, pushDuration)
                .WaitForCompletion();

            onComplete?.Invoke();
        }
    }
}
