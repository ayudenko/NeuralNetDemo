using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Models.NeuralNetModels;
using Models.NeuralNetModels.ActivationFunctions;
using NeuralNetDemo.Client.Entities;
using System.Threading.Tasks;

namespace NeuralNetDemo.Client.Pages
{
    public partial class Index
    {

        private Canvas2DContext _context;
        private MarksPopulation _population;
        private BECanvasComponent _canvasReference;
        private ElementReference _divCanvas;
        private Teacher _teacher;
        private Feedforward _neuralNet;
        private Coordinates _lineClickCoords;
        private Divider _line;
        private bool _needNewLine = true;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _context = await _canvasReference.CreateCanvas2DAsync();
                await _context.SetStrokeStyleAsync("Black");
                AddPopulation();
                SetupNeuralNet();
                _line = new Divider(_context);
                _teacher = new(_population.Marks, _neuralNet);
                _teacher.Line = _line;
            }

        }

        private void TeachMeOnClick()
        {
            _teacher.Teach();
        }

        private void ClearWeightsOnClick()
        {
            _neuralNet.InitializeWeightsWithRandomizer();
        }

        private async void RemoveLineOnClick()
        {
            await _context.ClearRectAsync(0, 0, 500, 500);
            _needNewLine = true;
            foreach (var mark in _population.Marks)
            {
                mark.Draw();
            }
        }

        private void ReRunOnClick()
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

        private async void OnClick(MouseEventArgs eventArgs)
        {
            var data = await jsRuntime.InvokeAsync<BoundingClientRect>("MyDOMGetBoundingClientRect", (object)_divCanvas);
            double mouseX = (int)(eventArgs.ClientX - data.Left);
            double mouseY = (int)(eventArgs.ClientY - data.Top);
            if (_needNewLine)
            {
                _lineClickCoords = new() { X = mouseX, Y = mouseY };
                DrawLineAsync();
                _needNewLine = false;
                
            }
            else
            {
                _population.AddMark(new Coordinates() { X = mouseX, Y = mouseY });
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
            _line.EndPoint = _lineClickCoords;
            await _line.DrawAsync();
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
