using Blazor.Extensions.Canvas.Canvas2D;
using System;
using System.Collections.Generic;

namespace NeuralNetDemo.Client.Entities
{
    public class MarksPopulation
    {

        private readonly Canvas2DContext _canvas;

        public List<Mark> Marks { get; set; } = new List<Mark>();

        public MarksPopulation(Canvas2DContext canvas)
        {
            _canvas = canvas;
        }

        public void Populate()
        {
            Random r = new();
            for (int i = 0; i < 100; i++)
            {
                var x = r.Next(0, 500);
                var y = r.Next(0, 500);
                Mark mark = new(_canvas);
                mark.Center.X = x;
                mark.Center.Y = y;
                mark.Draw();
                Marks.Add(mark);
            }
        }
    }
}
