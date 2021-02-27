using Blazor.Extensions.Canvas.Canvas2D;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NeuralNetDemo.Client.Entities
{
    public class Canvas
    {

        private readonly Canvas2DContext _context;
        public bool HasLine { get; set; } = false;
        public MarksPopulation Population { get; set; }
        public List<Divider> Lines { get; set; } = new List<Divider>();


        public Canvas(Canvas2DContext context)
        {
            _context = context;
            Population = new MarksPopulation(_context);
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
                if (mark is not null)
                {
                    mark.Draw();
                }
            }
            Lines = new List<Divider>();
        }

        public async Task<Divider> DrawLineAsync(Coordinates coords, string color)
        {
            var line = new Divider(_context, color);
            line.EndPoint = coords;
            await line.DrawAsync();
            Lines.Add(line);
            return line;
        }

        public async Task DrawLinesAsync()
        {
            foreach (var line in Lines)
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
