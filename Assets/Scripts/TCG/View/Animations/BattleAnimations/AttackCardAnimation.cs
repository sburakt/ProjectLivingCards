using System;
using System.Collections;
using DG.Tweening;
using TCG.View.Events;
using UnityEngine;

namespace TCG.View.Animations.BattleAnimations
{
    public class AttackCardAnimation : MonoBehaviour, IAnimation<AttackCardEvent>
    {
        [Header("Animation")]
        [SerializeField] private float attackDuration = 0.2f;
        [SerializeField] private float returnDuration = 0.2f;
        [Header("Dependencies")]
        [SerializeField] private CardViewRegistry cardViewRegistry;

        public void Play(AttackCardEvent damageSideEvent, Action onComplete)
        {
            Play(damageSideEvent, null, onComplete);
        }
        public void Play(AttackCardEvent damageSideEvent, Action onImpact, Action onComplete)
        {
            StartCoroutine(PlayAttack(damageSideEvent, onImpact, onComplete));
        }
        private IEnumerator PlayAttack( AttackCardEvent attackCardEvent,Action onImpact ,Action onComplete)
        {
            CardView attacker = cardViewRegistry.GetCard(attackCardEvent.AttackerCardId);

            CardView target = cardViewRegistry.GetCard(attackCardEvent.DefenderCardId);

            Vector3 originalPosition = attacker.transform.position;

            yield return attacker.transform
                .DOMove(target.transform.position, attackDuration)
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