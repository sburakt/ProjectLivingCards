using System;
using System.Collections;
using TCG.View.Animations.DestroyAnimations;
using TCG.View.Events;
using UnityEngine;

namespace TCG.View.Animations
{
    public class NormalSummonAnimation : MonoBehaviour, IAnimation<NormalSummonEvent>
    {
        [SerializeField] private SimpleSummonAnimation summonAnimation;
        [SerializeField] private PushMoveAnimation pushMoveAnimation;
        [SerializeField] private PushAbyssAnimation pushAbyssAnimation;
        [SerializeField] private DestroyAnimation destroyAnimation;


        public void Play(NormalSummonEvent damageSideEvent, Action onComplete)
        {
            StartCoroutine(PlayNormalSummon(damageSideEvent, onComplete));
        }

        private IEnumerator PlayNormalSummon(NormalSummonEvent summonEvent, Action onComplete)
        {
            int pushCount = 0; 
            if (summonEvent.PushAbyssEvent != null)
            {
                pushCount++;
                pushAbyssAnimation.Play(summonEvent.PushAbyssEvent, () =>  pushCount -- );
                yield return new WaitForSeconds(0.1f);
            }
            if (summonEvent.PushMoveEvent != null)
            {
                pushCount++;
                pushMoveAnimation.Play(summonEvent.PushMoveEvent , ()=> pushCount --);
            }
            summonAnimation.Play(summonEvent.SimpleSummonEvent, onComplete);
            yield return new WaitUntil(() => pushCount == 0);
            if(summonEvent.DestroyEvent != null)
                destroyAnimation.Play(summonEvent.DestroyEvent, () => {});
        }
    }
}