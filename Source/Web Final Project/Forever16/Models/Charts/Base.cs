using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forever16.Models.Charts
{
    public class Base<T>
    {
        public List<Serie<T>> Series { get; set; }

        public Base()
        {
            this.Series = new List<Serie<T>>();
        }
    }

    public class Serie<T>
    {
        public string Name { get; set; }
        public T[] Data { get; set; }
    }

    public class Serie3D<T> : Serie<T>
    {
        public string Stack { get; set; }
    }

    public class Data
    {
        public string Name { get; set; }
        public decimal Y { get; set; }
    }

    public class ListItem
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }
}