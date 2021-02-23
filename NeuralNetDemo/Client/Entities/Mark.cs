using Blazor.Extensions.Canvas.Canvas2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NeuralNetDemo.Client.Entities
{
    public class Mark
    {

        private readonly double _x;
        private readonly double _y;
        private string _color;
        private Canvas2DContext _context;

        public Mark(double x, double y, Canvas2DContext context)
        {
            (_x, _y, _context) = (x, y, context);
        }

        public void SetGreen()
        {
            _color = "Green";
        }

        public void SetRed()
        {
            _color = "Red";
        }

        public void SetWhite()
        {
            _color = "White";
        }

        public async void Draw()
        {
            await _context.BeginPathAsync();
            await _context.SetFillStyleAsync(_color);
            await _context.SetStrokeStyleAsync("Black");
            await _context.ArcAsync(_x, _y, 5, 0, Math.PI * 2);
            await _context.FillAsync();
            await _context.StrokeAsync();
        }

    }
}
