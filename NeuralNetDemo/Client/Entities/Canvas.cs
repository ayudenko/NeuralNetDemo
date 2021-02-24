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
        public Divider Line { get; set; }


        public Canvas(Canvas2DContext context)
        {
            _context = context;
        }

        public void InitAsync()
        {
            _context.SetStrokeStyleAsync("Black");
            AddPopulation();
            Line = new Divider(_context);
        }

        private void AddPopulation()
        {
            Population = new(_context);
            Population.Populate();
        }

        public async void RemoveLineAsync()
        {
            await _context.ClearRectAsync(0, 0, 500, 500);
            HasLine = false;
            foreach (var mark in Population.Marks)
            {
                mark.Draw();
            }
        }

        public async void DrawLineAsync(Coordinates coords)
        {
            Line.EndPoint = coords;
            await Line.DrawAsync();
            HasLine = true;
        }

    }
}
