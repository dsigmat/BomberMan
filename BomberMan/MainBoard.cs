using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BomberMan
{
    public delegate void deBabah(Bomb b);
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
        List<Mob> mobs;

        public MainBoard(Panel panel)
        {
            panelGame = panel;
            mobs = new List<Mob>();

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
                    if (x == 0 || y == 0 || x == sizeX - 1 || y == sizeY - 1)
                    {
                        CreatePlace(new Point(x, y), boxSize, Sost.стена);
                    }
                    else if (x % 2 == 0 && y % 2 == 0)
                    {
                        CreatePlace(new Point(x, y), boxSize, Sost.стена);
                    }
                    else if (random.Next(3) == 0)
                    {
                        CreatePlace(new Point(x, y), boxSize, Sost.кирпич);
                    }
                    else
                    {
                        CreatePlace(new Point(x, y), boxSize, Sost.пусто);
                    }
                }
            ChangeSost(new Point(1, 1), Sost.пусто);
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
            picture.Location = new Point(x * (boxSize) - 8, y * (boxSize) - 6);
            picture.Size = new Size(boxSize - 14, boxSize - 6);
            picture.Image = Properties.Resources.mob;
            picture.BackgroundImage = Properties.Resources.ground;
            picture.BackgroundImageLayout = ImageLayout.Stretch;
            picture.SizeMode = PictureBoxSizeMode.StretchImage;
            panelGame.Controls.Add(picture);
            picture.BringToFront();

            mobs.Add(new Mob(picture, mapPic, map));
        }

        public void MovePlayer(Arrows arrows)
        {
            if (player == null)
            {
                return;
            }
            player.MovePlayer(arrows);
        }

        private void FindEmptyPlace(out int x, out int y)
        {
            int loop = 0;
            do
            {
                x = random.Next(map.GetLength(0) / 2, map.GetLength(0));
                y = random.Next(1, map.GetLength(1));
            } while (map[x, y] != Sost.пусто && loop++ < 100);
        }

        public void PutBomb()
        {
            Point playerPoint = player.MyNowPoint();
            if (map[playerPoint.X, playerPoint.Y] == Sost.бомба) return;
            if (player.PutBomb(mapPic, Babah))
                ChangeSost(player.MyNowPoint(), Sost.бомба);
        }

        private void Babah(Bomb bomb)
        {
            ChangeSost(bomb.bombPlace, Sost.огонь);
            Flame(bomb.bombPlace, Arrows.left);
            Flame(bomb.bombPlace, Arrows.right);
            Flame(bomb.bombPlace, Arrows.up);
            Flame(bomb.bombPlace, Arrows.down);
            player.RemoveBomb(bomb);

            Blaze();
        }

        private void Blaze()
        {
            List<Mob> delMobs = new List<Mob>();
            foreach (Mob mob in mobs)
            {
                Point mobPoint = mob.MyNowPoint();
                if (map[mobPoint.X, mobPoint.Y] == Sost.огонь)
                    delMobs.Add(mob);
            }
            for (int x = 0; x < delMobs.Count; x++)
            {
                mobs.Remove(delMobs[x]);
                panelGame.Controls.Remove(delMobs[x].mob);
                delMobs[x] = null;
            }
        }

        private void Flame(Point bombPlace, Arrows arrow)
        {
            int sx = 0, sy = 0;
            switch (arrow)
            {
                case Arrows.left:
                    sx = -1;
                    break;
                case Arrows.right:
                    sx = 1;
                    break;
                case Arrows.up:
                    sy = -1;
                    break;
                case Arrows.down:
                    sy = 1;
                    break;
                default:
                    break;
            }

            bool isNotDone = true;
            int x = 0, y = 0;
            do
            {
                x += sx; y += sy;
                if (Math.Abs(x) > player.lenFire || Math.Abs(y) > player.lenFire) break;
                if (isFire(bombPlace, x, y))
                    ChangeSost(new Point(bombPlace.X + x, bombPlace.Y + y), Sost.огонь);
                isNotDone = false;
            } while (isNotDone);
        }

        private bool isFire(Point place, int sx, int sy)
        {
            switch (map[place.X + sx, place.Y + sy])
            {
                case Sost.пусто:
                    return true;
                case Sost.стена:
                    return false;
                case Sost.кирпич:
                    ChangeSost(new Point(place.X + sx, place.Y + sy), Sost.огонь);
                    return false;
                case Sost.бомба:
                    foreach (Bomb bomb in player.bombs)
                    {
                        if (bomb.bombPlace == new Point(place.X + place.Y + sy))
                            bomb.Reaction();
                    }
                    return false;
                default:
                    return true;
            }
        }
    }
}
