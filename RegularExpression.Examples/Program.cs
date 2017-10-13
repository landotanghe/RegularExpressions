using System;

namespace RegularExpression.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            var regex = RegularExpressionEvaluator.RegularExpression.For("(ab){3}");
            var automaton = regex._automaton;

            Console.SetBufferSize(3000, Int16.MaxValue-1);
            var visualizer = new FiniteAutomata.Visualizer.AutomatonVisualizer();
            visualizer.Visualize(automaton);

            while (true)
            {

            }           
        }
    }
}
