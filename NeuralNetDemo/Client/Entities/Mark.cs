﻿using Blazor.Extensions.Canvas.Canvas2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NeuralNetDemo.Client.Entities
{
    public class Mark
    {

        public Coordinates Center { get; set; } = new() { X = 0, Y = 0 };
        private string _color;
        private Canvas2DContext _context;

        public Mark(Canvas2DContext context)
        {
            _context = context;
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
            await _context.SetFillStyleAsync(_color);
            await _context.BeginPathAsync();
            await _context.ArcAsync(Center.X, Center.Y, 5, 0, Math.PI * 2);
            await _context.FillAsync();
            await _context.StrokeAsync();
            await _context.ClosePathAsync();
        }

    }
}
