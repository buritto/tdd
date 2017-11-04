using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace WindowsFormsApp1
{
    public class MyForm : Form
    {

        public Point center = new Point(350, 250);
        Font font1 = new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Point);

        public void DrawRectangle(Rectangle rec, Graphics graphics, int count)
        {
            graphics.DrawString(count.ToString(), font1, Brushes.Black,rec);
            graphics.DrawRectangle(Pens.Red, rec);
        }

        public void DrawSquareSpiral(int count)
        {
            var circle = new CircularCloudLayouter(center);
            var rectangleSize = new Size(50, 50);
            Graphics graphics = this.CreateGraphics();
            graphics.Clear(Color.WhiteSmoke);
            for (int i = 0; i < count; i++)
            {
                var rec = circle.PutNextRectangle(rectangleSize);
                graphics.DrawString(circle.CountTags.ToString(), font1, Brushes.Black, rec);
                graphics.DrawRectangle(Pens.Red, rec);
            }
        }

        public void DrawLargeIntersectionTest()
        {
            var circle = new CircularCloudLayouter(center);
            Graphics graphics = this.CreateGraphics();
            graphics.Clear(Color.WhiteSmoke);
            var sizeCenterRectangle = new Size(150, 150);
            var sizeLittleRectangleAroundCenter = new Size(50, 50);
            var sizeBigRectangle = new Size(300, 150);
            DrawRectangle(circle.PutNextRectangle(sizeCenterRectangle), graphics, circle.CountTags);
            for (int i = 0; i < 6; i++)
            {
                DrawRectangle(circle.PutNextRectangle(sizeLittleRectangleAroundCenter), graphics, circle.CountTags);
            }
            DrawRectangle(circle.PutNextRectangle(sizeBigRectangle), graphics, circle.CountTags);
        }


        public void DrawSpiralWithBigSquareInCenterAndLittleSquareAround()
        {
            var circle = new CircularCloudLayouter(center);
            var centerSqareSize = new Size(150, 150);
            Graphics graphics = this.CreateGraphics();
            graphics.Clear(Color.WhiteSmoke);
            DrawRectangle(circle.PutNextRectangle(centerSqareSize), graphics, circle.CountTags);
            var littleSqareSize = new Size(50, 50);
            for (int i = 0; i < 51; i++)
            {
                DrawRectangle(circle.PutNextRectangle(littleSqareSize), graphics, circle.CountTags);
            }

        }

        public void DrawDifferentRectangle()
        {
            var circle = new CircularCloudLayouter(center);
            var sizeRectangles = new List<Size>()
            {
                new Size(50, 50),
                new Size(100, 100),
                new Size(50, 50),
                new Size(150, 50),
                new Size(100, 150),
                new Size(50, 100),
                new Size(100, 50),
                new Size(150, 50),
                new Size(50, 100),
            };
            Graphics graphics = this.CreateGraphics();
            graphics.Clear(Color.WhiteSmoke);
            foreach (var sizeRectangle in sizeRectangles)
            {
                DrawRectangle(circle.PutNextRectangle(sizeRectangle), graphics, circle.CountTags);
            }

        }

        private CircularCloudLayouter cloudTag = new CircularCloudLayouter(new Point(400, 300));
        public void drawRectangle(int width, int height)
        {
            
            var size = new Size(width, height);
            var rec = cloudTag.PutNextRectangle(size);
            Graphics graphics = this.CreateGraphics();
            graphics.DrawRectangle(Pens.Red, rec);
            graphics.DrawString(cloudTag.CountTags.ToString(), font1, Brushes.Black, rec);

        }

        public MyForm(int width, int height)
        {
            this.Size = new Size(width, height);

            var textBoxWidh = new TextBox
            {
                Location = new Point(10, 10),
                Size = new Size(50, 200),
                Text = "50",
            };

            var textBoxHeight = new TextBox
            {
                Location = new Point(10, 30),
                Size = new Size(50, 200),
                Text = "50",
            };
            var buttonAdd = new Button
            {
                Location = new Point(10, 60),
                Size = new Size(50, 50),
                Text = "Add"
            };
            var buttonClear = new Button
            {
                Location = new Point(60, 60),
                Size = new Size(50, 50),
                Text = "Clear",
            };


            var buttonDrawSquareSpiral = new Button
            {
                Location = new Point(10, 120),
                Size = new Size(80, 60),
                Text = "DrawSquareSpiral ",
            };

            var buttonLargeIntersection = new Button
            {
                Location = new Point(10, 180),
                Size = new Size(80, 60),
                Text = "DrawLargeIntersectionTest",
            };

            var buttonhBigSquareInCenter = new Button
            {
                Location = new Point(10, 240),
                Size = new Size(80, 60),
                Text = "DrawBigSquareInCenter",
            };

            var buttonhDifferentRectangle = new Button
            {
                Location = new Point(10, 300),
                Size = new Size(80, 60),
                Text = "DrawDifferentRectangle",
            };




            buttonAdd.Click += (sender, args) => drawRectangle(int.Parse(textBoxWidh.Text),
                Int32.Parse(textBoxHeight.Text));
            buttonDrawSquareSpiral.Click += (sender, args) => DrawSquareSpiral(23);
            buttonLargeIntersection.Click += (sender, args) => DrawLargeIntersectionTest();
            buttonhBigSquareInCenter.Click += (sender, args) => DrawSpiralWithBigSquareInCenterAndLittleSquareAround();
            buttonhDifferentRectangle.Click += (sender, args) => DrawDifferentRectangle();
            buttonClear.Click += (sender, args) =>
            {
                Graphics graphics = this.CreateGraphics();
                graphics.Clear(Color.WhiteSmoke);
                cloudTag = new CircularCloudLayouter(center);

            };


            Controls.Add(buttonLargeIntersection);
            Controls.Add(textBoxHeight);
            Controls.Add(textBoxWidh);
            Controls.Add(buttonAdd);
            Controls.Add(buttonDrawSquareSpiral);
            Controls.Add(buttonhBigSquareInCenter);
            Controls.Add(buttonhDifferentRectangle);
            Controls.Add(buttonClear);
        }
    }

    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new MyForm(800, 600));
        }
    }
}
