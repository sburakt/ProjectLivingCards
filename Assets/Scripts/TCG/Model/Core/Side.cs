using System.Collections.Generic;
using TCG.Model.Cards;

namespace TCG.Model.Core
{
    public class Side
    {
        public readonly int SideIndex;
        public int LifePoints { get; set; }
        public Field Field { get; private set; }
        public List<RuntimeCard> Deck; // { get; private set; }
        public List<RuntimeCard> Hand { get; private set; }
        public List<RuntimeCard> Graveyard { get; private set; }

        public Side(int sideIndex)
        {
            SideIndex = sideIndex;
            LifePoints = 40;
            Field = new Field();
            Deck = new List<RuntimeCard>();
            Hand = new List<RuntimeCard>();
            Graveyard = new List<RuntimeCard>();
        }

        public IEnumerable<RuntimeCard> GetCardSlotsInField()
        {
            for (int i = 0; i < 2; i++)
            {
                foreach (Lane lane in Field.Lanes)
                {
                    yield return i==0? lane.FrontCard: lane.BackCard;
                }
            }
        }

        public IEnumerable<RuntimeCard> GetCardsInField()
        {
            foreach (Lane lane in Field.Lanes)
            {
                if (lane.FrontCard != null)
                    yield return lane.FrontCard;
                
                if (lane.BackCard != null)
                    yield return lane.BackCard;
            }
        }
        
        public IEnumerable<Position> GetPositions()
        {
            for (int i = 0; i < Field.Lanes.Length; i++)
            {
                if (!Field.Lanes[i].IsFull())
                {
                    yield return new Position { SideIndex = SideIndex, LaneIndex = i, IsFront = true };
                    yield return new Position { SideIndex = SideIndex, LaneIndex = i, IsFront = false };
                }
            }
        }
        
        
        public RuntimeCard FindRuntimeCardById(int instanceId)
        {
            // 1 Search the Field
            for (int i = 0; i < Field.Lanes.Length; i++)
            {
                Lane lane = Field.Lanes[i];
                if (lane.FrontCard != null && lane.FrontCard.InstanceId == instanceId) return lane.FrontCard;
                if (lane.BackCard != null && lane.BackCard.InstanceId == instanceId) return lane.BackCard;
            }

            // 2. Search the Hand
            for (int i = 0; i < Hand.Count; i++)
            {
                if (Hand[i].InstanceId == instanceId) return Hand[i];
            }
        
            // 3. Search the Graveyard
            for (int i = 0; i < Graveyard.Count; i++)
            {
                if (Graveyard[i].InstanceId == instanceId) return Graveyard[i];
            }
        
            // 4. Search the Deck
            for (int i = 0; i < Deck.Count; i++)
            {
                if (Deck[i].InstanceId == instanceId) return Deck[i];
            }

            return null;
        }
    }
}
