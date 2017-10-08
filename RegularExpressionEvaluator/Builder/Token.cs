namespace RegularExpressionEvaluator
{
    internal class Token
    {
        public Token(char sybmol, TokenType tokenType)
        {
            Symbol = sybmol;
            TokenType = tokenType;
        }

        public char Symbol { get; }
        public TokenType TokenType { get; }
    }
}
