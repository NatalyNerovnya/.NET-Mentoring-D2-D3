using System.Collections.Generic;
using System.Text;

namespace TestClasses
{
    public class Class1
    {
        public double Prop1 { get; set; }
        public string Prop2 { get; set; }
        public List<int> Prop3 { get; set; }
        
        public override string ToString()
        {
            var listValues = new StringBuilder();
            foreach (var value in Prop3)
            {
                listValues.Append($"{value} ");
            }
            return $"Prop1: {Prop1}, Prop2: {Prop2}, Prop3: {listValues}";
        }
    }
}
