using Blazor.Extensions.Canvas.Canvas2D;
using System.Threading.Tasks;

namespace NeuralNetDemo.Client.Entities
{
    public class Divider
    {

        private Canvas2DContext _context;
        private float _multiplier = 1;

        public Coordinates StartPoint { get; set; } = new() { X = 0, Y = 0 };
        public Coordinates EndPoint { get; set; } = new() { X = 0, Y = 0 };

        public Divider(Canvas2DContext context)
        {
            _context = context;
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
            if (EndPoint.X > EndPoint.Y)
            {
                EndPoint.X = 500;
                EndPoint.Y = _multiplier * EndPoint.X;
            }
            else
            {
                EndPoint.Y = 500;
                EndPoint.X = EndPoint.Y / _multiplier;
            }
        }
    }
}
