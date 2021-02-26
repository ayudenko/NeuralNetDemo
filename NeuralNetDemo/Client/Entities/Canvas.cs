using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NeuralNetDemo.Client.Entities
{
    public class Canvas
    {

        private readonly Canvas2DContext _context;
        public bool HasLine { get; set; } = false;
        public MarksPopulation Population { get; set; }
        public Dictionary<double, Divider> Lines { get; set; } = new Dictionary<double, Divider>();


        public Canvas(Canvas2DContext context)
        {
            _context = context;
        }

        public void InitAsync()
        {
            _context.SetStrokeStyleAsync("Black");
        }

        public void AddPopulation(int numberOfMarks)
        {
            Population = new(_context);
            Population.CreatePopulation(numberOfMarks);
        }

        public void RemoveLine()
        {
            ClearCanvas();
            foreach (var mark in Population.Marks)
            {
                mark.Draw();
            }
        }

        public async Task<Divider> DrawLineAsync(Coordinates coords)
        {
            var line = new Divider(_context);
            line.EndPoint = coords;
            await line.DrawAsync();
            Lines.Add(line.EndPoint.X, line);
            return line;
        }

        public async Task DrawLinesAsync()
        {
            foreach (var line in Lines.Values)
            {
                await line.DrawAsync();
            }
        }

        public async void ClearCanvas()
        {
            await _context.ClearRectAsync(0, 0, 500, 500);
        }

    }
}
