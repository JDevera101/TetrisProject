using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tetris;

namespace Tetris02
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            lblHighScore.Text = Properties.Settings.Default.HighScore.ToString();  
        }
        private TetrisBoard board;
        int cellSize = 25;
        bool gameStarted = false;

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            lblLine.Text = board.line.ToString();
            lblScore.Text = (board.line * 36).ToString();
            board.MoveBlock(0, 1);
            CheckHighScore();

            //ToDO Check High Score
            lblLevel.Text = board.level.ToString(); 
            if (board.IsGameOver())
            {
                gameTimer.Stop();
                MessageBox.Show("Game Over", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                btnStart.Text = "Start";
                btnStart.BackColor = Color.ForestGreen;
                gameStarted = false;
                board = null;
                lblLevel.Text = "1";
                lblLine.Text = "0";
                lblScore.Text = "0";
            }
            Canvas.Invalidate();
            picBoxNextBlock.Invalidate();

        }
        private void CheckHighScore()
        {
            if (int.Parse(lblScore.Text) > Properties.Settings.Default.HighScore)
            {
                Properties.Settings.Default.HighScore = int.Parse(lblScore.Text);
                Properties.Settings.Default.Save();
            }
            lblHighScore.Text = Properties.Settings.Default.HighScore.ToString();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    board.MoveBlock(-1, 0);
                    break;
                case Keys.Right:
                    board.MoveBlock(1, 0);
                    break;
                case Keys.Down:
                    board.MoveBlock(0, 1);
                    break;
                case Keys.Up:
                    board.RotateBlock();
                    break;
                
            }
            Canvas.Invalidate();
            picBoxNextBlock.Invalidate();
        }

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            if (board != null)
                board.Draw(e.Graphics, cellSize);
        }

        private void picBoxNextBlock_Paint(object sender, PaintEventArgs e)
        {
            if (board != null)
                board.DrawNextBlock(e.Graphics, 30, picBoxNextBlock.Width, picBoxNextBlock.Height);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {

            if (board == null)
            {
                board = new TetrisBoard();
                //board.SpawnBlock(new Block(new Random().Next(0, 7)));
            }

            if (!gameStarted)
            {
                gameTimer.Start();
                gameStarted = true;
                btnStart.Text = "Pause";
                btnStart.BackColor = Color.DarkOrange;
            }
            else
            {
                gameTimer.Stop();
                gameStarted = false;
                btnStart.Text = "Continue";
                btnStart.BackColor = Color.Green;
            }
            Canvas.Focus();
        }

        
    }
}
