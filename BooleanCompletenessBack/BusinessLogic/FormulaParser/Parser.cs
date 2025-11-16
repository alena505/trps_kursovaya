using Microsoft.AspNetCore.Mvc;

namespace BooleanCompletenessBack.BusinessLogic.FormulaParser
{
    public class Parser
    {
        private readonly List<Token> _tokens;
        private int _pos;
        public static Dictionary<string, int> _precedence = new Dictionary<string, int>
        {
            { "¬", 6 }, 
            { "∧", 5 }, { "↑", 5 },  
            { "∨", 4 }, { "⊕", 4 }, { "↓", 4 }, 
            { "→", 3 }, { "↛", 3 }, 
            { "↔️", 2 }, { "↮", 2 } 
        };
        private readonly HashSet<string> _rightAssoc = new HashSet<string> { "→", "↛", "¬" };
        private readonly HashSet<string> _unaryOps = new HashSet<string> { "¬" };

        public static Dictionary<string, int> OperatorsPrecedence
        {
            get { return _precedence; }
        }

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
            _pos = 0;
        }

        public List<Token> ToRPN()
        {
            var output = new List<Token>();
            var operators = new Stack<Token>();
            Token prevToken = null;

            while (_pos < _tokens.Count)
            {
                Token token = _tokens[_pos];

                
                if (prevToken != null &&
                    (prevToken.Type == TokenType.Variable || prevToken.Type == TokenType.RightParen) &&
                    (token.Type == TokenType.Variable || token.Type == TokenType.LeftParen ||
                    token.Type == TokenType.Operator && _unaryOps.Contains(token.Value)))
                {
                    
                    HandleOperator(new Token(TokenType.Operator, "∧"), output, operators);
                }

                if (token.Type == TokenType.Variable)
                {
                    output.Add(token);
                }
                else if (token.Type == TokenType.LeftParen)
                {
                    operators.Push(token);
                }
                else if (token.Type == TokenType.RightParen)
                {
                    while (operators.Count > 0 && operators.Peek().Type != TokenType.LeftParen)
                    {
                        output.Add(operators.Pop());
                    }
                    if (operators.Count == 0)
                        throw new ClientException("Несоответствие скобок в формуле");
                    operators.Pop(); 
                }
                else if (token.Type == TokenType.Operator)
                {
                    HandleOperator(token, output, operators);
                }

                prevToken = token;
                _pos++;
            }

            while (operators.Count > 0)
            {
                if (operators.Peek().Type == TokenType.LeftParen)
                    throw new ClientException("Несоответствие скобок в формуле");
                output.Add(operators.Pop());
            }

            ValidateRPN(output);

            return output;
        }


        private void ValidateRPN(List<Token> rpn)
        {
            int stackCount = 0;

            foreach (var token in rpn)
            {
                if (token.Type == TokenType.Variable)
                {
                    stackCount++;
                }
                else if (token.Type == TokenType.Operator)
                {
                    int arity = _unaryOps.Contains(token.Value) ? 1 : 2;
                    if (stackCount < arity)
                    {
                        throw new ClientException("Неверная формула: недостаточно операндов для оператора");
                    }
                    stackCount -= arity;
                    stackCount++;
                }
                // Игнорируем другие типы (не должны появляться в ОПЗ)
            }

            if (stackCount != 1)
            {
                throw new ClientException("Неверная формула: лишние операнды или неполное выражение");
            }
        }


        private void HandleOperator(Token op, List<Token> output, Stack<Token> operators)
        {
            if (_unaryOps.Contains(op.Value) && (operators.Count == 0 || operators.Peek().Type != TokenType.Variable && operators.Peek().Type != TokenType.RightParen))
            {
                
            }

            while (operators.Count > 0 && operators.Peek().Type == TokenType.Operator &&
                   ((_rightAssoc.Contains(op.Value) ? Precedence(op) < Precedence(operators.Peek()) : Precedence(op) <= Precedence(operators.Peek()))))
            {
                output.Add(operators.Pop());
            }
            operators.Push(op);
        }

        private int Precedence(Token op) => _precedence.GetValueOrDefault(op.Value, 0);
    }

}
