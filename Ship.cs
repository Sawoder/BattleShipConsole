using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipConsole
{
    class Ship
    {
        private int modelSize;
        public Orientation orientation;
        public BoardCell[] part;

        public Ship(int modelSize, Orientation orientation)
        {
            this.modelSize = modelSize;
            this.orientation = orientation;
            this.part = new BoardCell[modelSize];
        }

        public enum Orientation
        {
            Horizontal,
            Vertical
        }
    }
}
