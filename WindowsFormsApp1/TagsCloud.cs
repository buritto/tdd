using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace WindowsFormsApp1
{

    [TestFixture]
    public class TestClass
    {
        public Point center = new Point(100, 100);


        [Test]
        public void BuildRectangleInCenter()
        {
            var c = new CircularCloudLayouter(center);
            var sizeRectangle = new Size(10, 10);

            var actualRectangle = c.PutNextRectangle(sizeRectangle);
            var expceptedRectangle = new Rectangle(new Point(center.X + 5, center.Y + 5), sizeRectangle);

            Assert.AreEqual(expceptedRectangle, actualRectangle);
        }


        public Rectangle BuildRectangle(Size recSize, double angle)
        {
            var x = angle * Math.Cos(angle) + center.X + recSize.Width / 2;
            var y = angle * Math.Sin(angle) + center.Y + recSize.Height / 2;
            return new Rectangle(new Point((int) x, (int) y), recSize);
        }

        [TestCase(10, 10, 20)]
        [TestCase(1, 1, 40)]
        [TestCase(10, 5, 20)]
        [TestCase(5 , 10, 20)]
        public void BuildSameRectangle(int width,int height , int countRectangle)
        {
            var c = new CircularCloudLayouter(center);
            var sizeRectangle = new Size(width, height);
            var setExpectedRectangle = new List<Rectangle>();
            var step = 0.1;

            for (var angle = 0.0; angle < countRectangle * countRectangle; angle += step)
            {

                setExpectedRectangle.Add(BuildRectangle(sizeRectangle, angle));
            }

            var setActualRectangle = new List<Rectangle>();

            for (int i = 0; i < countRectangle; i++)
            {
                setActualRectangle.Add(c.PutNextRectangle(sizeRectangle));
            }

            setActualRectangle.Distinct().Should().BeSubsetOf(setExpectedRectangle.Distinct());
            setActualRectangle.Distinct().Count().Should().Be(countRectangle);
        }

        [TestCase(100, 5)]
        [TestCase(100, 25)]
        [TestCase(100, 1)]
        [TestCase(100, 50)]
        [TestCase(100, 80)]
        public void BuildDifferentRectangle(int countRectangle, int countDifferentRepresentative)
        {
            var setExpectedRectangles = new List<Rectangle>();
            var setActualRectangles = new List<Rectangle>();
            var c = new CircularCloudLayouter(center);

            var rnd = new Random();
            for (int representative = 0; representative < countDifferentRepresentative; representative++)
            {
                var width  = rnd.Next(10, 100);
                var height = rnd.Next(10, 100);
                for (int i = 0; i < countRectangle / countDifferentRepresentative; i++)
                {
                    setActualRectangles.Add(c.PutNextRectangle(new Size(width, height)));
                }

                var step = 0.1;

                for (var angle = 0.0; angle < countRectangle * countRectangle; angle += step)
                {

                   setExpectedRectangles.Add(BuildRectangle(new Size(width, height), angle));
                }

            }
            setActualRectangles.Distinct().Should().BeSubsetOf(setExpectedRectangles.Distinct());
            setActualRectangles.Distinct().Count().Should().Be(countRectangle/ countDifferentRepresentative * countDifferentRepresentative);

        }

    }

    public class CircularCloudLayouter
    {
        public Point centerCloud;

        public List<Rectangle> BuiltRectangles;

        private double step;

        public CircularCloudLayouter(Point center)
        {
            centerCloud = center;
            BuiltRectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {

            while (true)
            {
                var rectangle = new Rectangle(CalculateCenterNewRectangle(rectangleSize), rectangleSize);
                if (!BuiltRectangles.Any(rec => rec.IntersectsWith(rectangle)))
                {
                    BuiltRectangles.Add(rectangle);
                    break;
                }
            }
            return BuiltRectangles.Last();
        }

        private Point CalculateCenterNewRectangle(Size rectangleSize)
        {
            step += 0.1;
            var x = step * Math.Cos(step) + centerCloud.X + rectangleSize.Width / 2;
            var y = step * Math.Sin(step) + centerCloud.Y + rectangleSize.Height / 2;
            return new Point((int) x, (int) y);

        }

    }
}
