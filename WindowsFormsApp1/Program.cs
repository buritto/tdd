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

        public Point center = new Point(400, 300);
        public void DrawBuildOneTegInCenter()
        {

            var circle = new CircularCloudLayouter(center);
            var rectangleSize = new Size(10, 8);
            var rec = circle.PutNextRectangle(rectangleSize);
            Graphics graphics = this.CreateGraphics();
            graphics.Clear(Color.White);
            graphics.DrawRectangle(Pens.Red, rec);
        }

        public void DrawSquareSpiralLeftDirection()
        {
            var circle = new CircularCloudLayouter(center);
            var rectangleSize = new Size(100, 100);
            Graphics graphics = this.CreateGraphics();
            graphics.Clear(Color.White);
            graphics.DrawRectangle(Pens.Red, circle.PutNextRectangle(rectangleSize));
            graphics.DrawRectangle(Pens.Red, circle.PutNextRectangle(rectangleSize));


        }

        public void DrawSquareSpiralLeftAndDownDirection()
        {
            var circle = new CircularCloudLayouter(center);
            var rectangleSize = new Size(100, 100);
            Graphics graphics = this.CreateGraphics();
            graphics.Clear(Color.White);
            for (int i = 0; i < 3; i++)
                graphics.DrawRectangle(Pens.Red, circle.PutNextRectangle(rectangleSize));
        }


        public CircularCloudLayouter cloudTag = new CircularCloudLayouter(new Point(400, 300));

        public void drawRectangle(int width, int height)
        {
            var size = new Size(width, height);
            var rec = cloudTag.PutNextRectangle(size);
            //rec.X = -rec.X;
            rec.Y = -rec.Y;

            Graphics graphics = this.CreateGraphics();

            Font font1 = new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Point);

            
            graphics.TranslateTransform(0, ClientSize.Height);
            var vertex = new Rectangle(rec.Location, new Size(10, 10));
            graphics.DrawRectangle(Pens.Red, rec);
            //graphics.DrawRectangle(Pens.Blue, vertex);
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
            var buttonBuildOneTegInCenter = new Button
            {
                Location = new Point(10, 120),
                Size = new Size(80, 60),
                Text = "BuildOneTegInCenter"
            };

            var buttonSquareSpiralLeftDirection = new Button
            {
                Location = new Point(10, 180),
                Size = new Size(80, 60),
                Text = "SquareSpiralLeftDirection"
            };
            var buttonSquareSpiralLeftAndDownDirection = new Button
            {
                Location = new Point(10, 240),
                Size = new Size(80, 60),
                Text = "SquareSpiralLeftAndDownDirection"
            };


            buttonSquareSpiralLeftDirection.Click += (IChannelSender, args) => DrawSquareSpiralLeftDirection();
            buttonBuildOneTegInCenter.Click += (sender, args) => DrawBuildOneTegInCenter();
            buttonSquareSpiralLeftAndDownDirection.Click += (sender, args) => DrawSquareSpiralLeftAndDownDirection();
            buttonAdd.Click += (sender, args) => drawRectangle(int.Parse(textBoxWidh.Text),
                Int32.Parse(textBoxHeight.Text));
            Controls.Add(textBoxHeight);
            Controls.Add(textBoxWidh);
            Controls.Add(buttonAdd);
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
