﻿
namespace syncfusion.pivotgriddemos.wpf
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Linq;

    public enum Shape
    {
        Point,
        Line,
        Triangle,
        Square,
        Rectangle,
        Trapezoid,
        Pentagon
    }

    public class BusinessObjectsDataView : DataView
    {
        public static DataView GetDataTable(int count)
        {
            DataTable dt = new DataTable("BusinessObjectsDataTable");
            PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(typeof(BusinessObject));
            foreach (PropertyDescriptor pd in pdc)
            {
                dt.Columns.Add(new DataColumn(pd.Name, pd.PropertyType));
            }
            BusinessObjectCollection list = BusinessObjectCollection.GetList(count);
            foreach (BusinessObject bo in list)
            {
                DataRow dr = dt.NewRow();
                foreach (PropertyDescriptor pd in pdc)
                {
                    dr[pd.Name] = pd.GetValue(bo);
                }
                dt.Rows.Add(dr);
            }
            return dt.DefaultView;
        }
    }

    public class BusinessObjectCollection : List<BusinessObject>
    {
        public static BusinessObjectCollection GetList(int count)
        {
            BusinessObjectCollection list = new BusinessObjectCollection();

            List<string> Fruits = new List<string>(new string[] { "cherry", "mango", "orange", "grape", "Persimmon", "plum", "fig", "apple", "lime", "gooseberry", "strawberry", "rasberry" });
            List<string> Colors = new List<string>(new string[] { "red", "green", "blue", "yellow", "orange", "pink", "crimson", "almond", "white", "black", "aqua", "beige" });

            Random r = new Random(123123);
            int shapeCount = Enum.GetNames(typeof(Shape)).Count();

            for (int i = 0; i < count; ++i)
            {
                BusinessObject bo = new BusinessObject()
                {
                    Fruit = Fruits[r.Next(Fruits.Count)],
                    Shape = (Shape)r.Next(shapeCount),
                    Color = Colors[r.Next(Colors.Count)],
                    Weight = ((int)(r.NextDouble() * 1000)) / 10d,
                    Even = r.Next(2) == 0,
                    Count = r.Next(4) + 1,
                    Section = r.Next(6)
                };
                list.Add(bo);
            }
            return list;
        }
    }

    public class BusinessObject
    {
        public string Fruit { get; set; }
        public string Color { get; set; }
        public Shape Shape { get; set; }
        public bool Even { get; set; }
        public int Section { get; set; }
        public double Weight { get; set; }
        public int Count { get; set; }

        public override string ToString()
        {
            return string.Format("Fruit=[{0}] Color=[{1}] Shape=[{2}] Even=[{3}] Section=[{4}] Weight=[{5:0.0}] Count=[{6}]", this.Fruit, this.Color, this.Shape, this.Even, this.Section, this.Weight, this.Count);
        }

    }

}
