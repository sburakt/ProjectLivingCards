using System;
using System.Collections;
using DG.Tweening;
using TCG.View.Events;
using UnityEngine;
using UnityEngine.Serialization;

namespace TCG.View.Animations
{
    public class DrawAnimation : MonoBehaviour, IAnimation<DrawEvent>
    {
        [FormerlySerializedAs("DrawDuration")]
        [Header("Draw Animation")] 
        [SerializeField] public float drawDuration = 0.5f;
        [FormerlySerializedAs("TurnCardDuration")] [SerializeField] public float turnCardDuration = 0.2f;
        
        [FormerlySerializedAs("cardManager")]
        [Header("Dependencies")]
        [SerializeField] private CardViewRegistry cardViewRegistry;
        [SerializeField] private BoardView boardManager;
        
        public void Play(DrawEvent drawEvent, Action onComplete)
        {
            StartCoroutine(PlayDraw(drawEvent, onComplete));
        }

        private IEnumerator PlayDraw(DrawEvent drawEvent, Action onComplete)
        {
            SideView side = boardManager.sideViews[drawEvent.DrawingSideIndex];
            DeckView deckView = side.deckView;
            for (int i = 0; i < drawEvent.CardsToDraw.Count; i++)
            {
                CardView cardView = cardViewRegistry.GetOrCreateCard(drawEvent.CardsToDraw[i], drawEvent.CardSnapshots[i]);
                deckView.AddCardToTop(cardView);
                yield return StartCoroutine(DrawToHand(cardView, side.handView));
            }
            onComplete?.Invoke();
        }

        private IEnumerator DrawToHand(CardView cardView, HandView handView)
        {
            handView.AddCard(cardView);
            PosRot posRot = handView.GetPosRot(cardView);
            handView.FanOut();
            yield return DOTween.Sequence()
                .Join(cardView.transform.DOLocalMove(posRot.Position, drawDuration))
                .Join(cardView.transform.DOLocalRotateQuaternion(posRot.Rotation, turnCardDuration))
                .WaitForCompletion();
        }
    }
}