using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class Block
    {
        public Point[] Cells { get; private set; }
        public Color Color { get; private set; }
        private int rotationIndex;

        private static readonly Point[][] BlockShapes = new Point[][]
        {
        new Point[] { new Point(0,0), new Point(1,0), new Point(2,0), new Point(3,0) }, // I
        new Point[] { new Point(0,0), new Point(1,0), new Point(0,1), new Point(1,1) }, // O
        new Point[] { new Point(0,1), new Point(1,0), new Point(1,1), new Point(2,1) }, // T
        new Point[] { new Point(0,1), new Point(1,1), new Point(1,0), new Point(2,0) }, // Z
        new Point[] { new Point(0,0), new Point(1,0), new Point(1,1), new Point(2,1) }, // S
        new Point[] { new Point(0,0), new Point(1,0), new Point(2,0), new Point(2,1) }, // L
        new Point[] { new Point(0,1), new Point(1,1), new Point(2,1), new Point(2,0) }, // J
        };

        private static readonly Color[] BlockColors = new Color[]
        {
            Color.FromArgb(222,155, 68), Color.FromArgb(188,137,167), Color.FromArgb(46,141,55),
            Color.FromArgb(194, 54,22), Color.FromArgb(70, 197, 214), Color.FromArgb(124, 89, 215),
            Color.FromArgb(91, 218, 86)
        };

        public Block(int shapeIndex)
        {
            Cells = (Point[])BlockShapes[shapeIndex].Clone();
            Color = BlockColors[shapeIndex];
            rotationIndex = 0;
        }
        public void Rotate()
        {
            if (Cells == BlockShapes[1]) return; // O block does not rotate
            //Calculate center of block
            int centerX = 0;
            int centerY = 0;

            if (Cells == BlockShapes[2]) // Specaial case for T block
            {
                centerX = 1;
                centerY = 0;
            }
            else
            {
                foreach (var cell in Cells)
                {
                    centerX += cell.X;
                    centerX += cell.Y;
                }
                centerX /= Cells.Length;
                centerY /= Cells.Length;
            }

            for (int i = 0; i < Cells.Length; i++)
            {
                // Translate to origin
                int x = Cells[i].X - centerX;
                int y = Cells[i].Y - centerY;

                // Rotate around origin
                int temp = x;
                x = -y;
                y = temp;
                // Translate back
                Cells[i].X = x + centerX;
                Cells[i].Y = y + centerY;

            }
            rotationIndex = (rotationIndex + 1) % 4;
        }
    }
}

