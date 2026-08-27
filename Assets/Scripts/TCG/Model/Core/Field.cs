using System.Collections.Generic;

namespace TCG.Model.Core
{
    public class Field
    {
        public readonly Lane[] Lanes;
        private readonly Cell[] _cells;
        
        public Field(int sideIndex)
        {
            _cells = new Cell[6];
            Lanes = new Lane[3];
            for (int i = 0; i < 3; i++)
            {
                int laneIndex = i;
                Cell frontCell = new Cell(new Position()
                {
                    LaneIndex = i,
                    IsFront = true,
                    SideIndex = sideIndex
                });
                Cell backCell = new Cell(new Position()
                {
                        LaneIndex = i,
                        IsFront = false,
                        SideIndex = sideIndex
                });
                _cells[2*i] = frontCell;
                _cells[2*i+1] = backCell;
                Lanes[i] = new Lane(laneIndex, frontCell, backCell);
            }
        }
        
        public Cell GetCell(Position position)
        {
            Cell cell = position.IsFront ?
                Lanes[position.LaneIndex].FrontCell:
                Lanes[position.LaneIndex].BackCell;
            return cell;
        }

        public IEnumerable<Cell> GetCells()
        {
            foreach (Cell cell in _cells)
            {
                yield return cell;
            }
        }
    }
}