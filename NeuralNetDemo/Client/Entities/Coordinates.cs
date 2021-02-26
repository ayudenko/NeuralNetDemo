namespace NeuralNetDemo.Client.Entities
{
    public class Coordinates
    {

        public double X { get; init; }
        public double Y { get; init; }

        public Coordinates(double x, double y)
        {
            (X, Y) = (x, y);
        }

    }
}
