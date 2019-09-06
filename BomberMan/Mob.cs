using System;
using System.Drawing;
using System.Windows.Forms;

namespace BomberMan
{
    class Mob
    {
        PictureBox mob;
        Timer timer;

        public Mob(PictureBox picMob)
        {
            mob = picMob;
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
            mob.Location = new Point(mob.Location.X, mob.Location.Y - 3);
        }
    }
}
