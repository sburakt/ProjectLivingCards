using TCG.Model.Cards;

namespace TCG.Model.Core
{
    public class Lane
    {
        public int Number { get; private set; }
        public RuntimeCard FrontCard { get; set; }
        public RuntimeCard BackCard { get; set; }

        public Lane(int number)
        {
            Number = number;
        }

        public bool IsFull()
        {
            return FrontCard != null && BackCard != null;
        }
    }
}