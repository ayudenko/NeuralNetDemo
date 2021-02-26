using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Models.NeuralNetModels;
using Models.NeuralNetModels.ActivationFunctions;
using NeuralNetDemo.Client.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NeuralNetDemo.Client.Pages
{
    public partial class Index
    {

        private BECanvasComponent _canvasReference;
        private ElementReference _divCanvas;
        private Teacher _teacher;
        private Dictionary<Divider, Feedforward> _linesNeuralNets = new Dictionary<Divider, Feedforward>();
        private Canvas2DContext _context;
        private Canvas _canvas;
        private int _maxNumberOfLines = 2;

        private int _pointsNumberForTeaching { get; set; } = 50;
        private int _pointsNumberForRandomlyDrawing { get; set; } = 50;
        
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _context = await _canvasReference.CreateCanvas2DAsync();
                _canvas = new Canvas(_context);
                _canvas.InitAsync();
                _teacher = new(new MarksPopulation(_context));
            }
            
        }

        private void TeachMeOnClick()
        {
            _teacher.Lines = _canvas.Lines;
            _teacher.Teach(_pointsNumberForTeaching);
        }

        private async Task DrawMarksRandomly()
        {
            _canvas.ClearCanvas();
            await _canvas.DrawLinesAsync();
            _canvas.AddPopulation(_pointsNumberForRandomlyDrawing);
            _canvas.Population.DrawPopulation();
        }

        private void ClearWeightsOnClick()
        {
            foreach (var neuralNetId in _linesNeuralNets.Keys)
            {
                _linesNeuralNets[neuralNetId].InitializeWeightsWithRandomizer();
            }
        }

        private void RemoveLineOnClick()
        {
            _canvas.RemoveLine();
            _canvas.Population.DrawPopulation();
        }

        private void ReRunOnClick()
        {
            foreach (var mark in _canvas.Population.Marks)
            {
                foreach (var neuralNetId in _linesNeuralNets.Keys)
                {
                    _linesNeuralNets[neuralNetId].SetInputs(new float[] { (float)mark.Center.X, (float)mark.Center.Y });
                    _linesNeuralNets[neuralNetId].Process();
                    var output = _linesNeuralNets[neuralNetId].GetOutputs()[0];
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

        private async void OnClick(MouseEventArgs eventArgs)
        {
            if (_maxNumberOfLines > _linesNeuralNets.Count)
            {
                var data = await jsRuntime.InvokeAsync<BoundingClientRect>("MyDOMGetBoundingClientRect", (object)_divCanvas);
                double mouseX = (int)(eventArgs.ClientX - data.Left);
                double mouseY = (int)(eventArgs.ClientY - data.Top);
                var line = await _canvas.DrawLineAsync(new Coordinates(mouseX, mouseY));
                AddNeuralNet(line);
            }
        }

        private void AddNeuralNet(Divider line)
        {
            var neuralNet = new Feedforward(2, 2, true) { ActivationFunction = new BinaryStep() };
            neuralNet.InitializeWeightsWithRandomizer();
            _linesNeuralNets.Add(line, neuralNet);
            _teacher.NeuralNets.Add(line, neuralNet);
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
