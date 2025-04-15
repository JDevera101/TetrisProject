using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class TetrisBoard
    {
        private const int Width = 12;
        private const int Height = 20;
        private readonly Color[,] grid;
        private Block currentBlock;
        private Block nextBlock;
        private Point blockPosition;
        public int line = 0;
        public int level = 1;
        private Random random;

        public TetrisBoard()
        {
            grid = new Color[Width, Height];
            random = new Random();
            currentBlock = new Block(random.Next(0, 7));
            nextBlock = new Block(random.Next(0, 7));
        }

        public bool SpawnBlock(Block block)
        {
            currentBlock = nextBlock;
            nextBlock = new Block(random.Next(0, 7));
            blockPosition = new Point(Width / 2 - 1, 0);
            return !IsCollision();
        }
        private bool IsCollision()
        {
            foreach (Point cell in currentBlock.Cells)
            {
                int x = blockPosition.X + cell.X;
                int y = blockPosition.Y + cell.Y;

                if (x < 0 || x >= Width || y < 0 || y >= Height) return true;
                if (grid[x, y] != Color.Empty) return true;
            }

            return false;
        }

        public void MoveBlock(int deltaX, int deltaY)
        {
            blockPosition.X += deltaX;
            blockPosition.Y += deltaY;

            if (IsCollision())
            {
                blockPosition.X -= deltaX;
                blockPosition.Y -= deltaY;

                if (deltaY > 0)
                {
                    PlaceBlock();
                    ClearLines();
                    SpawnBlock(new Block(new Random().Next(0, 7)));

                }
            }
        }

        public void RotateBlock()
        {
            currentBlock.Rotate();
            if (IsCollision())
            {
                // Undo rotation if there is a collision
                for (int i = 0; i < 3; i++) currentBlock.Rotate();

            }
        }
        public bool IsGameOver()
        {
            for (int x = 0; x < Width; x++)
            {
                if (grid[x, 0] != Color.Empty) return true;
            }
            return false;
        }

        public void PlaceBlock()
        {
            foreach (Point cell in currentBlock.Cells)
            {
                int x = blockPosition.X + cell.X;
                int y = blockPosition.Y + cell.Y;
                if (x >= 0 && x < Width && y >= 0 && y < Height)
                    grid[x, y] = Color.FromArgb(181, 52, 113);
            }
        }

        private void ClearLines()
        {
            for (int y = Height - 1; y >= 0; y--)
            {
                bool isFull = true;
                for (int x = 0; x < Width; x++)
                {
                    if (grid[x, y] == Color.Empty)
                    {
                        isFull = false;
                        break;
                    }
                }

                if (isFull)
                {
                    // Clear Line and move everything down
                    for (int yy = y; yy > 0; yy--)
                    {
                        for (int xx = 0; xx < Width; xx++)
                        {
                            grid[xx, yy] = grid[xx, yy - 1];
                        }
                    }

                    for (int xx = 0; xx < Width; xx++)
                    {
                        grid[xx, 0] = Color.Empty;
                    }
                    y++; // Check the same line again
                    line++;
                    double rm = line % 6;
                    if (rm == 0 && line != 0) level++;


                }
            }
        }
        public void Draw(Graphics g, int cellSize)
        {
            Pen gridPen = new Pen(Color.FromArgb(20, 20, 20), 5);

            // Draw Grid
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    g.DrawRectangle(gridPen, x * cellSize, y * cellSize, cellSize, cellSize);
                    if (grid[x, y] != Color.Empty)
                    {
                        using (Brush brush = new SolidBrush(grid[x, y]))
                        {
                            g.FillRectangle(brush, x * cellSize, y * cellSize, cellSize, cellSize);
                        }
                    }

                }
            }

            // Draw current Block

            if (currentBlock != null)
            {
                foreach (Point cell in currentBlock.Cells)
                {
                    int drawX = (blockPosition.X + cell.X) * cellSize;
                    int drawY = (blockPosition.Y + cell.Y) * cellSize;
                    using (Brush brush = new SolidBrush(currentBlock.Color))
                    {
                        g.FillRectangle(brush, drawX, drawY, cellSize - 2, cellSize - 2);
                    }
                }
            }



        }
        public void DrawNextBlock(Graphics g, int cellSize, int canvasWidth, int canvasHeight)
        {
            g.Clear(Color.FromArgb(30, 30, 30));

            int minX = nextBlock.Cells.Min(cell => cell.X);
            int maxX = nextBlock.Cells.Max(cell => cell.X);
            int minY = nextBlock.Cells.Min(cell => cell.Y);
            int maxY = nextBlock.Cells.Max(cell => cell.Y);

            int blockWidth = (maxX - minX + 1) * cellSize;
            int blockHeight = (maxY - minY + 1) * cellSize;

            // Calculate the offset to center the block
            int offsetX = (canvasWidth - blockWidth) / 2;
            int offsetY = (canvasHeight - blockHeight) / 2;

            foreach (Point cell in nextBlock.Cells)
            {
                int x = (cell.X - minX) * cellSize + offsetX;
                int y = (cell.Y - minY) * cellSize + offsetY;
                using (Brush brush = new SolidBrush(nextBlock.Color))
                {
                    g.FillRectangle(brush, x, y, cellSize - 2, cellSize - 2);
                }
            }

        }




    }
}