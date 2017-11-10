﻿using System;
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

        public void DrawRectangle(Rectangle rec, Graphics graphics, int count)
        {
            graphics.DrawRectangle(Pens.Red, rec);
        }   

        private Random rnd = new Random();
        private CircularCloudLayouter cloudTag;
        private int cl;
        public void drawRectangle(int width, int height)
        {
            cl++;
            var size = new Size(width, height);
            var rec = cloudTag.PutNextRectangle(size);
            Graphics graphics = this.CreateGraphics();
            graphics.DrawRectangle(Pens.Red, rec);
        }

        public MyForm(int width, int height)
        {
            this.Size = new Size(width, height);
            cloudTag = new CircularCloudLayouter(new Point(350, 250), 600, 400);
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

            var buttonAddRandom = new Button
            {
                Location = new Point(10, 120),
                Size = new Size(100, 100),
                Text = "AddRandom"
            };
            var buttonClear = new Button
            {
                Location = new Point(60, 60),
                Size = new Size(50, 50),
                Text = "Clear",
            };

            buttonAdd.Click += (sender, args) => drawRectangle(int.Parse(textBoxWidh.Text),
                Int32.Parse(textBoxHeight.Text));

            buttonAddRandom.Click += (sender, args) => drawRectangle(rnd.Next(2, 100), rnd.Next(2, 100));
            buttonClear.Click += (sender, args) =>
            {
                Graphics graphics = this.CreateGraphics();
                graphics.Clear(Color.WhiteSmoke);
                cloudTag = new CircularCloudLayouter(center, this.Size.Width, this.Size.Height);

            };

            Controls.Add(buttonAddRandom);
            Controls.Add(textBoxHeight);
            Controls.Add(textBoxWidh);
            Controls.Add(buttonAdd);

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
