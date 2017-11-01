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
    public class CircularCloudLayouter
    {
        public Point centerTagsCload;
        public int CountTags;
        private Point bias;
        private int countTagsBeforeTurning ;
        private int countTurn;
        private Rectangle lastRectangle;
        private List<Point> blockedPointsList = new List<Point>();
        private List<Point> vertexList = new List<Point>();

        public Point direction = Direction.None;
        

        public CircularCloudLayouter(Point center)
        {
            centerTagsCload = center;
            lastRectangle = new Rectangle();
            bias = new Point(center.X, center.Y);
        }

        private void CountingTagsBeforeTurning()
        {
            countTagsBeforeTurning = countTurn / 2 ;
        }

        private void ChangeDirection()
        {
            if (direction == Direction.None)
            {
                direction = Direction.Left;
                return;
            }
            if (direction == Direction.Left)
            {
                direction = Direction.Down;
                return;
            }
            if (direction == Direction.Down)
            {
                direction = Direction.Rigth;
                return;
            }

            if (direction == Direction.Rigth)
            {
                direction = Direction.Top;
                return;
            }
            direction = Direction.Left;
        }

        private void AnalyzeLayout()
        {
            if (countTagsBeforeTurning == 0)
            {
                ChangeDirection();
                CountingTagsBeforeTurning();
                countTurn++;
                return;;
            }
            countTagsBeforeTurning--;
        }

        private void ClearMatchingVertices(Size rectangleSize)
        {
            if (vertexList.Contains(bias))
            {
                if (direction == Direction.Down)
                {
                    bias.X = bias.X - rectangleSize.Width;
                    return;
                }
                if (direction == Direction.Rigth)
                {
                    bias.Y = bias.Y - rectangleSize.Height;
                    return;
                }

                if (direction == Direction.Top)
                {
                    bias.X = bias.X + rectangleSize.Width;
                    return;
                }

                if (direction == Direction.Left)
                {
                    bias.Y = bias.Y + rectangleSize.Height;
                }
            }
            
        }

        private void ClearIntersection(Size rectangleSize)
        {

            var downRight = new Point(bias.X + rectangleSize.Width,
                bias.Y - rectangleSize.Height);
            var topRight = new Point(bias.X + rectangleSize.Width, bias.Y);
            var downLeft = new Point(bias.X, bias.Y - rectangleSize.Height);
            var topLeft = new Point(bias.X, bias.Y);



            if (blockedPointsList.Contains(topRight) &&
                blockedPointsList.Contains(downRight))
            {
                bias.Y--;
                ClearIntersection(rectangleSize);
                return;
            }

            if (blockedPointsList.Contains(topLeft) &&
                blockedPointsList.Contains(topRight))
            {
                bias.X++;
                ClearIntersection(rectangleSize);
                return;
            }

            if (blockedPointsList.Contains(topLeft) &&
                blockedPointsList.Contains(downLeft))
            {
                bias.Y++;
                ClearIntersection(rectangleSize);
                return;
            }

            if (blockedPointsList.Contains(downRight) &&
                blockedPointsList.Contains(downLeft))
            {
                bias.X--;
                ClearIntersection(rectangleSize);
                return;
            }

            if (blockedPointsList.Contains(topLeft))
            {
                bias.X++;
                ClearIntersection(rectangleSize);
                return;
            }


            if (blockedPointsList.Contains(topRight))
            {
                bias.Y--;
                ClearIntersection(rectangleSize);
                return;
            }

            if (blockedPointsList.Contains(downLeft))
            {
                bias.Y++;
                ClearIntersection(rectangleSize);
                return;
            }

            if (blockedPointsList.Contains(downRight))
            {
                bias.X--;
                ClearIntersection(rectangleSize);
            }

        }

        private void BlockPoint(Size rectangleSize)
        {
            for (int x = 1; x < rectangleSize.Height; x++)
            {
                for (int y = 1; y < rectangleSize.Width; y++)
                {
                    blockedPointsList.Add(new Point(bias.X + y, bias.Y - x));
                }
            }
            
        }

        private void CountBias (Size rectangleSize)
        {
            var deltaX = rectangleSize.Width;
            var deltaY = lastRectangle.Height;
            if (direction == Direction.Rigth)
            {
                deltaX = lastRectangle.Width;
            }

            if (direction == Direction.Top)
            {
                deltaY = rectangleSize.Height;
            }

            bias.X = deltaX * direction.X + bias.X;
            bias.Y = deltaY * direction.Y + bias.Y;
            ClearIntersection(rectangleSize);
            ClearMatchingVertices(rectangleSize);
        }

        

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            CountBias(rectangleSize);
            BlockPoint(rectangleSize);
            var result = new Rectangle(bias, rectangleSize);
            lastRectangle = result;
            vertexList.Add(result.Location);
            AnalyzeLayout();
            CountTags++;
            return result;
        }
    }

    [TestFixture]
    class TestTagsCload
    {

        //[Test]
        //public void Test()
        //{
        //    var cir = new CircularCloudLayouter(new Point(400, 300));
        //    var recSize = new Size(50, 50);
        //    var lastSize = new Size(50,55);
        //    cir.PutNextRectangle(recSize);
        //    cir.PutNextRectangle(recSize);
        //    Assert.AreEqual(new Rectangle(new Point(350, 250), lastSize),
        //        cir.PutNextRectangle(lastSize));

        //}
        [Test]
        public void BuildOneTegInCenter()
        {
            var pointCenter = new Point(5, 5);
            var circle = new CircularCloudLayouter(pointCenter);
            var rectangleSize = new Size(10, 8);
            var newTagsRectangle = circle.PutNextRectangle(rectangleSize);
            Assert.AreEqual(new Rectangle(new Point(5, 5), rectangleSize), newTagsRectangle);
        }

        [Test]
        public void BuildSquareSpiralLeftDirection()
        {
            var pointCenter = new Point(10, 10);
            var circle = new CircularCloudLayouter(pointCenter);
            var rectangleSize = new Size(1, 1);
            Assert.AreEqual(new Rectangle(new Point(10, 10), rectangleSize),
                circle.PutNextRectangle(rectangleSize));
            Assert.AreEqual(new Rectangle(new Point(9, 10), rectangleSize),
                circle.PutNextRectangle(rectangleSize));
        }

        [Test]
        public void BuildSquareSpiralLeftAndDownDirection()
        {
            var pointCenter = new Point(10, 10);
            var circle = new CircularCloudLayouter(pointCenter);
            var rectangleSize = new Size(1, 1);
            circle.PutNextRectangle(rectangleSize);
            circle.PutNextRectangle(rectangleSize);

            Assert.AreEqual(new Rectangle(new Point(9, 9), rectangleSize),
                circle.PutNextRectangle(rectangleSize));

        }

        [Test]
        public void BuildSquareSpiralLeftDownRight()
        {
            var pointCenter = new Point(10, 10);
            var circle = new CircularCloudLayouter(pointCenter);
            var rectangleSize = new Size(1, 1);
            circle.PutNextRectangle(rectangleSize);
            circle.PutNextRectangle(rectangleSize);
            circle.PutNextRectangle(rectangleSize);

            Assert.AreEqual(new Rectangle(new Point(10, 9), rectangleSize),
                circle.PutNextRectangle(rectangleSize));

            Assert.AreEqual(new Rectangle(new Point(11, 9), rectangleSize),
                circle.PutNextRectangle(rectangleSize));

        }

        [Test]
        public void BuildFullSpiralWithSquareTag()
        {
            var expectedResult = new List<Point>()
            {
                new Point(10, 10),
                new Point(9, 10),
                new Point(9, 9),
                new Point(10, 9),
                new Point(11, 9),
                new Point(11, 10),
                new Point(11, 11),
                new Point(10, 11),
                new Point(9, 11),
                new Point(8, 11)
            };
            var pointCenter = new Point(10, 10);
            var circle = new CircularCloudLayouter(pointCenter);
            var rectangeSize = new Size(1, 1);
            var actualList = new List<Point>();
            for (var i = 0; i < 10; i++)
            {
                var rectangle = circle.PutNextRectangle(rectangeSize);
                actualList.Add(new Point(rectangle.X, rectangle.Y));
            }
            actualList.Should().Equal(expectedResult);
        }

        [Test]
        public void BuildSpiralWithBigSquareInCenterAndLittleSquareAround()
        {

            var expectedResult = new List<Point>()
            {
                new Point(9, 10),
                new Point(9, 9),
                new Point(10, 7),
                new Point(11, 7),
                new Point(13, 8),
                new Point(13, 9),
                new Point(12, 11),
                new Point(11, 11),
                new Point(10, 11)
            };
            var pointCenter = new Point(10, 10);
            var circle = new CircularCloudLayouter(pointCenter);
            var centerSqareSize = new Size(3,3);
            circle.PutNextRectangle(centerSqareSize);
            var littleSqareSize = new Size(1,1);
            var actualList = new List<Point>();

            for (int i = 0; i < 9; i++)
            {
                actualList.Add(circle.PutNextRectangle(littleSqareSize).Location);
            }

            actualList.Should().Equal(expectedResult);
        }

        [Test]
        public void BuildBigSpiralWithBigSquareInCenterAndLittleSquareAround()
        {

            var expectedResult = new List<Point>()
            {
                new Point(8, 10),
                new Point(8, 9),
                new Point(8, 8),
                new Point(9, 8),
                new Point(10, 6),
                new Point(11, 6),
                new Point(12, 6),
                new Point(12, 7),
                new Point(14, 8),
                new Point(14, 9),
                new Point(14, 10),
                new Point(13, 10),
                new Point(12, 12),
                new Point(11, 12),
                new Point(10, 12),
                new Point(9, 12),
            };
            var pointCenter = new Point(10, 10);
            var circle = new CircularCloudLayouter(pointCenter);
            var centerSqareSize = new Size(3, 3);
            circle.PutNextRectangle(centerSqareSize);
            var littleSqareSize = new Size(1, 1);
            var actualList = new List<Point>();

            for (int i = 0; i < 9; i++)
            {
                circle.PutNextRectangle(littleSqareSize);
            }

            for (int i = 0; i < 16; i++)
            {
                actualList.Add(circle.PutNextRectangle(littleSqareSize).Location);
            }

            actualList.Should().Equal(expectedResult);
        }

        [Test]
        public void BuildThreeDiferentRectangle()
        {
            var pointCenter = new Point(10, 10);
            var circle = new CircularCloudLayouter(pointCenter);
            var firstFectangeSize = new Size(1, 1);
            var secondFectangeSize = new Size(2, 2);
            var thirdFectangeSize = new Size(2, 1);

            circle.PutNextRectangle(firstFectangeSize);
            circle.PutNextRectangle(secondFectangeSize);

            Assert.AreEqual(new Rectangle(new Point(8, 8), thirdFectangeSize),
                circle.PutNextRectangle(thirdFectangeSize));
        }

        [Test]
        public void BuildFullSpiralDiferentRectangle()
        {
            var pointCenter = new Point(10, 10);
            var circle = new CircularCloudLayouter(pointCenter);
            var r1 = new Size(1, 1);
            var r2 = new Size(2, 2);
            var r3 = new Size(1, 1);
            var r4 = new Size(3, 1);
            var r5 = new Size(2, 3);
            var r6 = new Size(1, 2);
            var r7 = new Size(2, 1);
            var r8 = new Size(3, 2);
            var r9 = new Size(1, 2);
            circle.PutNextRectangle(r1);
            circle.PutNextRectangle(r2);
            circle.PutNextRectangle(r3);
            Assert.AreEqual(new Rectangle(new Point(9, 8), r4),
                circle.PutNextRectangle(r4));
            Assert.AreEqual(new Rectangle(new Point(12, 8), r5),
                circle.PutNextRectangle(r5));
            Assert.AreEqual(new Rectangle(new Point(12, 10), r6),
                circle.PutNextRectangle(r6));
            Assert.AreEqual(new Rectangle(new Point(12, 11), r7),
                circle.PutNextRectangle(r7));
            Assert.AreEqual(new Rectangle(new Point(9, 12), r8),
                circle.PutNextRectangle(r8));
            Assert.AreEqual(new Rectangle(new Point(8, 12), r9),
                circle.PutNextRectangle(r9));
        }
    }
}
