using TCG.Model.Core;

namespace TCG.Model.Actions
{
    public class EnterAbyssAction: MatchAction
    {
        private readonly int _cardId;
        private readonly int _abyssIndex;
        private readonly int _laneIndex;
        public override void Execute(Match match)
        {
            //todo
            AbyssCell abyssCell = match.Abyss.AbyssCell[_abyssIndex][_laneIndex];
        }

        public EnterAbyssAction(int cardId ,int abyssIndex, int laneIndex)
        {
            _cardId = cardId;
            _abyssIndex = abyssIndex;
            _laneIndex = laneIndex;
        }
    }
}