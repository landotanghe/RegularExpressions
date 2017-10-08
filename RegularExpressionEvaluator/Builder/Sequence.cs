namespace RegularExpressionEvaluator
{
    internal class Sequence
    {
        public Sequence(string startState, string endState)
        {
            StartState = startState;
            EndState = endState;
        }
        public string StartState { get; }
        public string EndState { get; }
    }
}
