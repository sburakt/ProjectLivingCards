using System;
using TMPro;
using UnityEngine;
using DG.Tweening;
using TCG.View.Events;

namespace TCG.View.Animations.BattleAnimations
{
    public class DamageSideAnimation : MonoBehaviour, IAnimation<DamageSideEvent>
    {
        [SerializeField] private Transform[] sideTransforms;
        [SerializeField] private TMP_Text damageTextPrefab;

        [SerializeField] private float duration = 0.6f;
        [SerializeField] private float moveDistance = 0.5f;

        public void Play(DamageSideEvent damageSideEvent, Action onComplete)
        {
            Transform sideTransform = sideTransforms[damageSideEvent.DamageSideIndex];

            TMP_Text damageText = Instantiate(damageTextPrefab, sideTransform);

            damageText.text = $"-{damageSideEvent.Amount}";

            Transform textTransform = damageText.transform;

            Vector3 startPosition = textTransform.localPosition;
            Vector3 endPosition =
                startPosition + Vector3.up * moveDistance;

            textTransform.localScale = Vector3.zero;

            Sequence sequence = DOTween.Sequence();

            sequence.Append(
                textTransform
                    .DOScale(Vector3.one, 0.15f)
                    .SetEase(Ease.OutBack)
            );

            sequence.Join(
                textTransform
                    .DOLocalMove(endPosition, duration)
                    .SetEase(Ease.OutQuad)
            );

            sequence.Join(
                damageText
                    .DOFade(0f, duration)
                    .SetDelay(0.2f)
            );

            sequence.OnComplete(() =>
            {
                Destroy(damageText.gameObject);
                onComplete?.Invoke();
            });
        }
    }
}