using System;
using TCG.View.Events;
using UnityEngine;

namespace TCG.View.Animations
{
    public class PushAbyssAnimation : MonoBehaviour, IAnimation<PushAbyssEvent>
    {
        [SerializeField] CardViewRegistry CardViewRegistry;
        [SerializeField] AbyssAreaView AbyssAreaView;
        
        public void Play(PushAbyssEvent pushAbyssEvent, Action onComplete)
        {
            CardView pushedCard = CardViewRegistry.GetCard(pushAbyssEvent.CardId);
            //pushedCard.
        }
    }
}