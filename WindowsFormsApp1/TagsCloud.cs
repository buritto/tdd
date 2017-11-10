using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace WindowsFormsApp1
{

    [TestFixture]
    public class TestClass
    {
        public Point centerWindow = new Point(50, 50);


        [Test]
        public void BuildRectangleInCenter()
        {
            var center = new Point(500, 500);
            var size = new Size(123, 321);
            var c = new CircularCloudLayouter(center, 1000, 1000);

            var actualRectangle = c.PutNextRectangle(size);

            var expectedRectangle = new Rectangle(
                new Point(center.X - size.Width / 2, center.Y - size.Height / 2), size);

            actualRectangle.Should().Be(expectedRectangle);

        }

        [Test]
        public void NotIntersection()
        {
            var center = new Point(500, 500);
            var c = new CircularCloudLayouter(center, 1000, 1000);

            var rectangles = new List<Rectangle>();

            var differentSizes = new List<Size>()
            {
                new Size(123, 321),
                new Size(435, 432),
                new Size(1, 1),
                new Size(22, 33),
                new Size(100, 100),
                new Size(1, 400),
                new Size(400, 1)
            };

            foreach (var size in differentSizes)
            {
                rectangles.Add(c.PutNextRectangle(size));
            }

            foreach (var rec in rectangles)
            {
                Assert.IsFalse(rectangles.Any(rec1 => rec1.IntersectsWith(rec) && rec1 != rec));
            }
        }

        [Test]
        public void BuildBigRectangleInLittleWindow()
        {
            var center = new Point(5, 5);
            var c = new CircularCloudLayouter(center, 10, 20);

            Assert.That(() => c.PutNextRectangle(new Size(100500, 500100)), Throws.Exception);


        }

        [Test]
        public void BuildSqares()
        {
            var sideSqare = 2;

            var c = new CircularCloudLayouter(new Point(5, 5), 10, 10 );

            var rectangleSize = new Size(sideSqare, sideSqare);
            var expectedSqares = new List<Rectangle>()
            {
                new Rectangle(new Point(4, 4), rectangleSize),
                new Rectangle(new Point(2, 5), rectangleSize),
                new Rectangle(new Point(0, 4), rectangleSize),
                new Rectangle(new Point(0, 2), rectangleSize),
                new Rectangle(new Point(1, 0), rectangleSize),
                new Rectangle(new Point(7, 0), rectangleSize)
            };

            var actualSqares = new List<Rectangle>();

            for (int i = 0; i < 6; i++)
            {
                actualSqares.Add(c.PutNextRectangle(rectangleSize));
            }
            actualSqares.Should().Equal(expectedSqares);
        }

        [Test]
        public void BuildDifferentRenctangles()
        {
            var c = new CircularCloudLayouter(new Point(5, 5), 10, 10);
            var sizeRectangles = new List<Size>()
            {
                new Size(3, 2),
                new Size(2, 4),
                new Size(1, 1),
                new Size(2, 2), 
                new Size(4, 1)

            };

            var expectedRectangles = new List<Rectangle>()
            {
                new Rectangle(new Point(4, 4),sizeRectangles[0] ),
                new Rectangle(new Point(2, 4), sizeRectangles[1]),
                new Rectangle(new Point(5, 6), sizeRectangles[2]),
                new Rectangle(new Point(0, 4), sizeRectangles[3]),
                new Rectangle(new Point(0, 2), sizeRectangles[4])

            };

            var actualRectangles = new List<Rectangle>();
            foreach (var size in sizeRectangles)
            {
                actualRectangles.Add(c.PutNextRectangle(size));
            }
            actualRectangles.Should().Equal(expectedRectangles);
        }
    }

    public class CircularCloudLayouter
    {

        private readonly List<Rectangle> builtRectangles;
        private List<Point> spiralPoints;
        private Rectangle window;

        private void PutPointsOnSpiral(Point centerWindow)
        {
            spiralPoints = new List<Point>();
            var step = 0.0;
            while (true)
            {
                var x = (int)(step * Math.Cos(step) + centerWindow.X);
                var y = (int)(step * Math.Sin(step) + centerWindow.Y);

                if (SpiralGoingOutWindow(x, y))
                {
                    spiralPoints = spiralPoints.Distinct().ToList();
                    break;
                }
                step += 0.1;
                spiralPoints.Add(new Point(x, y));
            }
        }

        private bool SpiralGoingOutWindow(int x, int y)
        {
            return x < 0 && y < 0;
        }

        public CircularCloudLayouter(Point centerWindow, int widthWindow, int heightWindow)
        {
            builtRectangles = new List<Rectangle>();
            PutPointsOnSpiral(centerWindow);
            window = new Rectangle(new Point(0, 0), new Size(widthWindow, heightWindow) );
        }


        public Rectangle PutNextRectangle(Size rectangleSize)
        {

            foreach (var spiralPoint in spiralPoints)
            {
                var location = CalculateLocationRectangle(rectangleSize, spiralPoint);
                var rectangle = new Rectangle(location, rectangleSize);
                if (IsCorrectLocation(rectangle))
                {
                    builtRectangles.Add(rectangle);
                    return rectangle;
                }
            }
            throw  new Exception("Cloud is full");
        }


        private Point CalculateLocationRectangle(Size rectangleSize, Point spiralPoint)
        {
           return new Point()
           {
               X = spiralPoint.X - rectangleSize.Width / 2,
               Y = spiralPoint.Y - rectangleSize.Height / 2
           };
        }

        private bool IsCorrectLocation(Rectangle rectangle)
        {
            var notIntersection = !builtRectangles.Any(rec => rec.IntersectsWith(rectangle));

            var outside = rectangle.X >= 0 && rectangle.Right <= window.Width
                          && rectangle.Y >= 0 && rectangle.Bottom <= window.Height;
            return notIntersection && outside;
        }

    }
}
