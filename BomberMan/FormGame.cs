using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BomberMan
{
    public delegate void deClear();
    public partial class FormGame : Form
    {
        MainBoard board;
        int level = 1;

        public FormGame()
        {
            InitializeComponent();
            NewGame();
        }

        private void NewGame()
        {
            board = new MainBoard(panelGame, StartClear, labelScore);
            ChangeLevel(level);
            timerGameOver.Enabled = true;
        }

        private void aboutTheGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(@"
At the beginning of the game you are given one 
bomb and a single explosion range, there is no 
detonator yet. We must go and destroy the monsters 
with bombs. The main goal of each sublevel is to 
destroy the blue (or gray?) Devices. After the 
destruction of all these devices, a passage to the 
next sublevel opens, and at the same time bonuses 
appear - coins in place of the remaining destructible 
walls. It is necessary to collect coins and go to the 
next sublevel.
", "About The Game");
        }

        private void aboutTheAuthorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(@"
Hudson Soft Company, Limited (Japanese 株式会社 ハ ド ソ ン) 
is a Japanese company specializing in electronic entertainment 
and video games, founded in 1973. At first, the company was 
engaged only in products for personal computers, but subsequently 
expanded its activities to the development and publication of video 
games, mobile content and gaming devices. In 2003, the company 
had offices in Sapporo, Nagoya, Osaka, Tokyo and California, 
it employs more than 500 people. The company is currently owned 
by Konami Corporation.
", "About The Author");
        }

        private void FormGame_KeyDown(object sender, KeyEventArgs e)
        {
            if(timerGameOver.Enabled)
                switch (e.KeyCode)
                {
                    case Keys.Left:
                        board.MovePlayer(Arrows.left);
                        break;
                    case Keys.Right:
                        board.MovePlayer(Arrows.right);
                        break;
                    case Keys.Up:
                        board.MovePlayer(Arrows.up);
                        break;
                    case Keys.Down:
                        board.MovePlayer(Arrows.down);
                        break;
                    case Keys.Space:
                        board.PutBomb();
                        break;
                    default:
                        break;
                }
        }

        private void timerFireClear_Tick(object sender, EventArgs e)
        {
            board.ClearFire();
            timerFireClear.Enabled = false;
        }

        private void StartClear()
        {
            timerFireClear.Enabled = true;
        }

        private void timerGameOver_Tick(object sender, EventArgs e)
        {
            if (board.GameOver())
            {
                timerGameOver.Enabled = false;
                DialogResult dr = MessageBox.Show("Хотите начать заново?", "Конец игре!",
                                                    MessageBoxButtons.YesNo,
                                                    MessageBoxIcon.Question);
                if(dr == System.Windows.Forms.DialogResult.Yes)
                {
                    NewGame();
                }
            }
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ChangeLevel(int _level)
        {
            level = _level;
            board.SetMobLevel(level);
        }

        private void умнееToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeLevel(2);
        }

        private void самыйУмныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeLevel(3);
        }

        private void самыйГлупыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeLevel(1);
        }
    }
}
