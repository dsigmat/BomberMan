using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BomberMan
{
    enum Arrows
    {
        left,
        right,
        up,
        down
    }
    class Player
    {
        PictureBox player;
        int step;
        MovingClass moving;
        public List<Bomb> bombs { get; private set; }
        int kolBomb;
        public int lenFire { get; private set; }

        public Player(PictureBox _player, PictureBox[,] _mapPic, Sost[,] _map)
        {
            player = _player;
            step = 3;
            kolBomb = 3;
            lenFire = 3;
            moving = new MovingClass(_player, _mapPic, _map);
            bombs = new List<Bomb>();
        }

        public void MovePlayer(Arrows arrows)
        {
            switch (arrows)
            {
                case Arrows.left:
                    moving.Move(-step, 0);
                    break;
                case Arrows.right:
                    moving.Move(step, 0);
                    break;
                case Arrows.up:
                    moving.Move(0, -step);
                    break;
                case Arrows.down:
                    moving.Move(0, step);
                    break;
                default:
                    break;
            }
        }
        
        public Point MyNowPoint()
        {
            return moving.MyNowPoint();
        }

        public bool PutBomb(PictureBox[,] mapPic, deBabah _deBabah)
        {
            if (bombs.Count >= kolBomb) return false;
            Bomb bomb = new Bomb(mapPic, MyNowPoint(), _deBabah);
            bombs.Add(bomb);
            return true;
        }
        
        public void RemoveBomb(Bomb bomb)
        {
            bombs.Remove(bomb);
        }
    }
}
