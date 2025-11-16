using System.Text;

namespace BooleanCompletenessBack.BusinessLogic.FormulaParser
{
    public class Lexer
    {
        private readonly string _input;
        private int _position;
        private readonly HashSet<string> _operators = new HashSet<string>
        {
            "∧", "∨", "¬", "→", "↔", "⊕", "↓", "↑", "↛", "↮"
        };

        public Lexer(string input)
        {
            _input = input.Replace(" ", ""); 
            _position = 0;
        }

        public List<Token> Tokenize()
        {
            var tokens = new List<Token>();

            while (_position < _input.Length)
            {
                char current = _input[_position];

                if (char.IsLetter(current))
                {
                    
                    var sb = new StringBuilder();
                    sb.Append(current);
                    _position++;
                    while (_position < _input.Length && char.IsDigit(_input[_position]))
                    {
                        sb.Append(_input[_position]);
                        _position++;
                    }
                    tokens.Add(new Token(TokenType.Variable, sb.ToString()));
                }
                else if (current == '(')
                {
                    tokens.Add(new Token(TokenType.LeftParen, "("));
                    _position++;
                }
                else if (current == ')')
                {
                    tokens.Add(new Token(TokenType.RightParen, ")"));
                    _position++;
                }
                else
                {
                   
                    string op = current.ToString();
                    if (_operators.Contains(op))
                    {
                        tokens.Add(new Token(TokenType.Operator, op));
                        _position++;
                    }
                    else
                    {
                        throw new ClientException($"Недопустимый символ: {current}");
                    }
                }
            }

            return tokens;
        }
    }

}
