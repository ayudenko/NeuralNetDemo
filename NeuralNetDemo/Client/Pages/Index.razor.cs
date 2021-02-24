using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using NeuralNetDemo.Client.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NeuralNetDemo.Client.Pages
{
    public partial class Index
    {

        private Canvas2DContext _context;
        private ElementReference _divCanvas;

        protected BECanvasComponent _canvasReference;

        private List<Mark> marks = new List<Mark>();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            _context = await _canvasReference.CreateCanvas2DAsync();
            await _context.SetStrokeStyleAsync("Black");

            var line = new Divider(_context);
            line.EndPoint.X = 500;
            line.EndPoint.Y = 200;
            await line.Draw();
        }

        private async void OnClick(MouseEventArgs eventArgs)
        {
            var data = await jsRuntime.InvokeAsync<BoundingClientRect>("MyDOMGetBoundingClientRect", (object)_divCanvas);
            double mouseX = (int)(eventArgs.ClientX - data.Left);
            double mouseY = (int)(eventArgs.ClientY - data.Top);

            var mark = new Mark(_context);
            mark.Center.X = mouseX;
            mark.Center.Y = mouseY;
            mark.SetWhite();
            mark.Draw();
            marks.Add(mark);
        }

        public class BoundingClientRect
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double Width { get; set; }
            public double Height { get; set; }
            public double Top { get; set; }
            public double Right { get; set; }
            public double Bottom { get; set; }
            public double Left { get; set; }
        }

    }
}
