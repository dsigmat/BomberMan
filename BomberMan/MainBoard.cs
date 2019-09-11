using System;
using System.Drawing;
using System.Windows.Forms;

namespace BomberMan
{
    enum Sost
    {
        пусто,
        стена,
        кирпич,
        бомба,
        огонь,
        приз
    }
    class MainBoard
    {
        Panel panelGame;
        PictureBox[,] mapPic;
        Sost[,] map;
        int sizeX = 17;
        int sizeY = 11;
        static Random random = new Random();
        Player player;
        Mob mob;

        public MainBoard(Panel panel)
        {
            panelGame = panel;
            
            int boxSize;
            if ((panelGame.Width / sizeX) < (panelGame.Height / sizeY))
            {
                boxSize = panelGame.Width / sizeX;
            }
            else
            {
                boxSize = panelGame.Height / sizeY;
            }
            InitStartMap(boxSize);
            InitStartPlayer(boxSize);
            for (int i = 0; i < 5; i++)
            {
                InitMob(boxSize);
            }
            
        }
               

        private void InitStartMap(int boxSize)
        {
            mapPic = new PictureBox[sizeX, sizeY];
            map = new Sost[sizeX, sizeY];

            panelGame.Controls.Clear();

            for (int x = 0; x < sizeX; x++)
                for (int y = 0; y < sizeY; y++)
                {
                    if (x==0||y==0||x==sizeX - 1||y==sizeY - 1)
                    {
                        CreatePlace(new Point(x, y), boxSize, Sost.стена);
                    }
                    else if (x%2 == 0 && y%2 == 0)
                    {
                        CreatePlace(new Point(x, y), boxSize, Sost.стена);
                    }
                    else if (random.Next(3)==0)
                    {
                        CreatePlace(new Point(x, y), boxSize, Sost.кирпич);
                    }
                    else
                    {
                        CreatePlace(new Point(x, y), boxSize, Sost.пусто);
                    }
                }
            ChangeSost(new Point(1,1), Sost.пусто);
            ChangeSost(new Point(2, 1), Sost.пусто);
            ChangeSost(new Point(1, 2), Sost.пусто);

        }

        private void CreatePlace(Point point, int boxSize, Sost sost)
        {
            PictureBox picture = new PictureBox();
            picture.Location = new Point(point.X * (boxSize - 1), point.Y * (boxSize - 1));
            picture.Size = new Size(boxSize, boxSize);
            //picture.BorderStyle = BorderStyle.FixedSingle;
            picture.SizeMode = PictureBoxSizeMode.StretchImage;
            mapPic[point.X, point.Y] = picture;
            panelGame.Controls.Add(picture);
            ChangeSost(point, sost);
        }

        private void ChangeSost(Point point, Sost newSost)
        {
            switch (newSost)
            {
                case Sost.стена:
                    mapPic[point.X, point.Y].Image = Properties.Resources.wall;
                    break;
                case Sost.кирпич:
                    mapPic[point.X, point.Y].Image = Properties.Resources.brick;
                    break;
                case Sost.бомба:
                    mapPic[point.X, point.Y].Image = Properties.Resources.bomb;
                    break;
                case Sost.огонь:
                    mapPic[point.X, point.Y].Image = Properties.Resources.fire;
                    break;
                case Sost.приз:
                    mapPic[point.X, point.Y].Image = Properties.Resources.prize;
                    break;
                default:
                    mapPic[point.X, point.Y].Image = Properties.Resources.ground;
                    break;
            }

            map[point.X, point.Y] = newSost;
        }

        private void InitStartPlayer(int boxSize)
        {
            int x = 1;
            int y = 1;
            PictureBox picture = new PictureBox();
            picture.Location = new Point(x * (boxSize) + 7, y * (boxSize) + 3);
            picture.Size = new Size(boxSize - 14, boxSize - 6);
            picture.Image = Properties.Resources.player;
            picture.BackgroundImage = Properties.Resources.ground;
            picture.BackgroundImageLayout = ImageLayout.Stretch;
            picture.SizeMode = PictureBoxSizeMode.StretchImage;
            panelGame.Controls.Add(picture);
            picture.BringToFront();
            player = new Player(picture, mapPic, map);
        }

        private void InitMob(int boxSize)
        {
            int x = 15;
            int y = 9;
            FindEmptyPlace(out x, out y);
            PictureBox picture = new PictureBox();
            picture.Location = new Point(x * (boxSize)-8, y * (boxSize)-6);
            picture.Size = new Size(boxSize - 14, boxSize - 5);
            picture.Image = Properties.Resources.mob;
            picture.BackgroundImage = Properties.Resources.ground;
            picture.BackgroundImageLayout = ImageLayout.Stretch;
            picture.SizeMode = PictureBoxSizeMode.StretchImage;
            panelGame.Controls.Add(picture);
            picture.BringToFront();

            mob = new Mob(picture, mapPic, map);
        }

        public void MovePlayer(Arrows arrows)
        {
            if (player == null)
            {
                return;
            }
            player.MovePlayer(arrows);
        }

        public void PutBomb()
        {
            ChangeSost(player.MyNowPoint(), Sost.бомба);
        }

        private void FindEmptyPlace(out int x, out int y)
        {
            int loop = 0;
            do
            {
                x = random.Next(map.GetLength(0)/2, map.GetLength(0));
                y = random.Next(1, map.GetLength(1));
            } while (map[x,y] != Sost.пусто && loop++<100);
        }
    }
}
