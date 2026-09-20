using System;
using System.Collections;
using DG.Tweening;
using TCG.View.Events;
using UnityEngine;

namespace TCG.View.Animations
{
    public class FallIntoAbyssAnimation : MonoBehaviour, IAnimation<DestroyEvent>
    {
        [Header("Animation")]
        [SerializeField] private float fallDuration = 0.5f;
        [SerializeField] private float fallDistance = 5f;

        [Header("Dependencies")]
        [SerializeField] private CardViewRegistry cardViewRegistry;

        public void Play(DestroyEvent damageSideEvent, Action onComplete)
        {
            StartCoroutine(PlayFall(damageSideEvent, onComplete));
        }

        private IEnumerator PlayFall(
            DestroyEvent destroyEvent,
            Action onComplete)
        {
            CardView card =
                cardViewRegistry.GetCard(destroyEvent.DestroyedCardId);

            if (card.CurrentViewContainer is not AbyssCellView)
            {
                throw new InvalidOperationException(
                    $"Card {destroyEvent.DestroyedCardId} is not in an AbyssCellView when falling.");
            }

            yield return card.transform
                .DOLocalMove(
                    Vector3.down * fallDistance,
                    fallDuration)
                .WaitForCompletion();

            onComplete?.Invoke();
        }
    }
}