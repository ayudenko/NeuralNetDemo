using System.Collections.Generic;

namespace NeuralNetDemo.Client.Entities
{
    public class LinesComparer : IComparer<Divider>
    {
        public int Compare(Divider x, Divider y)
        {
            return y.GetMultiplier().CompareTo(x.GetMultiplier());
        }
    }
}
