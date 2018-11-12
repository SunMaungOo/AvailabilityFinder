namespace AvailabilityFinder
{
    public sealed class Range
    {
        public int Start { get; private set; }

        public int End { get; private set; }

        public Range(int start, int end)
        {
            Start = start;

            End = end;
        }

        /// <summary>
        /// Check whether the current range is overlap
        /// </summary>
        /// <param name="previousRange"></param>
        /// <returns></returns>
        public bool IsOverlap(Range range)
        {
            /*
              In the sorted array, if start time of an interval
              is less than end of previous interval, then there
              is an overlap
             */
            return range.End > Start;
        }

    }
}
