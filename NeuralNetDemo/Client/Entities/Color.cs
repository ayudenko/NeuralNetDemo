namespace NeuralNetDemo.Client.Entities
{
    public class Color
    {

        public string Name { get; init; }
        public bool IsUsed { get; set; } = false;

        public Color(string name)
        {
            Name = name;
        }
        
    }
}
