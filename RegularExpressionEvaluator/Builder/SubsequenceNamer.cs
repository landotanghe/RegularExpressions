namespace RegularExpressionEvaluator.Builder
{
    internal class SubsequenceNamer
    {
        public int _id = 0;

        public string CreateNameForSubSequence()
        {
            return "Subsequence " + _id++;
        }
    }
}
