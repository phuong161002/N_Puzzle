using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using N_Puzzle.Algorithms;
using System.Diagnostics;

namespace N_Puzzle
{
    public partial class MainForm : Form
    {
        private PictureBox[] myPictureBoxes;
        private Image[] myImages;
        private Controller controller;

        public MainForm()
        {
            InitializeComponent();
            CenterToParent();
            controller = new Controller(this);
            cbSolveType.DataSource = Enum.GetValues(typeof(SolverType));
            LoadImage();
            Control.CheckForIllegalCrossThreadCalls = false;
            UpdateGameView(Settings.StartState);
        }

        public void LoadImage()
        {
            myPictureBoxes = new PictureBox[9];
            myPictureBoxes[0] = pictureBox1;
            myPictureBoxes[1] = pictureBox2;
            myPictureBoxes[2] = pictureBox3;
            myPictureBoxes[3] = pictureBox4;
            myPictureBoxes[4] = pictureBox5;
            myPictureBoxes[5] = pictureBox6;
            myPictureBoxes[6] = pictureBox7;
            myPictureBoxes[7] = pictureBox8;
            myPictureBoxes[8] = pictureBox9;

            myImages = new Image[10];
            myImages[0] = Properties.Resources.black;
            myImages[1] = Properties.Resources.Img1;
            myImages[2] = Properties.Resources.Img2;
            myImages[3] = Properties.Resources.Img3;
            myImages[4] = Properties.Resources.Img4;
            myImages[5] = Properties.Resources.Img5;
            myImages[6] = Properties.Resources.Img6;
            myImages[7] = Properties.Resources.Img7;
            myImages[8] = Properties.Resources.Img8;
            myImages[9] = Properties.Resources.Img9;
        }

        private void UpdateGameView(int[] array)
        {
            controller.CurrentState = array;
            for (int i = 0; i < array.Length && i < myPictureBoxes.Length; i++)
            {
                myPictureBoxes[i].Image = myImages[array[i]];
            }
        }

        public void PerformMoves(List<int[]> moves)
        {
            new Thread(() =>
            {
                for (int i = 0; i < moves.Count; i++)
                {
                    UpdateGameView(moves[i]);
                    Thread.Sleep(Settings.DelayMove);
                }
            })
            { IsBackground = true }.Start();
        }

        private void btnShuffer_Click(object sender, EventArgs e)
        {
            if (int.TryParse(tbShufferItters.Text, out int num))
            {
                var state = Settings.DefaultGoalState;
                var newState = Utils.Shuffer(state, num);
                controller.CurrentState = newState;
                UpdateGameView(newState);
            }
        }

        private void btnSolve_Click(object sender, EventArgs e)
        {
            controller.Solve((SolverType)cbSolveType.SelectedItem, controller.CurrentState, Settings.DefaultGoalState);
        }
    }
}
