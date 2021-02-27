using Blazor.Extensions.Canvas.Canvas2D;
using System.Threading.Tasks;

namespace NeuralNetDemo.Client.Entities
{
    public class Divider
    {

        private Canvas2DContext _context;
        private float _multiplier = 1;

        public Coordinates StartPoint { get; set; } = new(0, 0);
        public Coordinates EndPoint { get; set; } = new(0, 0);
        public string Color { get; init; }

        public Divider(Canvas2DContext context, string color)
        {
            _context = context;
            Color = color;
        }

        public float GetMultiplier()
        {
            return _multiplier;
        }

        public async Task DrawAsync()
        {
            NormalizeCoords();
            await _context.BeginPathAsync();
            await _context.MoveToAsync(StartPoint.X, StartPoint.Y);
            await _context.LineToAsync(EndPoint.X, EndPoint.Y);
            await _context.StrokeAsync();
            await _context.ClosePathAsync();
        }

        public bool IsAboveTheLine(Coordinates coords)
        {
            return coords.X * _multiplier > coords.Y;
        }

        private void NormalizeCoords()
        {
            _multiplier = (float)(EndPoint.Y / EndPoint.X);
            double maxX = 500;
            double maxY = 500;
            if (EndPoint.X > EndPoint.Y)
            {
                EndPoint = new Coordinates(maxX, _multiplier * maxX);
            }
            else
            {
                EndPoint = new Coordinates(maxY / _multiplier, maxY);
            }
        }
    }
}
