namespace TCG.Model.Core
{
    public struct Position
    {
        public int  SideIndex;
        public int LaneIndex;
        public bool IsFront;
        public bool IsBack
        {
            get => !IsFront;
            set => IsFront = !value;
        }

        public override string ToString()
        {
            return "[" + SideIndex + "," + LaneIndex + IsFront + "]";
        }
        // adding is back confusing but i can miss !IsFront since I and ! blend together sometimes
    }
}