using Models.NeuralNetModels;
using System.Collections.Generic;

namespace NeuralNetDemo.Client.Entities
{
    public class Teacher
    {

        private List<Mark> _marks = new List<Mark>();
        private Feedforward _neuralNet;
        public Divider Line { get; set; }

        public Teacher(List<Mark> marks, Feedforward neuralNet)
        {
            (_marks, _neuralNet) = (marks, neuralNet);
        }

        public void Teach()
        {
            foreach (var mark in _marks)
            {
                _neuralNet.SetInputs(new float[] { (float)mark.Center.X, (float)mark.Center.Y });
                _neuralNet.Process();
                var output = _neuralNet.GetOutputs()[0];
                var expectedResult = 0f;
                if (Line.IsAboveTheLine(mark.Center))
                {
                    expectedResult = 1f;
                }
                var error = 2f;
                if (expectedResult == output)
                {
                    error = 0f;
                }
                else if (expectedResult < output)
                {
                    error = -2f;
                }
                //_neuralNet.AdjustWeightsWithError(error);
                var inputs = new float[] { (float)mark.Center.X, (float)mark.Center.Y, 1f };
                for (var i = 0; i < _neuralNet.Weights.GetLength(0); i++)
                {
                    for (var k = 0; k < _neuralNet.Weights.GetLength(1); k++)
                    {
                        _neuralNet.Weights[i, k] += 0.005f * error * inputs[k];
                    }
                }
            }
        }

    }
}
