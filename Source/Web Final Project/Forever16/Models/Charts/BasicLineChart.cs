using System.Collections.Generic;

namespace Forever16.Models.Charts
{
    public class BasicLine<T> : Base<T>
    {
        public int PointStart { get; set; }
    }

    public class BasicBar<T> : Base<T>
    {
        public string[] Categories { get; set; }
    }

    public class Basic3DBar<T>
    {
        public List<Serie3D<T>> Series { get; set; }
        public string[] Categories { get; set; }

        public Basic3DBar()
        {
            Series = new List<Serie3D<T>>();
        }
    }

    public class PieChart
    {
        public Data[] Data { get; set; }
    }
}