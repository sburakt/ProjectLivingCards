using System;
using DG.Tweening;
using TCG.View.Events;
using TMPro;
using UnityEngine;

namespace TCG.View.Animations.DamageAnimations
{
    public class DamageCardAnimation : MonoBehaviour, IAnimation<DamageCardEvent>
    {
        [Header("Animations")]
        [SerializeField] private float duration = 0.6f;
        [SerializeField] private float moveDistance = 0.5f;
        
        [Header("Dependencies")]
        [SerializeField] private CardViewRegistry cardViewRegistry;
        [SerializeField] private TMP_Text damageTextPrefab;

        public void Play(DamageCardEvent damageSideEvent, Action onComplete)
        {
            Debug.Log("Playing damage card animation");
            CardView card = cardViewRegistry.GetCard(damageSideEvent.DamagedCardId);

            TMP_Text damageText = Instantiate(damageTextPrefab, card.transform);

            damageText.text = $"-{damageSideEvent.Amount}";

            Transform textTransform = damageText.transform;

            Vector3 startPosition = textTransform.localPosition;
            Vector3 endPosition =
                startPosition + Vector3.up * moveDistance;

            Color startColor = damageText.color;

            textTransform.localScale = Vector3.zero;

            var sequence = DG.Tweening.DOTween.Sequence();

            sequence.Append(
                textTransform
                    .DOScale(Vector3.one, 0.15f)
                    .SetEase(DG.Tweening.Ease.OutBack)
            );

            sequence.Join(
                textTransform
                    .DOLocalMove(endPosition, duration)
                    .SetEase(DG.Tweening.Ease.OutQuad)
            );

            sequence.Join(
                damageText
                    .DOFade(0f, duration)
                    .SetDelay(0.2f)
            );

            sequence.OnComplete(() =>
            {
                //Destroy(damageText.gameObject);
                onComplete?.Invoke();
            });
        }
    }
}