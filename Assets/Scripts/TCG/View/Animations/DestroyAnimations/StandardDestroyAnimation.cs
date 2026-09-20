using System;
using TCG.View.Events;
using UnityEngine;

namespace TCG.View.Animations.DestroyAnimations
{
    public class StandardDestroyAnimation: MonoBehaviour, IAnimation<DestroyEvent>
    {
        [SerializeField] private CardViewRegistry cardViewRegistry;
        public void Play(DestroyEvent destroyEvent, Action onComplete)
        {
            CardView card = cardViewRegistry.GetCard(destroyEvent.DestroyedCardId);

            card.gameObject.SetActive(false);

            onComplete?.Invoke();
        }
    }
}