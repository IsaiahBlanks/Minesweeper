using MinesweeperModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper5
{
    public partial class Form1 : Form
    {
        int RowCount = 5, ColCount = 8;
        Button[,] buttons;
        MinesweeperModel.Model model = new MinesweeperModel.Model();
        public Form1()
        {
            InitializeComponent();

            InitializeComponent2();
        }

        private void InitializeComponent2()
        {
            comboBox1.SelectedIndex = 2;

            // Resize Window not allowed
            //buttons = new Button[RowCount, ColCount];
            //bool color = false;
            //for (int r = 0; r < RowCount; r++)
            //    for (int c = 0; c < ColCount; c++)
            //    {
            //        buttons[r, c] = new System.Windows.Forms.Button();
            //        buttons[r, c].Dock = DockStyle.Fill;
            //        // TabIndex in order 0..RowCount*ColCount-1
            //        this.tableLayoutPanel1.Controls.Add(buttons[r, c], c, r);
            //        buttons[r, c].Text = $"[{r}][{c}]";
            //        buttons[r, c].UseVisualStyleBackColor = true;

            //        buttons[r, c].Click += OnButtonClick;
            //        buttons[r, c].Tag = new Point(c,r);
            //      //  buttons[r, c].BackColor = color ? Color.Green : Color.GreenYellow;
            //        color = ! color;
            //    }

            //// 
            //// tableLayoutPanel1
            //// 
            //this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.tableLayoutPanel1.ColumnCount = ColCount;
            //for (int c = 0; c < ColCount; c++)
            //{
            //    this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F / ColCount)); 
            //}


            //this.tableLayoutPanel1.RowCount = RowCount;
            //for (int r = 0; r < RowCount; r++)
            //    this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F / RowCount));
           
           
        
            // TODO change cursor to arrow


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void OnButtonClick(object sender, EventArgs e)
        {
            
            Button sourceButton = (Button)sender;
            Point p = (Point) sourceButton.Tag;

            // alt not needed - loop through all buttons and check == with sender
            var changedCells = model.OpenCell(p.X, p.Y); // how to dtermine row and column?

            // UpdateUI based on move
            // OPtion1 update entire UI from model
            // Option 2 (RECOMMENDED) update cells that changed

            // if Bomb, how does model indicate that?
            buttons[p.Y,p.X].Text = ":-)";

            // Create a  New Thread
            // call in new Thread SlowComputation();
            // New thread calls Form Update to display completed

            // return before new thread completes

            // Starts an async method which calls SlowComputation
            // return from this event handler
            // async will continue running in this method

            //await SlowComputationAsync();
        }

        //private async Task SlowComputationAsync()
        //{
        //    await Task.Delay(2000);
           
        //    this.BackColor = Color.FromArgb(new Random().Next(255), new Random().Next(255), new Random().Next(255));
        //}

        private void difficultyLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            DifficultyLevel dl = (DifficultyLevel)((ComboBox)sender).SelectedIndex;
            model.Setup( dl );
            this.GridSetup( dl );
        }

        private void GridSetup(DifficultyLevel dl)
        {

            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();

            RowCount = dl.GetSize().Y;
            ColCount = dl.GetSize().X;
            this.tableLayoutPanel1.Controls.Clear();
            this.tableLayoutPanel1.ColumnStyles.Clear();
            this.tableLayoutPanel1.RowStyles.Clear();


            // Resize Window not allowed
            buttons = new Button[RowCount, ColCount];
            bool color = false;
            for (int r = 0; r < RowCount; r++)
                for (int c = 0; c < ColCount; c++)
                {
                    buttons[r, c] = new System.Windows.Forms.Button();
                    buttons[r, c].Dock = DockStyle.Fill;
                    // TabIndex in order 0..RowCount*ColCount-1
                    this.tableLayoutPanel1.Controls.Add(buttons[r, c], c, r);
                    buttons[r, c].Text = "";
                    buttons[r, c].UseVisualStyleBackColor = true;

                    buttons[r, c].Click += OnButtonClick;
                    buttons[r, c].Tag = new Point(c, r);
                    //  buttons[r, c].BackColor = color ? Color.Green : Color.GreenYellow;
                    color = !color;
                }

            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.ColumnCount = ColCount;
            for (int c = 0; c < ColCount; c++)
            {
                this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F / ColCount));
            }


            this.tableLayoutPanel1.RowCount = RowCount;
            for (int r = 0; r < RowCount; r++)
                this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F / RowCount));

            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(true);
            this.ResumeLayout(true);

            //Refresh();
        }



        private void SlowComputation()
        {
            System.Threading.Thread.Sleep(5000);
        }
    }
}
