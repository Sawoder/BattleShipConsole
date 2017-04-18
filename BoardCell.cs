using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipConsole
{
    class BoardCell
    {
        private int x;
        private int y;
        public StateCell stateCell;

        public BoardCell(int x, int y)
        {
            this.x = x;
            this.y = y;
            stateCell = StateCell.Empty;
        }

        public int X { get { return x; } }
        public int Y { get { return y; } }

        public enum StateCell
        {
            Empty,
            Ship,
            Closed,
            Miss,
            Hit
        }
    }
}
