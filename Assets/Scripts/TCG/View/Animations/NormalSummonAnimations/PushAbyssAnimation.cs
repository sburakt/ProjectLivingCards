using System;
using System.Collections;
using DG.Tweening;
using TCG.Model.Enums;
using TCG.View.Events;
using UnityEngine;

namespace TCG.View.Animations
{
    public class PushAbyssAnimation : MonoBehaviour, IAnimation<PushAbyssEvent>
    {
        [Header("Animation")]
        [SerializeField] private float moveToAbyssDuration = 0.3f;
        
        [Header("Dependencies")]
        [SerializeField] CardViewRegistry CardViewRegistry;
        [SerializeField] AbyssAreaView AbyssAreaView;
        
        public void Play(PushAbyssEvent damageSideEvent, Action onComplete)
        {
            StartCoroutine(PlayPushAbyss(damageSideEvent, onComplete));
        }

        private IEnumerator PlayPushAbyss(PushAbyssEvent pushAbyssEvent, Action onComplete)
        {
            CardView pushedCard = CardViewRegistry.GetCard(pushAbyssEvent.CardId);

            if (pushedCard.CurrentViewContainer is not CellView cellView)
                throw new InvalidOperationException($"Card {pushAbyssEvent.CardId} is not in a CellView when pushed to Abyss.");

            int laneIndex = cellView.Position.LaneIndex;
            AbyssCellView abyssCell;
            if (pushAbyssEvent.Direction == VerticalPushDirection.Front)
                abyssCell = AbyssAreaView.abyssCellsMid[laneIndex];
            else
                abyssCell = AbyssAreaView.AbyssCellsSide[pushAbyssEvent.CardSideIndex][laneIndex];
            pushedCard.MoveTo(abyssCell);
            
            yield return pushedCard.transform
                .DOLocalMove(Vector3.zero, moveToAbyssDuration)
                .WaitForCompletion();
            
            onComplete?.Invoke();
        }
    }
}