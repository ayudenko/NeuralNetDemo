using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Models.NeuralNetModels;
using Models.NeuralNetModels.ActivationFunctions;
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
        private MarksPopulation _population;
        protected BECanvasComponent _canvasReference;
        private ElementReference _divCanvas;
        protected Teacher _teacher;
        private ElementReference _teachMeButton;
        private Feedforward _neuralNet;
        private Coordinates _lineClickCoords;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _context = await _canvasReference.CreateCanvas2DAsync();
                await _context.SetStrokeStyleAsync("Black");
                AddPopulation();
                SetupNeuralNet();
                _teacher = new(_population.Marks, _neuralNet);
            }

        }

        private void TeachMeOnClick()
        {
            _teacher.Teach();
        }

        private async void OnClick(MouseEventArgs eventArgs)
        {
            var data = await jsRuntime.InvokeAsync<BoundingClientRect>("MyDOMGetBoundingClientRect", (object)_divCanvas);
            double mouseX = (int)(eventArgs.ClientX - data.Left);
            double mouseY = (int)(eventArgs.ClientY - data.Top);
            if (_lineClickCoords is null)
            {
                _lineClickCoords = new();
                _lineClickCoords.X = mouseX;
                _lineClickCoords.Y = mouseY;
                DrawLineAsync();
            }
            else
            {
                foreach (var mark in _population.Marks)
                {
                    _neuralNet.SetInputs(new float[] { (float)mark.Center.X, (float)mark.Center.Y });
                    _neuralNet.Process();
                    var output = _neuralNet.GetOutputs()[0];
                    if (output == 1f)
                    {
                        mark.SetGreen();
                    }
                    else
                    {
                        mark.SetRed();
                    }
                    mark.Draw();
                }
            }
        }

        private void SetupNeuralNet()
        {
            _neuralNet = new Feedforward(2, 1, true) { ActivationFunction = new BinaryStep() };
            _neuralNet.InitializeWeightsWithRandomizer();
        }

        private void AddPopulation()
        {
            _population = new(_context);
            _population.Populate();
        }

        private async void DrawLineAsync()
        {
            var line = new Divider(_context);
            line.EndPoint = _lineClickCoords;
            await line.Draw();
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
