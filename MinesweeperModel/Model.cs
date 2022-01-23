using System;
using System.Collections.Generic;
using System.Drawing;

namespace MinesweeperModel
{
    // Reference Game: https://www.google.com/search?q=minesweeper&rlz=1C1EJFC_enUS867US867&oq=mine&aqs=chrome.0.69i59j46i67i433j46i67j46i67i433j0i20i263i433i512j46i433i512l2j69i60.1199j0j7&sourceid=chrome&ie=UTF-8
    // There is much more to the IModel than what we completed in class
    // the first casualty of war is the plan
    public interface IModel
    {
        //ops
        /// <summary>
        /// Opens Cell
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <returns>List of Points that changed due to this operation. If Bomb, that point will be only returned value</returns>
        List<System.Drawing.Point> OpenCell(int col, int row, List<Point> points);
        void Setup(DifficultyLevel level);
        void FlagCell(int col, int row, bool flagOn);
        int GetTime(); // In Seconds

        Cell GetCell(int col, int row);
    }

    public class Model : IModel
    {
        //imp
        internal Cell[,] board;

        public DifficultyLevel difficultyLevel { get; private set; }
        public int bombCount { get; private set; }

        public void FlagCell(int col, int row, bool flagOn)
        {
            // data validation
            board[col, row].IsFlagged = flagOn;
        }

        public Cell GetCell(int col, int row)
        {
            return board[col, row];
        }

        public int GetTime()
        {
            throw new NotImplementedException();
        }

        public List<Point> OpenCell(int col, int row, List<Point> affected)
        {
            List<Point> affectedCells = affected;
            if (board[col, row].IsBomb || !(board[col, row].NeighborCount == 0))
            {
                board[col, row].IsFlagged = true;
                affectedCells.Add(new Point(col, row));
            } 
            else
            {
                board[col, row].IsFlagged = true;
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        int colAdd = col + i;
                        int rowAdd = row + j;
                        if (rowAdd < 0 || colAdd < 0 || colAdd >= board.GetLength(0) || rowAdd >= board.GetLength(1))
                        {
                            continue;
                        }
                        if (board[col + i, row + j].IsBomb || board[col + i, row + j].NeighborCount > 0)
                        {
                            board[col + i, row + j].IsFlagged = true;
                            affectedCells.Add(new Point(col + i, row + j));
                        }
                        else if (!affectedCells.Contains(new Point(col + i, row + j)) && board[col + i, row + j].NeighborCount < 1)
                        {
                            board[col + i, row + j].IsFlagged = true;
                            affectedCells.Add(new Point(col + i, row + j));
                            OpenCell(col + i, row + j, affectedCells);
                        }
                    }
                }
            }
            return affectedCells;
        }
    
        public void Setup(DifficultyLevel level)
        {
            this.difficultyLevel = level;
            this.bombCount = level.GetBombNumber();


            board = new Cell[difficultyLevel.GetSize().Y, difficultyLevel.GetSize().X];
            for (int x = 0; x < board.GetLength(1); x++)
                for (int y = 0; y < board.GetLength(0); y++)
                    board[y, x] = new Cell();

            int bombsLeft = bombCount;
            while (bombsLeft > 0)
            {
                var random = new Random();
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    for (int j = 0; j < board.GetLength(0); j++)
                    {
                        if (random.NextDouble() > 0.85f && !board[i, j].IsBomb)
                        {
                            board[i, j].IsBomb = true;
                            bombsLeft--;
                        }
                    }
                }
            }

            SetAllNeighborCounts();

        }

        public void SetAllNeighborCounts()
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (!board[i, j].IsBomb)
                    {
                        board[i, j].NeighborCount = CalculateNeighborCount(i, j);
                    }
                }
            }
        }

        private int CalculateNeighborCount(int i, int j)
        {
            int count = 0;
            for (int k = i - 1; k <= i+1; k++)
            {
                for (int l = j - 1; l <= j+1; l++)
                {
                    if(k >= 0 && l >= 0 && k < board.GetLength(0) && l < board.GetLength(1))
                    {
                        if(board[k,l].IsBomb)
                        {
                            count++;
                        }
                    }
                }
            }
            return count;
        }

        /*
        List<Point> IModel.OpenCell(int col, int row)
        {
            List<Point> affectedCells = new List<Point>();
            if (!board[col, row].IsBomb)
            {
                int neighborCount = 0;
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        int addCol = col + i;
                        int addRow = row + j;
                        if (addCol >= 0 && addRow >= 0)
                        {
                            affectedCells.Add(new Point(addCol, addRow));
                            if (board[addCol, addRow].IsBomb)
                            {
                                neighborCount++;
                            }
                        }

                    }
                }
                board[col, row].NeighborCount = neighborCount;
            } else
            {
                affectedCells.Add(new Point(col, row));
            }
            return affectedCells;
        }
        */
    }


}
