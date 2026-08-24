using System.Collections.Generic;

namespace TCG.Model.Core //idk if view should also use inputs or not
{
    public class InputRequest
    {
        // type might be need to make it easy for view to visualize the legal moves in the ui
        public string DisplayMessage;
        public List<PlayerMove> LegalMoves;
    }

    public class PlayerInput
    {
        public int OptionIndex { get; }
    
        public PlayerInput(int optionIndex)
        {
            OptionIndex = optionIndex;
        }
    }
}