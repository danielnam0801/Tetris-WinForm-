using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris_10404남준성
{
    internal class Diagram
    {
        internal int X { get; set; }
        internal int Y { get; set; }

        internal int Turn
        {
            get;
            private set;
        }

        internal int BlockNum
        {
            get;
            private set;
        }
        
        internal Diagram()
        {
            Reset();
        }

        internal void Reset()
        {
            Random random = new Random();
            X = GameRule.SX; // 4
            Y = GameRule.SY; // 0
            Turn = random.Next() % 4;
            BlockNum = random.Next() % 7;
        }

        internal void MoveLeft()
        {
            X--;
        }
        internal void MoveRight()
        {
            X++;
        }
        internal void MoveDown()
        {
            Y++;
        }
        internal void MoveTurn()
        {
            Turn = (Turn + 1) % 4;
        }


    }
}
