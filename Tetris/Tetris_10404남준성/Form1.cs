using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris_10404남준성
{
    public partial class Form1 : Form
    {
        Game game;

        int bx;
        int by;
        int bwidth;
        int bheight;

        Image[] block = new Image[7];
        public Form1()
        {
            InitializeComponent();
            Bitmap croppedBitmap = new Bitmap(@"C:\Users\user\Downloads\Untitled-3.png");

            for (int i = 0; i < 7; i++)
            {
                Rectangle rect = new Rectangle(30 * i, 0, 30, 30);
                Bitmap cropBitmap = cropAtRect(croppedBitmap, rect);

                block[i] = cropBitmap;
            }
        }

        public Bitmap cropAtRect(Bitmap orgImg, Rectangle sRect)
        {
            Rectangle destRect = new Rectangle(Point.Empty, sRect.Size);

            var cropImage = new Bitmap(destRect.Width, destRect.Height);
            using (var graphics = Graphics.FromImage(cropImage))
            {
                graphics.DrawImage(orgImg, destRect, sRect, GraphicsUnit.Pixel);
            }
            return cropImage;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            game = Game.Singleton;
            bx = GameRule.BX; //12
            by = GameRule.BY; //20
            bwidth = GameRule.B_Width;  //30
            bheight = GameRule.B_Height;//30
            SetClientSizeCore(bwidth * bx, bheight * by);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawGraduation(e.Graphics);
            DrawDiagram(e.Graphics);
            DrawDiaImage(e.Graphics);
            DrowBoard(e.Graphics);
            DoubleBuffered = true;
            
        }

        private void DrowBoard(Graphics graphics)
        {
            for(int xx = 0; xx < bx; xx++)
            {
                for(int yy = 0; yy < by; yy++)
                {
                    if(game[xx, yy] != 0)
                    {
                        Rectangle now_rt = new Rectangle(xx * bwidth + 2 , yy * bheight +2 , bwidth - 4, bheight - 4);
                        graphics.DrawRectangle(Pens.DarkBlue, now_rt);
                        graphics.FillRectangle(Brushes.GreenYellow, now_rt);
                    }
                }
            }
        }

        private void DrawDiagram(Graphics graphics)
        {
            Pen dpen = new Pen(Color.Red, 4);
            Point now = game.NowPosition;
            int bn = game.BlockNum;
            int tn = game.Turn;

            for (int xx = 0; xx < 4; xx++)
            {
                for (int yy = 0; yy < 4; yy++)
                {
                    if (BlockValue.bValues[bn, tn, xx, yy] != 0)
                    {
                        Rectangle rect = new Rectangle((now.X + xx) * bwidth + 2, (now.Y + yy) * bheight + 2, bwidth - 4, bheight - 4);
                        graphics.DrawRectangle(dpen,rect);
                    }
                }
            }

        }

        private void DrawDiaImage(Graphics graphics)
        {
            Point now = game.NowPosition;
            int bn = game.BlockNum;
            int tn = game.Turn;

            for (int xx = 0; xx < 4; xx++)
            {
                for (int yy = 0; yy < 4; yy++)
                {
                    if (BlockValue.bValues[bn, tn, xx, yy] != 0)
                    {
                        graphics.DrawImage(block[bn], (now.X + xx) * bwidth, (now.Y + yy) * bheight, bwidth+2, bheight+2);
                    }
                }
            }

        }

        //private Image ResizeImage(Image image, int bn)
        //{
        //    //Bitmap newImage = new Bitmap(width, height, PixelFormat.Format24bppRgb);
        //    //Graphics temp = Graphics.FromImage(newImage);

        //    //// 자르기
        //    //temp.DrawImage(img,
        //    //        new Rectangle(0, 0, width, height),
        //    //        new Rectangle(x, y, width, height),
        //    //        GraphicsUnit.Pixel);

        //    //temp.Dispose();
        //    //return newImage;
        //    //[출처] (C#) 이미지 처리:  자르기 (3)|작성자 코딩ABC

            
        //    //Bitmap croppedBitmap = new Bitmap(30,30,System.Drawing.Imaging.PixelFormat.Format24bppRgb);
        //    //Graphics temp = Graphics.FromImage(croppedBitmap);
        //    //temp.DrawImage(image, new Rectangle()
        //    //croppedBitmap = croppedBitmap.Clone(new Rectangle(bn * bwidth, 0, 30, 30), System.Drawing.Imaging.PixelFormat.DontCare);

        //    return croppedBitmap;
        //}

        private void DrawGraduation(Graphics graphics)
        {
            DrawHorizons(graphics); //수평
            DrawDiagramVerticals(graphics); //수직
        }

        private void DrawDiagramVerticals(Graphics graphics)
        {
            Point st = new Point(); //시작점
            Point et = new Point(); //끝점

            for (int cx = 0; cx < bx; cx++)
            {
                st.X = cx * bwidth;
                st.Y = 0;
                et.X = st.X;
                et.Y = by * bheight;
                graphics.DrawLine(Pens.Black, st, et);
            }
        }

        private void DrawHorizons(Graphics graphics)
        {
            Point st = new Point();
            Point et = new Point();

            for (int cy = 0; cy < by; cy++)
            {
                st.X = 0;
                st.Y = cy * bheight;
                et.X = bx * bwidth;
                et.Y = st.Y;
                graphics.DrawLine(Pens.Black, st, et);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Right: MoveRight(); return;
                case Keys.Left: MoveLeft(); return;
                case Keys.Space: moveDown(); return;
                case Keys.Up: MoveTurn(); return;
                case Keys.Down: MoveSSDown(); return;   
            }
        }

        private void MoveSSDown()
        {
            while(game.MoveDown())
            {
                Region rg = MakeRegion(0, -1); //rg 에는 이전 영역과 현재 영역을 합친 영역이 저장
                Invalidate(rg);
            }
            EndingCheck();
        }

        private void MoveTurn()
        {
            if (game.MoveTurn())
            {
                Region rg = MakeRegion();
                Invalidate(rg);
            }
        }

        private void moveDown()
        {
            if (game.MoveDown())
            {
                Region rg = MakeRegion(0, -1); //rg 에는 이전 영역과 현재 영역을 합친 영역이 저장
                Invalidate(rg);
            }
            else
            {
                EndingCheck();
            }
        }

        private void EndingCheck()
        {
            if (game.Next())
            {
                Invalidate();
            }
            else
            {
                timer_down.Enabled = false;
                DialogResult re = MessageBox.Show("다시시작", "다시시작 여부", MessageBoxButtons.YesNo);

                if (re == DialogResult.Yes)
                {
                    game.Restart();
                    timer_down.Enabled=true;
                    Invalidate();
                }
                else
                {
                    Close(); 
                }
            }
        }

        private Region MakeRegion(int cx, int cy)
        {
            Point now = game.NowPosition;
            int bn = game.BlockNum;
            int tn = game.Turn;

            Region region = new Region();

            for (int xx = 0; xx < 4; xx++)
            {
                for (int yy = 0; yy < 4; yy++)
                {
                    if (BlockValue.bValues[bn, tn, xx, yy] != 0)
                    {
                        Rectangle rect1 = new Rectangle((now.X + xx) * bwidth, (now.Y + yy) * bheight, bwidth, bheight); //현재 사각형
                        Rectangle rect2 = new Rectangle((now.X + xx + cx) * bwidth, (now.Y + yy + cy) * bheight, bwidth, bheight);

                        Region rg1 = new Region(rect1);
                        Region rg2 = new Region(rect2);

                        region.Union(rg1);
                        region.Union(rg2);
                    }
                }
            }
            return region;
        }

        private Region MakeRegion()
        {
            Point now = game.NowPosition;
            int bn = game.BlockNum;
            int tn = game.Turn;
            int oldtn = (tn + 3) % 4;

            Region region = new Region();

            for (int xx = 0; xx < 4; xx++)
            {
                for (int yy = 0; yy < 4; yy++)
                {
                    if (BlockValue.bValues[bn, tn, xx, yy] != 0) //회전 끝난 후의 현재 도형에서 1이 차있는 네모 한 칸의 영역을 합해줌
                    {
                        Rectangle rect1 = new Rectangle((now.X + xx) * bwidth, (now.Y + yy) * bheight, bwidth, bheight);
                        Region rg1 = new Region(rect1);
                        region.Union(rg1);
                    }
                    if (BlockValue.bValues[bn, oldtn, xx, yy] != 0) //회전 이전의 영역을 합쳐줌
                    {
                        Rectangle rect1 = new Rectangle((now.X + xx) * bwidth, (now.Y + yy) * bheight, bwidth, bheight);
                        Region rg1 = new Region(rect1);
                        region.Union(rg1);
                    }
                }
            }
            return region;
        }


        private void MoveLeft()
        {
            if (game.MoveLeft())
            {
                Region rg = MakeRegion(1, 0); //rg 에는 이전 영역과 현재 영역을 합친 영역이 저장
                Invalidate(rg);
            }
        }

        private void MoveRight()
        {
            if (game.MoveRight())
            {
                Region rg = MakeRegion(-1, 0); //rg 에는 이전 영역과 현재 영역을 합친 영역이 저장
                Invalidate(rg);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            moveDown();
        }
    }
}
