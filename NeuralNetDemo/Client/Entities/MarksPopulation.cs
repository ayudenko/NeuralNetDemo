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

        public void CreatePopulation(int number)
        {
            Random r = new();
            for (int i = 0; i < number; i++)
            {
                var x = r.Next(0, 700);
                var y = r.Next(0, 700);
                Coordinates coords = new(x, y);
                AddMark(coords);
            }
        }

        public void AddMark(Coordinates coords)
        {
            Mark mark = new(_canvas);
            mark.Center = new Coordinates(coords.X, coords.Y);
            Marks.Add(mark);
        }

        public void DrawPopulation()
        {
            _canvas.BeginPathAsync();
            foreach (var mark in Marks)
            {
                mark.Draw();
            }
            _canvas.EndBatchAsync();
        }

    }
}
