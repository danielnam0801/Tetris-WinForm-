using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris_10404남준성
{
    internal class Game
    {
        Diagram now;
        Board gboard = Board.GameBoard;
        public static int score = 0;
        public static int level = 1;

        internal int this[int x, int y]
        {
            get
            {
                return gboard[x, y];
            }
        }
        internal Point NowPosition
        {
            get
            {
                if (now == null) return new Point(0, 0);
                return new Point(now.X, now.Y);
            }
        }

        internal int BlockNum
        {
            get
            {
                return now.BlockNum;
            }
        }

        internal int Turn
        {
            get
            {
                return now.Turn; // 현재 turn 이 어떻게 되어있는지
            }
        }

        internal static Game Singleton
        {
            get; private set;
        }

        static Game()
        {
            Singleton = new Game();
        }

        Game()
        {
            now = new Diagram();
        }

        internal bool MoveLeft()
        {
            for (int xx = 0; xx < 4; xx++)
            {
                for (int yy = 0; yy < 4; yy++)
                {
                    if (BlockValue.bValues[now.BlockNum, Turn, xx, yy] != 0)
                    {
                        if (now.X + xx <= 0)
                        {
                            return false;
                        }
                    }
                }
            }
            if (gboard.MoveEnable(now.BlockNum, Turn, now.X - 1, now.Y))
            {
                now.MoveLeft();
                return true;
            }

            return false;
        }

        internal bool MoveRight()
        {
            for (int xx = 0; xx < 4; xx++)
            {
                for (int yy = 0; yy < 4; yy++)
                {
                    if (BlockValue.bValues[now.BlockNum, Turn, xx, yy] != 0)
                    {
                        if (now.X + xx + 1 >= GameRule.BX)
                        {
                            return false;
                        }
                    }
                }
            }
            if (gboard.MoveEnable(now.BlockNum, Turn, now.X + 1, now.Y))
            {
                now.MoveRight();
                return true;
            }

            return false;
        }

        internal bool MoveDown()
        {
            for (int xx = 0; xx < 4; xx++)
            {
                for (int yy = 0; yy < 4; yy++)
                {
                    if (BlockValue.bValues[now.BlockNum, Turn, xx, yy] != 0)
                    {
                        if (now.Y + yy + 1 >= GameRule.BY)
                        {
                            gboard.Store(now.BlockNum, Turn, now.X, now.Y);
                            return false;
                        }
                    }
                }
            }

            if (gboard.MoveEnable(now.BlockNum, Turn, now.X, now.Y + 1))
            {
                now.MoveDown();
                return true;
            }
            gboard.Store(now.BlockNum, Turn, now.X, now.Y);
            return false;
        }
        internal bool MoveTurn()
        {
            for (int xx = 0; xx < 4; xx++)
            {
                for (int yy = 0; yy < 4; yy++)
                {
                    if (BlockValue.bValues[now.BlockNum, (Turn + 1) % 4, xx, yy] != 0)
                    {
                        if ((now.X + xx < 0) || (now.X + xx + 1 >= GameRule.BX) || (now.Y + yy + 1 >= GameRule.BY))
                        {
                            return false;
                        }
                    }
                }
            }
            if (gboard.MoveEnable(now.BlockNum, (Turn + 1) % 4, now.X, now.Y))
            {
                now.MoveTurn();
                return true;
            }

            return false;
        }
        internal bool Next()
        {
            now.Reset();
            return gboard.MoveEnable(now.BlockNum, Turn, now.X, now.Y);
        }

        internal void Restart()
        {
            gboard.ClearBoard();
        }
    }
}

