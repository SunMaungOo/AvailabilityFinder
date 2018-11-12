using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AvailabilityFinder.Tests
{
    [TestClass()]
    public class AvailabilityTests
    {
        [TestMethod()]
        public void GetStartRangeTest()
        {
            Range limiting = new Range(0, 100);

            int currentIndex = 0;

            IList<Range> dataPoint = new List<Range>();
            dataPoint.Add(new Range(0, 10));
            dataPoint.Add(new Range(0, 11));

            IList<Range> orderStartTime = (from x in dataPoint
                                           orderby x.Start ascending, x.End ascending
                                           select x).ToList();

            int lastIndex = 0;

            int startTime = Availability.GetStartRange(limiting.Start, ref currentIndex,
                orderStartTime, out lastIndex);


            Assert.AreEqual(12, startTime);
        }

        [TestMethod()]
        public void GetStartRangeTest2()
        {
            Range limiting = new Range(0, 100);

            int currentIndex = 0;

            IList<Range> dataPoint = new List<Range>();
            dataPoint.Add(new Range(0, 11));
            dataPoint.Add(new Range(1, 19));

            IList<Range> orderStartTime = (from x in dataPoint
                                           orderby x.Start ascending, x.End ascending
                                           select x).ToList();

            int lastIndex = 0;

            int startTime = Availability.GetStartRange(limiting.Start, ref currentIndex,
                       orderStartTime, out lastIndex);

            Assert.AreEqual(20, startTime);
        }

        [TestMethod()]
        public void GetStartRangeTest3()
        {
            Range limiting = new Range(0, 100);

            int currentIndex = 0;

            IList<Range> dataPoint = new List<Range>();
            dataPoint.Add(new Range(0, 11));
            dataPoint.Add(new Range(1, 19));
            dataPoint.Add(new Range(1, 19));
            dataPoint.Add(new Range(1, 31));
            dataPoint.Add(new Range(1, 54));
            dataPoint.Add(new Range(1, 23));


            IList<Range> orderStartTime = (from x in dataPoint
                                           orderby x.Start ascending, x.End ascending
                                           select x).ToList();

            int lastIndex = 0;

            int startTime = Availability.GetStartRange(limiting.Start, ref currentIndex,
                              orderStartTime, out lastIndex);

            Assert.AreEqual(55, startTime);
        }

        [TestMethod()]
        public void GetStartRangeTest4()
        {
            Range limiting = new Range(0, 100);

            IList<Range> dataPoint = new List<Range>();
            dataPoint.Add(new Range(50, 99));
            dataPoint.Add(new Range(80, 99));
            dataPoint.Add(new Range(0, 30));

            IList<Range> orderStartTime = (from x in dataPoint
                                           orderby x.Start ascending, x.End ascending
                                           select x).ToList();

            int currentIndex = 0;

            int lastIndex = 0;

            int startTime = Availability.GetStartRange(limiting.Start, ref currentIndex,
                                   orderStartTime, out lastIndex);

            Assert.AreEqual(31, startTime);
        }

        [TestMethod()]
        public void IsOverlapTest()
        {
            IList<Range> dataPoint = new List<Range>();

            dataPoint.Add(new Range(1, 15));
            dataPoint.Add(new Range(0, 30));


            IList<Range> orderStartTime = (from x in dataPoint
                                           orderby x.Start ascending, x.End ascending
                                           select x).ToList();


            bool result = dataPoint[1].IsOverlap(dataPoint[0]);

            Assert.IsTrue(result);
        }


        [TestMethod()]
        public void FindAvailableTest()
        {
            Range limiting = new Range(0, 100);

            IList<Range> dataPoint = new List<Range>();
            dataPoint.Add(new Range(50, 99));
            dataPoint.Add(new Range(80, 99));
            dataPoint.Add(new Range(0, 30));

            Range[] result = Availability.FindAvailable(limiting, dataPoint.ToArray());

            Assert.IsTrue(result.Length == 2, "The result length is not 2");

            Range firstRange = result[0];

            Assert.IsTrue(firstRange.Start == 31 && firstRange.End == 79);

            Range secondRange = result[1];

            Assert.IsTrue(secondRange.Start == 100 && secondRange.End == 100);

        }

        [TestMethod()]
        public void FindAvailableTest2()
        {
            Range limiting = new Range(0, 100);

            IList<Range> dataPoint = new List<Range>();
            dataPoint.Add(new Range(50, 97));
            dataPoint.Add(new Range(80, 97));
            dataPoint.Add(new Range(0, 30));

            Range[] result = Availability.FindAvailable(limiting, dataPoint.ToArray());

            Assert.IsTrue(result.Length == 2, "The result length is not 2");

            Range firstRange = result[0];

            Assert.IsTrue(firstRange.Start == 31 && firstRange.End == 79);

            Range secondRange = result[1];

            Assert.IsTrue(secondRange.Start == 98 && secondRange.End == 100);

        }

        [TestMethod()]
        public void FindAvailableTest3()
        {
            Range limiting = new Range(0, 100);

            Range[] dataPoint = new Range[]
            {
                new Range(0,20)
            };

            Range[] result = Availability.FindAvailable(limiting, dataPoint);

            Assert.IsTrue(result.Length == 1, "The length is not 1");

            Assert.IsTrue(result[0].Start == 21 && result[0].End == 100);
        }


        [TestMethod()]
        public void FindAvailableTest4()
        {
            Range limiting = new Range(0, 100);

            Range[] dataPoint = new Range[]
            {
                new Range(10,20)
            };

            Range[] result = Availability.FindAvailable(limiting, dataPoint);

            Assert.IsTrue(result.Length == 2, "The length is not 2");

            Range firstRange = result[0];

            Assert.IsTrue(firstRange.Start == 0 && firstRange.End == 9, "First result error");

            Range secondRange = result[1];

            Assert.IsTrue(secondRange.Start == 21 && secondRange.End == 100, "Second result error");

        }
    }
}