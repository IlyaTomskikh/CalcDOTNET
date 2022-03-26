namespace ConsoleApp4;
public static class Calculator
{
    private static Queue<IToken?> InfixToPostfix(TokenParser parser)
    {
        var output = new Queue<IToken?>();
        var operators = new Stack<IToken?>();

        while (parser.HasNext())
        {
            //add a number into the queue
            var token = parser.IsNextToken();
            if (token is Number)
            {
                output.Enqueue(token);
                continue;
            }

            //we must have at least 2 numbers straight
            if (token is not Operator curOp)
                throw new Exception($"Operator <{token}> is not supported");

            switch (curOp.Value)
            {
                case "(":
                    operators.Push(curOp);
                    continue;
                //when we meet a closing bracket start searching for an opening bracket
                //adding all operators according to their priority
                case ")":
                    while (true)
                    {
                        if (!operators.TryPeek(out var topToken))
                            throw new InvalidOperationException("Incorrect input");
                        //if we found one remove both from the stack
                        if (topToken is Operator {Value: "("})
                        {
                            operators.Pop();
                            break;
                        }
                        //else move the token into queue
                        output.Enqueue(topToken);
                        operators.Pop();
                    }
                    continue;
            }

            //operator processing (that is not a bracket)
            while (operators.Count > 0)
            {
                if (!operators.TryPeek(out var topToken)) break;
                if (topToken is not Operator topOp) continue;
                if (string.Equals(topOp.Value, "(", StringComparison.Ordinal)) break;
                if (curOp.Priority < topOp.Priority ||
                    curOp.Priority.Equals(topOp.Priority) &&
                    string.Equals(curOp.Associativity, "left", StringComparison.Ordinal))
                {
                    operators.Pop();
                    output.Enqueue(topOp);
                    continue;
                }

                break;
            }
            operators.Push(curOp);
        }

        while (operators.Count > 0)
        {
            var topToken = operators.Pop();
            //If the token is a bracket, than there are unclosed brackets somewhere
            if (topToken is Operator {Value: ")" or "("})
                throw new InvalidOperationException("Mismatched brackets");
            output.Enqueue(topToken);
        }

        return output;
    }

    public static double EvaluatePostfix(string input)
    {
        var parser = new TokenParser(input);
        var expression = InfixToPostfix(parser);
        var numbers = new Stack<double>();
        while (expression.Count > 0)
        {
            var token = expression.Dequeue();
            switch (token)
            {
                case Number number:
                    numbers.Push(number.Value);
                    continue;
                case Operator op:
                {
                    double left;
                    double right;
                    switch (op.Value)
                    {
                        case "+":
                            right = numbers.Pop();
                            left = numbers.Pop();
                            numbers.Push(left + right);
                            break;

                        case "-":
                            right = numbers.Pop();
                            left = numbers.Pop();
                            numbers.Push(left - right);
                            break;

                        case "*":
                            right = numbers.Pop();
                            left = numbers.Pop();
                            numbers.Push(left * right);
                            break;

                        case "/":
                            right = numbers.Pop();
                            if (right == 0) throw new DivideByZeroException();
                            left = numbers.Pop();
                            numbers.Push(left / right);
                            break;

                        case "-u":
                            right = numbers.Pop();
                            numbers.Push(-right);
                            break;

                        case "+u":
                            break;
                    }

                    break;
                }
            }
        }

        return numbers.Pop();
    }
}