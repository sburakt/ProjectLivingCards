using System;
using System.Collections;
using TCG.View.Events;
using UnityEngine;

namespace TCG.View.Animations
{
    public class NormalSummonAnimation : MonoBehaviour, IAnimation<NormalSummonEvent>
    {
        [SerializeField] private SimpleSummonAnimation summonAnimation;
        [SerializeField] private PushMoveAnimation pushMoveAnimation;
        //[SerializeField] private  pushMoveAnomation;
        //...


        public void Play(NormalSummonEvent summonEvent, Action onComplete)
        {
            StartCoroutine(PlayNormalSummon(summonEvent, onComplete));
            
        }

        private IEnumerator PlayNormalSummon(NormalSummonEvent summonEvent, Action onComplete)
        {
            bool currentAnimationComplete = false;
            foreach (PushMoveEvent pushMoveEvent in summonEvent.PushedEvents)
            {
                pushMoveAnimation.Play(pushMoveEvent, ()=> currentAnimationComplete = true);
                yield return new WaitUntil(() => currentAnimationComplete);
                currentAnimationComplete = false;
            }
            summonAnimation.Play(summonEvent.SimpleSummonEvent, ()=> currentAnimationComplete = true);
            yield return new WaitUntil(() => currentAnimationComplete);
            currentAnimationComplete = false;
            onComplete?.Invoke();
        }
    }
}