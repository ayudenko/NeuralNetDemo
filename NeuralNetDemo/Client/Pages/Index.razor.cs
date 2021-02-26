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

        private BECanvasComponent _canvasReference;
        private ElementReference _divCanvas;
        private Teacher _teacher;
        private Feedforward _neuralNet;
        private Canvas2DContext _context;
        private Canvas _canvas;

        private int _pointsNumberForTeaching { get; set; } = 50;
        private int _pointsNumberForRandomlyDrawing { get; set; } = 50;
        
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _context = await _canvasReference.CreateCanvas2DAsync();
                _canvas = new Canvas(_context);
                _canvas.InitAsync();
                SetupNeuralNet();
                _teacher = new(_neuralNet, new MarksPopulation(_context));
            }
            
        }

        private void TeachMeOnClick()
        {
            _teacher.Line = _canvas.Line;
            _teacher.Teach(_pointsNumberForTeaching);
        }

        private void DrawMarksRandomly()
        {
            _canvas.ClearCanvas();
            _canvas.AddPopulation(_pointsNumberForRandomlyDrawing);
            _canvas.Population.DrawPopulation();
        }

        private void ClearWeightsOnClick()
        {
            _neuralNet.InitializeWeightsWithRandomizer();
        }

        private void RemoveLineOnClick()
        {
            _canvas.RemoveLineAsync();
            _canvas.Population.DrawPopulation();
        }

        private void ReRunOnClick()
        {
            foreach (var mark in _canvas.Population.Marks)
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
            if (!_canvas.HasLine)
            {
                _canvas.DrawLineAsync(new Coordinates(mouseX, mouseY));
                _canvas.HasLine = true;

            }
            else
            {
                _canvas.Population.AddMark(new Coordinates(mouseX, mouseY));
            }
        }

        private void SetupNeuralNet()
        {
            _neuralNet = new Feedforward(2, 1, true) { ActivationFunction = new BinaryStep() };
            _neuralNet.InitializeWeightsWithRandomizer();
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
