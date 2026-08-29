using TCG.Model.Core;

namespace TCG.Model.Actions
{
    public abstract class MatchAction
    {
        public abstract void Execute(Match match);
    }
}

    /*
    public struct ActionData
    {
        public ActionType ActionType;
        public int Int1;
        public int Int2;
        public int Int3;
        public int Int4;
        public int Int5;
        public Position Pos1;
        public Position Pos2;
        public Position Pos3;
        public Position Pos4;
    }

    enum ActionType
    {
        //all actions here
    }
    */


    // enemy AI will probably use MCTS and MinMax and some preset combos i need to search what tcg ai can use
    // this means Match will be cloned and each branch will create action to run once
    // actions will create lots of short life class in short time
    // gc can kill performance
    // 
