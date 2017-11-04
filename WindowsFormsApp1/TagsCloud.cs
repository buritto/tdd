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
        public Point CenterTagsCload;
        public int CountTags;
        private Point center;
        private int countTagsBeforeTurning ;
        private int countTurn;
        private List<Rectangle> allBuildRectangles;
        public Point DirectionSpiral = WindowsFormsApp1.Direction.None;
        

        public CircularCloudLayouter(Point center)
        {
            CenterTagsCload = center;
            this.center = new Point(center.X, center.Y);
            allBuildRectangles = new List<Rectangle>();
        }

        private void CountingTagsBeforeTurning()
        {
            countTagsBeforeTurning = countTurn / 2 ;
        }

        private void ChangeDirection()
        {
            if (DirectionSpiral == Direction.None)
            {
                DirectionSpiral = Direction.Left;
                return;
            }
            if (DirectionSpiral == Direction.Left)
            {
                DirectionSpiral = Direction.Down;
                return;
            }
            if (DirectionSpiral == Direction.Down)
            {
                DirectionSpiral = Direction.Rigth;
                return;
            }

            if (DirectionSpiral == Direction.Rigth)
            {
                DirectionSpiral = Direction.Top;
                return;
            }
            DirectionSpiral = Direction.Left;
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

        private void MoveRectangle(ref Rectangle newRectangle, Rectangle oldRectangle)
        {
            oldRectangle.Intersect(newRectangle);
            if (DirectionSpiral == Direction.Down)
            {
                newRectangle.X = newRectangle.X - oldRectangle.Width;
                return;
            }
            if (DirectionSpiral == Direction.Rigth)
            {
                newRectangle.Y = newRectangle.Y + oldRectangle.Height;
                return;
            }

            if (DirectionSpiral == Direction.Top)
            {
                newRectangle.X = newRectangle.X + oldRectangle.Width; 
                return;
            }

            if (DirectionSpiral == Direction.Left)
            {
                newRectangle.Y = newRectangle.Y - oldRectangle.Height;
            }
        }


        private void DeleteIntersection(ref Rectangle rectangle)
        {
            foreach (var buildRectangle in allBuildRectangles)
            {
                if (buildRectangle.IntersectsWith(rectangle))
                {
                    MoveRectangle(ref rectangle, buildRectangle);
                    DeleteIntersection(ref rectangle);
                }
            }
        }

        private Rectangle BuildRectangle(Size rectangleSize)
        {
            if (allBuildRectangles.Count == 0)
            {
                return new Rectangle(center, rectangleSize);
            }
            var lastRectangle = allBuildRectangles.Last();
            var deltaX = rectangleSize.Width;
            var deltaY = lastRectangle.Height;
            if (DirectionSpiral == Direction.Rigth)
            {
                deltaX = lastRectangle.Width;
            }
            if (DirectionSpiral == Direction.Top)
            {
                deltaY = rectangleSize.Height;
            }

            var locationNewRectangle = new Point()
            {
                X = deltaX * DirectionSpiral.X + lastRectangle.X,
                Y = deltaY * DirectionSpiral.Y + lastRectangle.Y
            };
            var result = new Rectangle(locationNewRectangle, rectangleSize);
            DeleteIntersection(ref result);
            return result;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var result = BuildRectangle(rectangleSize);
            allBuildRectangles.Add(result);
            AnalyzeLayout();
            CountTags++;
            return result;
        }
    }

    [TestFixture]
    class TestTagsCload
    {
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

            Assert.AreEqual(new Rectangle(new Point(9, 11), rectangleSize),
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

            Assert.AreEqual(new Rectangle(new Point(10, 11), rectangleSize),
                circle.PutNextRectangle(rectangleSize));

            Assert.AreEqual(new Rectangle(new Point(11, 11), rectangleSize),
                circle.PutNextRectangle(rectangleSize));

        }

        [Test]
        public void BuildFullSpiralWithSquareTag()
        {
            var expectedResult = new List<Point>()
            {
                new Point(10, 10),
                new Point(9, 10),
                new Point(9, 11),
                new Point(10, 11),
                new Point(11, 11),
                new Point(11, 10),
                new Point(11, 9),
                new Point(10, 9),
                new Point(9, 9),
                new Point(8, 9)
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
                new Point(9, 11),
                new Point(10, 13),
                new Point(11, 13),
                new Point(13, 12),
                new Point(13, 11),
                new Point(12, 9),
                new Point(11, 9),
                new Point(10, 9)
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
        public void BuildThreeDiferentRectangle()
        {
            var pointCenter = new Point(10, 10);
            var circle = new CircularCloudLayouter(pointCenter);
            var firstFectangeSize = new Size(1, 1);
            var secondFectangeSize = new Size(2, 2);
            var thirdFectangeSize = new Size(2, 1);

            circle.PutNextRectangle(firstFectangeSize);
            circle.PutNextRectangle(secondFectangeSize);

            Assert.AreEqual(new Rectangle(new Point(8, 12), thirdFectangeSize),
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
            Assert.AreEqual(new Rectangle(new Point(9, 12), r4),
                circle.PutNextRectangle(r4));
            Assert.AreEqual(new Rectangle(new Point(12, 12), r5),
                circle.PutNextRectangle(r5));
            Assert.AreEqual(new Rectangle(new Point(12, 10), r6),
                circle.PutNextRectangle(r6));
            Assert.AreEqual(new Rectangle(new Point(12, 9), r7),
                circle.PutNextRectangle(r7));
            Assert.AreEqual(new Rectangle(new Point(9, 8), r8),
                circle.PutNextRectangle(r8));
            Assert.AreEqual(new Rectangle(new Point(8, 8), r9),
                circle.PutNextRectangle(r9));
        }

        [Test]
        public void LargeIntersection()
        {
            var pointCenter = new Point(10, 10);
            var circle = new CircularCloudLayouter(pointCenter);
            var sizeCenterRectangle = new Size(3,3);
            var sizeLittleRectangleAroundCenter = new Size(1, 1);
            var sizeBigRectangle = new Size(6, 3);
            circle.PutNextRectangle(sizeCenterRectangle);
            for (int i = 0; i < 6; i++)
            {
                circle.PutNextRectangle(sizeLittleRectangleAroundCenter);
            }
            Assert.AreEqual(new Rectangle(new Point(7,7), sizeBigRectangle ),
                circle.PutNextRectangle(sizeBigRectangle));
        }
    }
}
