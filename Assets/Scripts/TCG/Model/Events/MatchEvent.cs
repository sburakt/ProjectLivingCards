using TCG.Model.Cards;
using TCG.Model.Core;
using TCG.Model.Enums;

namespace TCG.Model.Events
{
    public abstract class MatchEvent
    {
        public int GroupId { get; set; }
        public int IntroducedCardId = 0;
    }
    
    public class AttackedCardEvent :MatchEvent
    {
        public int AttackerId { get; private set; }
        public int DefenderId { get; private set; }

        public AttackedCardEvent(int attackerId, int defenderId, int groupId)
        {
            AttackerId = attackerId;
            DefenderId = defenderId;
            GroupId = groupId;
        }   
    }

    public class AttackedSideEvent :MatchEvent
    {
        public int AttackerId { get; private set; }
        public int DefenderSideIndex { get; private set; }

        public AttackedSideEvent(int attackerId, int defenderSideIndex, int groupId)
        {
            AttackerId = attackerId;
            DefenderSideIndex = defenderSideIndex;
            GroupId = groupId;
        }
    }

    public class DamagedSideEvent : MatchEvent
    {
        public int AttackerId { get; private set; }
        public int DamagedSideIndex { get; private set; }
        public int DamageAmount { get; private set; }
        public DamageType DamageType { get; private set; }
        
        public DamagedSideEvent(int attackerId, int damagedSideIndex, int damageAmount, DamageType damageType, int groupId)
        {
            AttackerId = attackerId;
            DamagedSideIndex = damagedSideIndex;
            DamageAmount = damageAmount;
            DamageType = damageType;
            GroupId = groupId;
        }
    }

    public class DamagedCardEvent :MatchEvent
    {
        public int AttackerId { get; private set; }
        public int DefenderId { get; private set; }
        public int DamageAmount { get; private set; }
        
        public DamageType DamageType { get; private set; }

        public DamagedCardEvent(int attackerId, int defenderId, int damageAmount, DamageType damageType, int groupId)
        {
            AttackerId = attackerId;
            DefenderId = defenderId;
            DamageAmount = damageAmount;
            DamageType = damageType;
            GroupId = groupId;
        }
    }

    public class DestroyedEvent : MatchEvent
    {
        public int DestroyerId { get; private set; }
        public int DestroyedId { get; private set; }
        
        public DestroyCause DestroyCause { get; private set; }

        public DestroyedEvent(int destroyerId, int destroyedId, DestroyCause destroyCause, int groupId)
        {
            DestroyerId = destroyerId;
            DestroyedId = destroyedId;
            DestroyCause = destroyCause;
            GroupId = groupId;
        }
    }

    public class EnteredFieldEvent :MatchEvent
    {
        public int CardId { get; private set; }
        public int SideIndex { get; private set; }

        public EnteredFieldEvent(int cardId, int sideIndex, int groupId)
        {
            CardId = cardId;
            SideIndex = sideIndex;
            GroupId = groupId;
            IntroducedCardId = cardId;
        }
    }

    public class ExitFieldEvent :MatchEvent
    {
        public int CardId { get; private set; }
        public int SideIndex { get; private set; }

        public ExitFieldEvent(int cardId, int sideIndex, int groupId)
        {
            CardId = cardId;
            SideIndex = sideIndex;
            GroupId = groupId;
            IntroducedCardId = cardId;
        }
    }

    public class DrawnEvent : MatchEvent
    {
        public int DrawnCardId { get; private set; }
        public int DrawingSideIndex { get; private set; }
        

        public DrawnEvent(int drawnCardId, int drawingSideIndex, int groupId)
        {
            DrawnCardId = drawnCardId;
            DrawingSideIndex = drawingSideIndex;
            GroupId = groupId;
            IntroducedCardId = drawnCardId;
        }
    }

    public class MovedEvent :MatchEvent
    {
        public int MovedCardId { get; private set; }
        public Position OldPosition { get; private set; }
        public Position NewPosition{ get; private set; }

        public MovedEvent( int movedCardId, Position oldPosition, Position newPosition, int groupId)
        {
            MovedCardId = movedCardId;
            OldPosition = oldPosition;
            NewPosition = newPosition;
            GroupId = groupId;
        }
        
    }
    public class NormalSummonedEvent : MatchEvent
    {
        public int SummoningSideIndex { get; private set; }
        public Position SummonTargetPosition { get; private set; }
        public int SummonedCardId { get; private set; }

        public NormalSummonedEvent(int summonedCardId, int summoningSideIndex ,Position summonTargetPosition, int groupId)
        {
            SummonedCardId = summonedCardId;
            SummonTargetPosition = summonTargetPosition;
            GroupId = groupId;
            IntroducedCardId = summonedCardId;
        }
    }
    
    public class PushedEvent: MatchEvent
    {
        public int PushedCardId { get; private set; }
        public int PusherCardId { get; private set; }
        public PushedEvent(int pushedCardId, int pusherCardId, int groupId)
        {
            PushedCardId = pushedCardId;
            PusherCardId = pusherCardId;
            GroupId = groupId;
        }
        
    }

    public class SimpleSummonedEvent : MatchEvent
    {
        public int SummonedCardId { get; private set; }
        public Position SummonedPosition { get; private set; }

        public SimpleSummonedEvent(int summonedCardId, Position summonedPosition, int groupId)
        {
            SummonedCardId = summonedCardId;
            SummonedPosition = summonedPosition;
            GroupId = groupId;
            IntroducedCardId = summonedCardId;
        }
    }

    public class StandardLaneBattleEvent : MatchEvent
    {
        public int LaneIndex { get; private set; }
        public int AttackerSineIndex { get; private set; }

        public StandardLaneBattleEvent(int laneIndex, int attackerSineIndex, int groupId)
        {
            LaneIndex = laneIndex;
            AttackerSineIndex = attackerSineIndex;
            GroupId = groupId;
        }
    }

}