using System.Collections.Generic;
using System.Linq;

namespace AvailabilityFinder
{
    public static class Availability
    {
        /// <summary>
        /// Return all range in the limiting which is free
        /// </summary>
        /// <param name="limiting">The start and end of the total duration we wanted to find</param>
        /// <param name="dataPoints">The ranges which are not availably</param>
        /// <returns>All range in limiting which is free or empty array</returns>    
        public static Range[] FindAvailable(Range limiting, Range[] dataPoints)
        {
            IList<Range> freeRanges = new List<Range>();

            //find the range which is in range we wanted to find

            IList<Range> inDate = IsInRange(limiting, dataPoints);

            if (inDate.Count() > 1)
            {
                IList<Range> orderRange = SortData(inDate);

                int currentIndex = 0;

                int tmpStart = limiting.Start;

                bool hasNextItem = HasNextItem(currentIndex + 1, orderRange.Count());

                while (hasNextItem)
                {
                    int lastIndex = 0;

                    //get the first start value which have become available
                     
                    tmpStart = GetStartRange(tmpStart, ref currentIndex,
                        orderRange, out lastIndex);

                    hasNextItem = HasNextItem(lastIndex + 1, orderRange.Count());

                    if (hasNextItem)
                    {
                        ++lastIndex;

                        Range nextRange = orderRange[lastIndex + 1];

                        Range freeRange = new Range(tmpStart, nextRange.Start - 1);

                        freeRanges.Add(freeRange);

                        tmpStart = nextRange.Start;

                        hasNextItem = HasNextItem(lastIndex + 1, orderRange.Count());

                    }
                    else
                    {
                        Range freeRange = new Range(tmpStart, limiting.End);

                        freeRanges.Add(freeRange);

                    }



                }

            }
            else if (inDate.Count() == 1)
            {
                Range range = inDate[0];

                //if they don't have the same starting value

                if (limiting.Start != range.Start)
                {
                    Range freeRange = new Range(limiting.Start, range.Start - 1);

                    freeRanges.Add(freeRange);
                }

                //if range doesn't end by taking all of the "limiting" end value
                //we need to add another range

                if (limiting.End != range.End)
                {
                    Range freeRange = new Range(range.End + 1, limiting.End);

                    freeRanges.Add(freeRange);

                }

            }

            return freeRanges.ToArray();
        }


        /// <summary>
        /// Calcuate the start available range
        /// </summary>
        /// <param name="limitingStart">The start value of the limiting</param>
        /// <param name="currentIndex">Current index to start searching in the sorted list</param>
        /// <param name="orderRange">The list sorted by Start and End</param>
        /// <param name="lastUsedIndex">The last index which the overlap range is found
        /// when calcuating start value</param>
        /// <returns>The start value of the free range</returns>
        public static int GetStartRange(int limitingStart, ref int currentIndex,
            IList<Range> orderRange, out int lastUsedIndex)
        {
            lastUsedIndex = currentIndex;

            int start = limitingStart;

            //if the current item is not the last item

            if (HasNextItem(currentIndex, orderRange.Count()))
            {
                //get the current item

                Range current = orderRange[currentIndex];

                Range next = null;

                // if the start of the limiting range is not available

                if (start == current.Start)
                {

                    start = current.End + 1;

                    bool hasNextItem = HasNextItem(currentIndex + 1, orderRange.Count());

                    while (hasNextItem)
                    {
                        ++currentIndex;

                        //get the next item in the list

                        next = orderRange[currentIndex];

                        //if the current item overlap with the next item

                        if (next.IsOverlap(current))
                        {                            
                            start = next.End + 1;

                            current = next;

                            lastUsedIndex = currentIndex;
                        }

                        //check whether there is more item

                        hasNextItem = HasNextItem(currentIndex + 1, orderRange.Count());

                    }

                }

            }

            return start;
        }

        /// <summary>
        /// Find how many range are in the range we wanted to find availability
        /// </summary>
        /// <param name="limiting">The start and end of the total range we wanted to find</param>
        /// <param name="dataPoints"></param>
        /// <returns></returns>
        public static int Count(Range limiting, Range[] dataPoints)
        {
            return IsInRange(limiting, dataPoints).Count();
        }

        /// <summary>
        /// Return whether there is more item 
        /// </summary>
        /// <param name="currentIndex">Current index is the collection</param>
        /// <param name="size">Total size of the collection</param>
        /// <returns></returns>
        private static bool HasNextItem(int currentIndex, int size)
        {
            return currentIndex < size;
        }

        /// <summary>
        /// Return all the range which is in limiting range.
        /// </summary>
        /// <param name="limiting"></param>
        /// <param name="dataPoints"></param>
        /// <returns>All the range which is in limiting range or empty collection</returns>
        private static IList<Range> IsInRange(Range limiting, Range[] dataPoints)
        {
            IList<Range> inDate = new List<Range>();

            foreach (Range range in dataPoints)
            {
                if (range.Start >= limiting.Start &&
                    range.End <= limiting.End)
                {
                    inDate.Add(range);
                }
            }

            return inDate;
        }

        /// <summary>
        /// Sort the range
        /// </summary>
        /// <param name="data">not null list</param>
        /// <returns>Sorted list</returns>
        private static IList<Range> SortData(IList<Range> data)
        {
            return (from x in data
                    orderby x.Start ascending, x.End ascending
                    select x).ToList();
        }
    }
}
