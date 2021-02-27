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
        private Color[] _colors = new Color[] { new Color("Green"), new Color("Red"), new Color("White") };

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
            _teacher.Teach(_pointsNumberForTeaching);
        }

        private async Task DrawMarksRandomly()
        {
            _canvas.ClearCanvas();
            _canvas.Lines = new List<Divider>();
            await _canvas.DrawLinesAsync();
            _canvas.AddPopulation(_pointsNumberForRandomlyDrawing);
            _canvas.Population.DrawPopulation();
        }

        private void ClearWeightsOnClick()
        {
            foreach (var neuralNetId in _teacher.NeuralNets.Keys)
            {
                _teacher.NeuralNets[neuralNetId].InitializeWeightsWithRandomizer();
            }
        }

        private void RemoveLineOnClick()
        {
            _canvas.RemoveLine();
            _teacher.NeuralNets = new Dictionary<Divider, Feedforward>();
            _canvas.Population.DrawPopulation();
            foreach (Color color in _colors)
            {
                color.IsUsed = false;
            }
        }

        private void ReRunOnClick()
        {
            foreach (var mark in _canvas.Population.Marks)
            {
                mark.SetColor("Black");
            }
            foreach (var neuralNetId in _teacher.NeuralNets.Keys)
            {
                foreach (var mark in _canvas.Population.Marks)
                {
                    _teacher.NeuralNets[neuralNetId].SetInputs(new float[] { (float)mark.Center.X, (float)mark.Center.Y });
                    _teacher.NeuralNets[neuralNetId].Process();
                    var output = _teacher.NeuralNets[neuralNetId].GetOutputs()[0];
                    if (output == 1f)
                    {
                        mark.SetColor(neuralNetId.Color);
                    }
                    mark.Draw();
                }
            }
        }

        private async void OnClick(MouseEventArgs eventArgs)
        {
            if (_maxNumberOfLines > _teacher.NeuralNets.Count)
            {
                var data = await jsRuntime.InvokeAsync<BoundingClientRect>("MyDOMGetBoundingClientRect", (object)_divCanvas);
                double mouseX = (int)(eventArgs.ClientX - data.Left);
                double mouseY = (int)(eventArgs.ClientY - data.Top);
                string backgroundColor = "Black";
                foreach (Color color in _colors)
                {
                    if (!color.IsUsed)
                    {
                        backgroundColor = color.Name;
                        color.IsUsed = true;
                        break;
                    }
                }
                var line = await _canvas.DrawLineAsync(new Coordinates(mouseX, mouseY), backgroundColor);
                AddNeuralNet(line);
            }
        }

        private void AddNeuralNet(Divider line)
        {
            var neuralNet = new Feedforward(2, 2, true) { ActivationFunction = new BinaryStep() };
            neuralNet.InitializeWeightsWithRandomizer();
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
