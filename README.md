# AvailabilityFinder

[![Build status](https://ci.appveyor.com/api/projects/status/06kcynwd3888l432/branch/master?svg=true)](https://ci.appveyor.com/project/SunMaungOo/availabilityfinder/branch/master)


Library to find the availability in the range

**Usage**

To find the available **range** which start at 0 and end at 100

```c#

//create a range we wanted to find the availability

Range limiting = new Range(0, 100);

//range which are not available

IList<Range> dataPoints = new List<Range>();
             dataPoints.Add(new Range(50, 99));
             dataPoints.Add(new Range(80, 99));
             dataPoints.Add(new Range(0, 30));

//return range which are available

Range[] result = Availability.FindAvailable(limiting, dataPoints.ToArray());

foreach (Range range in result)
{
   Console.WriteLine(string.Format("Range Start:{0},End:{1}",range.Start,range.End));
}

```
