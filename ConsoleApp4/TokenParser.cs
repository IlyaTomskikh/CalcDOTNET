namespace ConsoleApp4;

internal class TokenParser 
{
    //arithmetic expression
    private readonly string _expression;
    //current position of parser
    private int _position;
    private bool _unary;
    //is used to find errors in the expression (the preview token is number or not)
    private bool _prev;

    public TokenParser(string expression)
    {
        _expression = expression;
        _position = 0;
        _unary = true;
        _prev = false;
    }

    public bool HasNext()
    {
        return _position < _expression.Length;
    }

    public IToken IsNextToken()
    {
        while (_position < _expression.Length)
            switch (_expression[_position])
            {
                case '+':
                    _prev = false;
                    ++_position;
                    if (_unary) return new Operator("+u", 3, "right");
                    _unary = true;
                    return new Operator("+", 1);
                
                case '-':
                    _prev = false;
                    ++_position;

                    //unary minus:
                    if (_unary) return new Operator("-u", 3, "right");
                    _unary = true;
                    return new Operator("-", 1);

                case '*':
                    _prev = false;
                    _unary = true;
                    ++_position;
                    return new Operator("*", 2);

                case '/':
                    _prev = false;
                    _unary = true;
                    ++_position;
                    return new Operator("/", 2);

                case '(':
                    _prev = false;
                    _unary = true;
                    ++_position;
                    return new Operator("(", 4);

                case ')':
                    _prev = false;
                    _unary = false;
                    ++_position;
                    return new Operator(")", 4);

                case ' ':
                    ++_position;
                    break;

                //fractional part
                case ',' or '.':
                    _unary = false;
                    if (_prev) throw new InvalidOperationException("Invalid operation");
                    _prev = true;
                    var startIndex = _position;
                    ++_position;
                    while (_position < _expression.Length && char.IsDigit(_expression[_position])) ++_position;

                    var value = string.Concat("0", _expression.AsSpan(startIndex, _position - startIndex));
                    var number = Convert.ToDouble(value);
                    return new Number(number);

                default:
                    _unary = false;
                    if (!char.IsNumber(_expression[_position]))
                        throw new ArgumentException($"Symbol <{_expression[_position]}> is not supported");
                    if (_prev) throw new InvalidOperationException("Invalid operation");
                    _prev = true;
                    startIndex = _position;
                    ++_position;
                    while (_position < _expression.Length && char.IsDigit(_expression[_position])) ++_position;
                    if (_position < _expression.Length && _expression[_position].Equals(','))
                    {
                        ++_position;
                        while (_position < _expression.Length && char.IsDigit(_expression[_position])) ++_position;
                    }
                    value = _expression.Substring(startIndex, _position - startIndex);
                    number = double.Parse(value);
                    return new Number(number);
            }

        throw new ArgumentException("Incorrect input");
    }
}