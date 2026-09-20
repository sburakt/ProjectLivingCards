using System;
using TCG.Model.Enums;
using TCG.View.Events;
using UnityEngine;

namespace TCG.View.Animations.DestroyAnimations
{
    public class DestroyAnimation: MonoBehaviour ,IAnimation<DestroyEvent>
    {
        [SerializeField] private FallIntoAbyssAnimation fallIntoAbyssAnimation;
        [SerializeField] private StandardDestroyAnimation standardDestroyAnimation;
        [SerializeField] private CardViewRegistry cardViewRegistry;
        [SerializeField] private BoardView boardView;

        public void Play(DestroyEvent damageSideEvent, Action onComplete)
        {
            switch (damageSideEvent.DestroyCause)
            {
                case DestroyCause.Push:
                    PlayFallAnimation(damageSideEvent, onComplete);
                    break;
                case DestroyCause.AttackDamage:
                    PlayStandardDestroyAnimation(damageSideEvent, onComplete);
                    break;
            }
        }

        private void PlayFallAnimation(DestroyEvent destroyEvent, Action onComplete)
        {
            fallIntoAbyssAnimation.Play(destroyEvent, onComplete);
        }

        private void PlayStandardDestroyAnimation(DestroyEvent destroyEvent, Action onComplete)
        {
            standardDestroyAnimation.Play(destroyEvent, onComplete);
        }
    }
}