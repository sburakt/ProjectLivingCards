using System;
using System.Collections;
using DG.Tweening;
using TCG.View.Events;
using UnityEngine;

namespace TCG.View.Animations.BattleAnimations
{
    public class AttackSideAnimation : MonoBehaviour, IAnimation<AttackSideEvent>
    {
        [Header("Animation")]
        [SerializeField] private float attackDuration = 0.2f;
        [SerializeField] private float returnDuration = 0.2f;

        [Header("Dependencies")]
        [SerializeField] private CardViewRegistry cardViewRegistry;
        [SerializeField] private Transform[] sideTargets;

        public void Play(AttackSideEvent damageSideEvent, Action onComplete)
        {
            Play(damageSideEvent, null, onComplete);
        }

        public void Play(AttackSideEvent damageSideEvent, Action onImpact, Action onComplete)
        {
            StartCoroutine(PlayAttack(damageSideEvent, onImpact, onComplete));
        }

        private IEnumerator PlayAttack(AttackSideEvent attackSideEvent, Action onImpact, Action onComplete)
        {
            CardView attacker =
                cardViewRegistry.GetCard(attackSideEvent.AttackerCardId);

            Transform target =
                sideTargets[attackSideEvent.DefenderSideIndex];

            Vector3 originalPosition = attacker.transform.position;

            yield return attacker.transform
                .DOMove(target.position, attackDuration)
                .SetEase(Ease.OutQuad)
                .WaitForCompletion();

            onImpact?.Invoke();

            yield return attacker.transform
                .DOMove(originalPosition, returnDuration)
                .SetEase(Ease.InQuad)
                .WaitForCompletion();

            onComplete?.Invoke();
        }
    }
}