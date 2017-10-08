namespace RegularExpressionEvaluator
{
    internal class StateNamer
    {
        private const string StartStatePrefix = "Start ";
        private const string FinalStatePrefix = "Final ";
        private const string IntermediatePrefix = "Intermediate ";

        private int StateNumber = 0;

        public string CreateNameForStartOfSequence()
        {
            return StartStatePrefix + StateNumber++;
        }

        public string CreateNameForEndOfSequence()
        {
            return FinalStatePrefix + StateNumber++;
        }

        public string CreateNameForIntermediateState()
        {
            return IntermediatePrefix + StateNumber++;
        }
    }
}