using Blazor.Extensions.Canvas.Canvas2D;
using System.Threading.Tasks;

namespace NeuralNetDemo.Client.Entities
{
    public class Divider
    {

        private Canvas2DContext _context;

        public Coordinates StartPoint { get; set; } = new() { X = 0, Y = 0 };
        public Coordinates EndPoint { get; set; } = new() { X = 0, Y = 0 };

        public Divider(Canvas2DContext context)
        {
            _context = context;
        }

        public async Task Draw()
        {
            await _context.BeginPathAsync();
            await _context.MoveToAsync(StartPoint.X, StartPoint.Y);
            await _context.LineToAsync(EndPoint.X, EndPoint.Y);
            await _context.StrokeAsync();
            await _context.ClosePathAsync();
        }
    }
}
