using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace TCG.View
{
    public class HandView : MonoBehaviour, ICardContainer
    {
        [Header("Hand Settings")] [SerializeField]
        private float cardSpacing = 1.2f;

        [SerializeField] private float maxHandWidth = 8f;
        [SerializeField] private float heightArc = 0.2f;
        [SerializeField] private float rotationAngle = 12f;
        [SerializeField] private float fanDuration = 0.35f;

        private List<CardView> _cardViews = new List<CardView>();
        private Dictionary<CardView, PosRot> _cardPosRots = new Dictionary<CardView, PosRot>();

        public void AddCard(CardView cardView)
        {
            _cardViews.Add(cardView);
            cardView.transform.SetParent(transform, true);
            CalculatePositions();
        }

        public void RemoveCard(CardView cardView)
        {
            _cardViews.Remove(cardView);
            _cardPosRots.Remove(cardView);
            CalculatePositions();
        }

        public PosRot GetPosRot(CardView cardView)
        {
            return _cardPosRots[cardView];
        }


        private void CalculatePositions()
        {
            int cardCount = _cardViews.Count;

            if (cardCount == 0)
                return;

            float currentWidth = Mathf.Min(maxHandWidth, (cardCount - 1) * cardSpacing);

            (Vector3, Quaternion) targetPosRot = (transform.position, Quaternion.identity);

            for (int i = 0; i < cardCount; i++)
            {
                CardView cardView = _cardViews[i];

                // -1 to 1 is nomalized pos
                float normalizedPos = 0f;
                if (cardCount > 1)
                {
                    normalizedPos = ((float)i / (cardCount - 1)) * 2f - 1f;
                }

                float targetX = normalizedPos * (currentWidth / 2f);

                // todo use sorting instead after finalized arc and stuff
                float targetY = -i * 0.01f;

                float targetZ = -(normalizedPos * normalizedPos) * heightArc;

                Vector3 targetLocalPos = new Vector3(targetX, targetY, targetZ);

                Vector3 targetLocalRot = new Vector3(0, normalizedPos * rotationAngle, 0);

                _cardPosRots[cardView] = new PosRot(targetLocalPos, Quaternion.Euler(targetLocalRot));
            }
        }

        public void FanOutExcept(CardView targetCardView)
        {
            int cardCount = _cardViews.Count;

            if (cardCount == 0)
                return;

            for (int i = 0; i < cardCount; i++)
            {
                CardView cardView = _cardViews[i];
                if (cardView == targetCardView)
                    continue;

                Vector3 targetLocalPos = _cardPosRots[cardView].Position;
                Quaternion targetLocalRot = _cardPosRots[cardView].Rotation;

                cardView.transform.DOKill();
                cardView.transform.DOLocalMove(targetLocalPos, fanDuration).SetEase(Ease.OutBack);
                cardView.transform.DOLocalRotateQuaternion(targetLocalRot, fanDuration).SetEase(Ease.OutCubic);

            }
        }

        public void FanOut()
        {
            int cardCount = _cardViews.Count;

            if (cardCount == 0)
                return;

            for (int i = 0; i < cardCount; i++)
            {
                CardView cardView = _cardViews[i];

                Vector3 targetLocalPos = _cardPosRots[cardView].Position;
                Quaternion targetLocalRot = _cardPosRots[cardView].Rotation;

                cardView.transform.DOKill();
                cardView.transform.DOLocalMove(targetLocalPos, fanDuration).SetEase(Ease.OutBack);
                cardView.transform.DOLocalRotateQuaternion(targetLocalRot, fanDuration).SetEase(Ease.OutCubic);

            }
        }
    }
}