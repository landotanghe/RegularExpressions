namespace RegularExpressionEvaluator
{
    internal enum TokenType
    {
        Character,
        OrOperator,
        EndOfInput,
        StartNewSequence,
        EndNewSequence,
        RepeatZeroOrMore,
        OpenRepeat,
        CloseRepeat,
        RepeatAtLeastOnce
    }
}
