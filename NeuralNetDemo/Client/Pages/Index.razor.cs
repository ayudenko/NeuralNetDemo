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

        private Canvas _canvas;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _canvas = new Canvas(await _canvasReference.CreateCanvas2DAsync());
                _canvas.InitAsync();

                SetupNeuralNet();
                _teacher = new(_canvas.Population.Marks, _neuralNet);
                _teacher.Line = _canvas.Line;
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

        private void RemoveLineOnClick()
        {
            _canvas.RemoveLineAsync();
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
                DrawLineAsync(new() { X = mouseX, Y = mouseY });
                _canvas.HasLine = true;
                
            }
            else
            {
                _canvas.Population.AddMark(new Coordinates() { X = mouseX, Y = mouseY });
            }
        }

        private void SetupNeuralNet()
        {
            _neuralNet = new Feedforward(2, 1, true) { ActivationFunction = new BinaryStep() };
            _neuralNet.InitializeWeightsWithRandomizer();
        }

        private async void DrawLineAsync(Coordinates coords)
        {
            _canvas.Line.EndPoint = coords;
            await _canvas.Line.DrawAsync();
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
