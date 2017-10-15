using System;

namespace RegularExpression.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetBufferSize(3000, Int16.MaxValue - 1);


            var regex = RegularExpressionEvaluator.RegularExpression.For("a?");
            var automaton = regex._automaton;

            var visualizer = new FiniteAutomata.Visualizer.AutomatonVisualizer();
            visualizer.Visualize(automaton);

            while (true)
            {

            }           
        }
    }
}
