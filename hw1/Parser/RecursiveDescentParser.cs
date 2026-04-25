namespace hw1.Parser;

using hw1.Tokenizer;

/// <summary>
/// The class computes the value of expression defined by the following grammar (in EBNF)
/// 
/// <code>
/// Expression ::= Sum EndOfExpression
/// Sum ::= Product ( Plus|Minus Product )*
/// Product ::= Grouping ( Star|Slash Grouping )*
/// Grouping ::= Number | ( LeftParen Sum RightParen )
/// </code>
/// </summary>
public class RecursiveDescentParser : IParser
{
    private readonly ITokenizer _tokenizer;
    private Token? _lastToken;

    public RecursiveDescentParser(ITokenizer tokenizer)
    {
        _tokenizer = tokenizer;
    }

    // Expression ::= Sum EndOfExpression
    public float ComputeExpression()
    {
        float result = ComputeSum();

        ExpectToken(TokenType.EndOfExpression);

        return result;
    }

    // Sum ::= Product ( Plus|Minus Product )*
    private float ComputeSum()
    {
        float value = ComputeProduct();

        while (PeekNextToken()?.Type is TokenType.Plus or TokenType.Minus)
        {
            // Token is not null, because PeekNextToken() did not return null
            Token token = (Token)GetNextToken()!;
            float nextValue = ComputeProduct();

            if (token.Type == TokenType.Plus)
            {
                value += nextValue;
            }
            else
            {
                value -= nextValue;
            }
        }

        return value;
    }

    // Product ::= Grouping ( Star|Slash Grouping )*
    private float ComputeProduct()
    {
        float value = ComputeGrouping();

        while (PeekNextToken()?.Type is TokenType.Star or TokenType.Slash)
        {
            // Token is not null, because PeekNextToken() did not return null
            Token token = (Token)GetNextToken()!;
            float nextValue = ComputeGrouping();

            if (token.Type == TokenType.Star)
            {
                value *= nextValue;
            }
            else
            {
                if (nextValue == 0)
                {
                    throw new DivisionByZeroException(token.Line, token.Column);
                }
                value /= nextValue;
            }
        }

        return value;
    }

    // Grouping ::= Number | ( LeftParen Sum RightParen )
    private float ComputeGrouping()
    {
        if (PeekNextToken()?.Type == TokenType.LeftParen)
        {
            ExpectToken(TokenType.LeftParen);
            float value = ComputeSum();
            ExpectToken(TokenType.RightParen);
            return value;
        }
        return ExpectToken(TokenType.Number).NumericValue.GetValueOrDefault();
    }

    private Token ExpectToken(TokenType expected) => GetNextToken() switch
    {
        Token token when token.Type == expected => token,
        Token token => throw new UnexpectedTokenException(token, expected),
        null => throw new ExpressionEndedException(expected)
    };

    private Token? GetNextToken()
    {
        if (_lastToken != null)
        {
            Token? token = _lastToken;
            _lastToken = null;
            return token;
        }
        return _tokenizer.GetNextToken();
    }

    private Token? PeekNextToken()
    {
        if (_lastToken != null)
        {
            return _lastToken;
        }
        _lastToken = _tokenizer.GetNextToken();
        return _lastToken;
    }
}
