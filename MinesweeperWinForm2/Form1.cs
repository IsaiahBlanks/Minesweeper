using MinesweeperModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper5
{
    public partial class Form1 : Form
    {
        int RowCount = 5, ColCount = 8;
        Button[,] buttons;
        Model model = new Model();
        List<Point> changedCells = new List<Point>();
        bool firstClick = true;
        private DateTime start;

        public Form1()
        {
            InitializeComponent();

            InitializeComponent2();
        }

        private void InitializeComponent2()
        {
            comboBox1.SelectedIndex = 2;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
            timer1.Tick += TimerTick;
            start = DateTime.Now;
        }

        private void TimerTick(object sender, EventArgs e)
        {

            label2.Text = "Seconds: " + ((int) (DateTime.Now - start).TotalSeconds).ToString();
        }


        public void FormThreadProcedure()
        {
            SlowComputation();
        }

        private async void OnButtonClick(object sender, EventArgs e)
        {

            Button sourceButton = (Button)sender;
            Point p = (Point)sourceButton.Tag;

            changedCells = model.OpenCell(p.X, p.Y, new List<Point>());
            UpdateForm(changedCells);

            Thread t = new Thread(new ThreadStart(FormThreadProcedure));
            t.Start();

            await SlowComputationAsync();
        }

        private void UpdateForm(List<Point> changedCells)
        {
            foreach (Point pnt in changedCells)
            {
                buttons[pnt.Y, pnt.X].Enabled = false;
                buttons[pnt.Y, pnt.X].BackColor = Color.ForestGreen;
                //if the games first click is a bomb, move the bomb somewhere else
                if (firstClick && model.GetCell(pnt.X, pnt.Y).IsBomb)
                {
                    bool switched = false;
                    Random random = new Random();
                    while (!switched)
                    {
                        int row = random.Next(0, RowCount);
                        int col = random.Next(0, ColCount);
                        if (!model.GetCell(col, row).IsBomb)
                        {
                            model.GetCell(col, row).IsBomb = true;
                            model.GetCell(pnt.X, pnt.Y).IsBomb = false;
                            model.SetAllNeighborCounts();
                            switched = true;
                        }
                    }
                } else if (firstClick)
                {
                    firstClick = false;
                }

                if (model.GetCell(pnt.X, pnt.Y).IsBomb && !firstClick)
                {
                    buttons[pnt.Y, pnt.X].Text = "X";
                    foreach (Button b in buttons)
                    {
                        b.Enabled = false;
                        b.BackColor = Color.ForestGreen;
                    }
                    label1.Text = "Game Over!";
                }
                else if (model.GetCell(pnt.X, pnt.Y).NeighborCount > 0)
                {
                    buttons[pnt.Y, pnt.X].Text = model.GetCell(pnt.X, pnt.Y).NeighborCount.ToString();
                }
            }
        }

        private async Task SlowComputationAsync()
        {
            Color[] color = new Color[] { Color.Red, Color.Orange, Color.Yellow, Color.Blue, Color.Indigo, Color.Violet };
            Random random = new Random();
            await Task.Delay(2000);
            if (label1.Text == "Game Over!")
            {
                for (int i = 0; i < buttons.GetLength(0); i++)
                {
                    for (int j = 0; j < buttons.GetLength(1); j++)
                    {
                        if (model.GetCell(j, i).IsBomb && !model.GetCell(j, i).IsFlagged)
                        {
                            buttons[i, j].BackColor = color[random.Next(0, 6)];
                            buttons[i, j].Text = "X";
                            model.GetCell(j, i).IsFlagged = true;
                            await Task.Delay(50);
                        }
                    }
                }
            }
       
        }

        private void difficultyLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            DifficultyLevel dl = (DifficultyLevel)((ComboBox)sender).SelectedIndex;
            model.Setup(dl);
            this.GridSetup(dl);
        }

        private void GridSetup(DifficultyLevel dl)
        {

            ((ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();

            RowCount = dl.GetSize().X;
            ColCount = dl.GetSize().Y;
            this.tableLayoutPanel1.Controls.Clear();
            this.tableLayoutPanel1.ColumnStyles.Clear();
            this.tableLayoutPanel1.RowStyles.Clear();

            firstClick = true;

            // Resize Window not allowed
            buttons = new Button[RowCount, ColCount];
            bool color = false;
            for (int r = 0; r < RowCount; r++)
                for (int c = 0; c < ColCount; c++)
                {
                    buttons[r, c] = new Button();
                    buttons[r, c].Dock = DockStyle.Fill;
                    // TabIndex in order 0..RowCount*ColCount-1
                    this.tableLayoutPanel1.Controls.Add(buttons[r, c], c, r);
                    buttons[r, c].Text = "";
                    buttons[r, c].UseVisualStyleBackColor = true;

                    buttons[r, c].Click += OnButtonClick;
                    buttons[r, c].Tag = new Point(c, r);
                    buttons[r, c].BackColor = color ? Color.Green : Color.GreenYellow;
                    buttons[r, c].Font = new Font("Verdana", 12, FontStyle.Bold);
                    buttons[r, c].Padding = new Padding(0);
                    buttons[r, c].Margin = new Padding(0);
                    color = !color;
                }

            // 
            // tableLayoutPanel1
            // 

            label1.Text = "";
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.ColumnCount = ColCount;
            for (int c = 0; c < ColCount; c++)
            {
                this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F / ColCount));
            }


            this.tableLayoutPanel1.RowCount = RowCount;
            for (int r = 0; r < RowCount; r++)
                this.tableLayoutPanel1.RowStyles.Add(new RowStyle(System.Windows.Forms.SizeType.Percent, 100F / RowCount));

            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(true);
            this.ResumeLayout(true);

            start = DateTime.Now;
            //Refresh();
        }



        private static void SlowComputation()
        {
            System.Threading.Thread.Sleep(5000);
        }
    }
}

