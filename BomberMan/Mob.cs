using System;
using System.Drawing;
using System.Windows.Forms;

namespace BomberMan
{
    class Mob
    {
        /// <summary>
        /// Уровни сложности:
        ///     1. Самый глупый - выбирает доступную точку и бежит к ней.
        ///     2. Умный        - выбирает доступную точку и бежит к ней, если видит бомбу - убегает.
        ///     3. Самый умный  - бегает от точки к точке, если доступен человек - бежит к нему, если встретил бомбу - убегает.
        /// </summary>

        PictureBox mob;
        Timer timer;
        Point destinePlace;
        Point mobPlace;
        MovingClass moving;
        int step = 3;
        Sost[,] map;
        int[,] fmap;
        int paths;
        Point[] path;
        int pathStep;
        static Random rand = new Random();

        public Mob(PictureBox picMob, PictureBox[,] _mapPic, Sost[,] _map)
        {
            mob = picMob;
            map = _map;
            fmap = new int[map.GetLength(0), map.GetLength(1)];
            path = new Point[map.GetLength(0) * map.GetLength(1)];
            moving = new MovingClass(picMob, _mapPic, _map);
            mobPlace = moving.MyNowPoint();
            destinePlace = new Point(15, 7);
            CreateTimer();
            timer.Enabled = true;
        }

        private void CreateTimer()
        {
            timer = new Timer();
            timer.Interval = 50;
            timer.Tick += timer_Tick;   
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (mobPlace == destinePlace) GetNewPlace();
            if (path[0].X == 0 & path[0].Y == 0)
                if (!FindPath()) return;
            if (pathStep > paths) return;
            if (path[pathStep] == mobPlace)
                pathStep++;
            else
                MoveMob(destinePlace);
        }

        private void MoveMob(Point newPlace)
        {
            int sx = 0;
            int sy = 0;
            if (mobPlace.X < newPlace.X)
            {
                sx = newPlace.X - mobPlace.X > step ? step : newPlace.X - mobPlace.X;
            }
            else
            {
                sx = mobPlace.X - newPlace.X < step ? newPlace.X - mobPlace.X : -step;
            }

            if (mobPlace.Y < newPlace.Y)
            {
                sy = newPlace.Y - mobPlace.Y > step ? step : newPlace.Y - mobPlace.Y;
            }
            else
            {
                sy = mobPlace.Y - newPlace.Y < step ? newPlace.Y - mobPlace.Y : -step;
            }
            moving.Move(sx, sy);
            mobPlace = moving.MyNowPoint();
        }

        private bool FindPath()
        {
            for (int x = 0; x < map.GetLength(0); x++)
                for (int y = 0; y < map.GetLength(1); y++)
                    fmap[x, y] = 0;
            bool added;
            bool found = false;
            fmap[mobPlace.X, mobPlace.Y] = 1;
            int nr = 1;
            do
            {
                added = false;
                for (int x = 0; x < map.GetLength(0); x++)
                    for (int y = 0; y < map.GetLength(1); y++)
                        if (fmap[x,y] == nr)
                        {
                            MarkPath(x + 1, y, nr + 1);
                            MarkPath(x - 1, y, nr + 1);
                            MarkPath(x, y + 1, nr + 1);
                            MarkPath(x, y - 1, nr + 1);
                            added = true;
                        }
                if (fmap[destinePlace.X, destinePlace.Y] > 0)
                {
                    found = true;
                    break;
                }
                nr++;

            } while (added);
            if (!found) return false;
            int sx = destinePlace.X;
            int sy = destinePlace.Y;
            paths = nr;
            while (nr>=0)
            {
                path[nr].X = sx;
                path[nr].Y = sy;
                if (IsPath(sx + 1, sy, nr)) sx++;
                else if (IsPath(sx - 1, sy, nr)) sx--;
                else if (IsPath(sx, sy + 1, nr)) sx++;
                else if (IsPath(sx, sy - 1, nr)) sx--;
                nr--;
            }
            pathStep = 0;
            return true;
        }

        private void MarkPath(int x, int y, int n)
        {
            if (x < 0 || x >= map.GetLength(0)) return;
            if (y < 0 || y >= map.GetLength(1)) return;
            if (fmap[x, y] > 0) return;
            fmap[x, y] = n;
        }

        private bool IsPath(int x, int y, int n)
        {
            if (x < 0 || x >= map.GetLength(0)) return false;
            if (y < 0 || y >= map.GetLength(1)) return false;
            return fmap[x, y] == n;
        }

        private void GetNewPlace()
        {
            int loop = 0;
            do
            {
                destinePlace.X = rand.Next(1, map.GetLength(0)-1);
                destinePlace.Y = rand.Next(1, map.GetLength(1) - 1);
            } while (!FindPath() && loop++ < 100);
            if (loop>=100)
            {
                destinePlace = mobPlace;
            }
        }
    }
}
