using System.Collections.Generic;
using System.Text;

namespace TestClasses
{
    public class Class3
    {
        public int Prop1 { get; set; }
        public string Prop2 { get; set; }
        public List<double> Prop3 { get; set; }
        public double Prop4 { get; set; }

        public override string ToString()
        {
            var listValues = new StringBuilder();
            foreach (var value in Prop3)
            {
                listValues.Append($"{value} ");
            }
            return $"Prop1: {Prop1}, Prop2: {Prop2}, Prop3: {listValues}, Prop4: {Prop4}";
        }

    }
}
