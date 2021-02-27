using Models.NeuralNetModels;
using System.Collections.Generic;

namespace NeuralNetDemo.Client.Entities
{
    public class Teacher
    {

        private MarksPopulation _marksPopulation;
        public  SortedDictionary<Divider, Feedforward> NeuralNets { get; set; } = new SortedDictionary<Divider, Feedforward>(new LinesComparer());

        public Teacher(MarksPopulation marksPopulation)
        {
            _marksPopulation = marksPopulation;
        }

        public void Teach(int numberOfPointsToTeach)
        {
            _marksPopulation.Marks = new List<Mark>();
            foreach (var lineId in NeuralNets.Keys)
            {
                if (NeuralNets[lineId] is not null)
                {
                    _marksPopulation.CreatePopulation(numberOfPointsToTeach);
                    List<Mark> marks = _marksPopulation.Marks;
                    foreach (var mark in marks)
                    {
                        NeuralNets[lineId].SetInputs(new float[] { (float)mark.Center.X, (float)mark.Center.Y });
                        NeuralNets[lineId].Process();
                        var output = NeuralNets[lineId].GetOutputs()[0];
                        var expectedResult = 0f;
                        if (lineId.IsAboveTheLine(mark.Center))
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
                        for (var i = 0; i < NeuralNets[lineId].Weights.GetLength(0); i++)
                        {
                            for (var k = 0; k < NeuralNets[lineId].Weights.GetLength(1); k++)
                            {
                                NeuralNets[lineId].Weights[i, k] += 0.005f * error * inputs[k];
                            }
                        }
                    }
                }
            }
        }

    }
}
