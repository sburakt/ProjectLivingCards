using UnityEngine;
using System;
using System.Collections;
using TCG.Model.Enums;
using TCG.View.Animations.DamageAnimations;
using TCG.View.Animations.DestroyAnimations;
using TCG.View.Events;

namespace TCG.View.Animations.BattleAnimations
{
    public class StandardLaneBattleAnimation: MonoBehaviour, IAnimation<StandardLaneBattleEvent>
    {
        [Header("Battle Animation")] 
        [SerializeField] public float summonDuration = 0.5f;
        [SerializeField] public float turnCardDuration = 0.3f;

        
        [Header("Dependencies")]
        [SerializeField] private BoardView boardView;
        [SerializeField] private AttackCardAnimation attackCardAnimation;
        [SerializeField] private AttackSideAnimation attackSideAnimation;
        [SerializeField] private DestroyAnimation destroyAnimation;
        [SerializeField] private DamageCardAnimation damageCardAnimation;
        [SerializeField] private DamageSideAnimation damageSideAnimation;

        public void Play(StandardLaneBattleEvent damageSideEvent, Action onComplete)
        {
            StartCoroutine(PlayStandardLaneBattleAnimation(damageSideEvent, onComplete));
        }

        private IEnumerator PlayStandardLaneBattleAnimation(StandardLaneBattleEvent battleEvent, Action onComplete)
        {
            bool attackDone = false;
            Action onImpact;
            switch (battleEvent.BattleType)
            {
                case BattleType.AttackSide:
                    if (battleEvent.DamageSideEvent != null)
                        onImpact = () => damageSideAnimation.Play(battleEvent.DamageSideEvent, onComplete: () => {});
                    else
                        onImpact =() => {};
                    attackSideAnimation.Play(
                        battleEvent.AttackSideEvent,
                        onImpact: onImpact,
                        onComplete: () => attackDone = true);
                    break;
                case BattleType.AttackCard:
                    if (battleEvent.DamageCardEvent != null)
                        onImpact = () => damageCardAnimation.Play(battleEvent.DamageCardEvent, onComplete: () => { });
                    else
                        onImpact =() => { };
                    attackCardAnimation.Play(
                        battleEvent.AttackCardEvent,
                        onImpact: onImpact,
                        onComplete: () => attackDone = true);
                    break;
                case BattleType.NoBattle:
                    attackDone = true;
                    break;
            }
            yield return new WaitUntil(()=>attackDone);
            if (battleEvent.DestroyEvent != null)
                destroyAnimation.Play(battleEvent.DestroyEvent, onComplete: () => {});
            onComplete?.Invoke();
        }
    }
}