﻿using System.Drawing;
using System.Windows.Forms;

namespace BomberMan
{
    class MovingClass
    {
        PictureBox player;
        PictureBox[,] mapPic;
        Sost[,] map;

        public MovingClass(PictureBox _player, PictureBox[,] _mapPic, Sost[,] _map)
        {
            player = _player;
            mapPic = _mapPic;
            map = _map;
        }
        public void Move(int sx, int sy)
        {
            if (IsEmpty(ref sx, ref sy))
            {
                player.Location = new Point(player.Location.X + sx, player.Location.Y + sy);
            }

        }

        private bool IsEmpty(ref int sx, ref int sy)
        {
            Point playerPoint = MyNowPoint();

            int playerRight = player.Location.X + player.Size.Width;
            int playerLeft = player.Location.X;
            int playerDown = player.Location.Y + player.Size.Height;
            int playerUp = player.Location.Y;

            int rightWallLeft = mapPic[playerPoint.X + 1, playerPoint.Y].Location.X;
            int leftWallRight = mapPic[playerPoint.X - 1, playerPoint.Y].Location.X + mapPic[playerPoint.X - 1, playerPoint.Y].Size.Width;
            int downWallUp = mapPic[playerPoint.X, playerPoint.Y + 1].Location.Y;
            int UpWallDown = mapPic[playerPoint.X, playerPoint.Y - 1].Location.Y + mapPic[playerPoint.X, playerPoint.Y - 1].Size.Height;

            int rightUpWallDown = mapPic[playerPoint.X + 1, playerPoint.Y - 1].Location.Y + mapPic[playerPoint.X + 1, playerPoint.Y - 1].Size.Height;
            int rightDownWallUp = mapPic[playerPoint.X + 1, playerPoint.Y + 1].Location.Y;
            int leftUpWallDown = mapPic[playerPoint.X - 1, playerPoint.Y - 1].Location.Y + mapPic[playerPoint.X - 1, playerPoint.Y - 1].Size.Height;
            int leftDownWallUp = mapPic[playerPoint.X - 1, playerPoint.Y + 1].Location.Y;

            int rightUpWallLeft = mapPic[playerPoint.X + 1, playerPoint.Y - 1].Location.X;
            int leftUpWallRight = mapPic[playerPoint.X - 1, playerPoint.Y - 1].Location.X + mapPic[playerPoint.X - 1, playerPoint.Y - 1].Size.Width;
            int rightDownWallLeft = mapPic[playerPoint.X + 1, playerPoint.Y + 1].Location.X;
            int leftDownWallRight = mapPic[playerPoint.X - 1, playerPoint.Y + 1].Location.X + mapPic[playerPoint.X - 1, playerPoint.Y + 1].Size.Width;

            int offset = 3;
            if (sx > 0 && map[playerPoint.X + 1, playerPoint.Y] == Sost.пусто)
            {
                if (playerUp < rightUpWallDown)
                {
                    if (rightUpWallDown - playerUp > offset)
                    {
                        sy = offset;
                    }
                    else
                    {
                        sy = rightUpWallDown - playerUp;
                    }

                }
                if (playerDown > rightDownWallUp)
                {
                    if (rightDownWallUp - playerDown < -offset)
                    {
                        sy = -offset;
                    }
                    else
                    {
                        sy = rightDownWallUp - playerDown;
                    }
                }
                return true;
            }
            if (sx < 0 && map[playerPoint.X - 1, playerPoint.Y] == Sost.пусто)
            {
                if (playerUp < leftUpWallDown)
                {
                    if (leftUpWallDown - playerUp > offset)
                    {
                        sy = offset;
                    }
                    else
                    {
                        sy = leftUpWallDown - playerUp;
                    }
                }
                if (playerDown > leftDownWallUp)
                {
                    if (leftDownWallUp - playerDown < -offset)
                    {
                        sy = -offset;
                    }
                    sy = leftDownWallUp - playerDown;
                }
                return true;
            }
            if (sy > 0 && map[playerPoint.X, playerPoint.Y + 1] == Sost.пусто)
            {
                if (playerRight > rightDownWallLeft)
                {
                    if (rightDownWallLeft - playerRight < -offset)
                    {
                        sx = -offset;
                    }
                    else
                    {
                        sx = rightDownWallLeft - playerRight;
                    }
                }
                if (playerLeft < leftDownWallRight)
                {
                    if (leftDownWallRight - playerLeft > offset)
                    {
                        sx = offset;
                    }
                    else
                    {
                        sx = leftDownWallRight - playerLeft;
                    }
                }
                return true;
            }
            if (sy < 0 && map[playerPoint.X, playerPoint.Y - 1] == Sost.пусто)
            {
                if (playerRight > rightUpWallLeft)
                {
                    if (rightUpWallLeft - playerRight < -offset)
                    {
                        sx = -offset;
                    }
                    else
                    {
                        sx = rightUpWallLeft - playerRight;
                    }

                }
                if (playerLeft < leftUpWallRight)
                {
                    if (leftUpWallRight - playerLeft > offset)
                    {
                        sx = offset;
                    }
                    else
                    {
                        sx = leftUpWallRight - playerLeft;
                    }
                }
                return true;
            }

            if (sx > 0 && playerRight + sx > rightWallLeft)
            {
                sx = rightWallLeft - playerRight;
            }
            if (sx < 0 && playerLeft + sx < leftWallRight)
            {
                sx = leftWallRight - playerLeft;
            }
            if (sy > 0 && playerDown + sy > downWallUp)
            {
                sy = downWallUp - playerDown;
            }
            if (sy < 0 && playerUp + sy < UpWallDown)
            {
                sy = UpWallDown - playerUp;
            }

            return true;
        }

        public Point MyNowPoint()
        {
            Point point = new Point();
            {
                point.X = player.Location.X + player.Size.Width / 2;
                point.Y = player.Location.Y + player.Size.Height / 2;
            }
            for (int x = 0; x < mapPic.GetLength(0); x++)
                for (int y = 0; y < mapPic.GetLength(1); y++)
                {
                    if (
                        mapPic[x, y].Location.X < point.X &&
                        mapPic[x, y].Location.Y < point.Y &&
                        mapPic[x, y].Location.X + mapPic[x, y].Size.Width > point.X &&
                        mapPic[x, y].Location.Y + mapPic[x, y].Size.Height > point.Y)
                        return new Point(x, y);
                }

            return point;
        }
    }
}
