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
                Coordinates coords = new() { X = x, Y = y };
                AddMark(coords);
            }
        }

        public void AddMark(Coordinates coords)
        {
            Mark mark = new(_canvas);
            mark.Center.X = coords.X;
            mark.Center.Y = coords.Y;
            mark.Draw();
            Marks.Add(mark);
        }

        public void DrawPopulation()
        {
            foreach (var mark in Marks)
            {
                mark.Draw();
            }
        }

    }
}
