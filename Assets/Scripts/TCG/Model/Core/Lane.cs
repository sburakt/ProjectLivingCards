using TCG.Model.Cards;

namespace TCG.Model.Core
{
    public class Lane
    {
        public int Index { get; private set; }

        public readonly Cell FrontCell;
        public readonly Cell BackCell;

        // delete FrontCard and BackCard once usage is 0
        public RuntimeCard FrontCard
        {
            get => FrontCell.Card;
            set => FrontCell.SetCard(value); 
        }
        public RuntimeCard BackCard
        {
            get => BackCell.Card;
            set => BackCell.SetCard(value); 
        }
        public bool IsFull => FrontCell.IsFull && BackCell.IsFull;

        public Lane(int index, Cell frontCell, Cell backCell)
        {
            Index = index;
            FrontCell = frontCell;
            BackCell = backCell;
        }
    }
}