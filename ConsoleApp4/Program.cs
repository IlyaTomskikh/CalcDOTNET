/*
 * The algorithm called Shunting yard
 * 
 * ___{ The idea is from https://www.youtube.com/watch?v=HJOnJU77EUs&ab_channel=BYUAwesomeCS235TA &&
 * https://www.youtube.com/watch?v=A-SSrZUHYSk&t=210s&ab_channel=PacktVideo &&
 * https://en.wikipedia.org/wiki/Shunting-yard_algorithm }___
 *
 * Basics:
 * Parse the input string in the infix notation by TokenParser, splitting the string into separate tokens
 * then transform in into postfix notation and compute the expression
 */

using ConsoleApp4;

while (true)
{
    try
    {
        Console.WriteLine("Enter an arithmetic expression in the infix notation");
        var input = Console.ReadLine();
        if (input?.ToLower() is "close" or "exit" or "finish" or "end")
        {
            Console.WriteLine("Thank you for using this calculator! Have a nice day!");
            break;
        }
        if (string.IsNullOrEmpty(input)) continue;
        Console.WriteLine(input + " = " + Calculator.EvaluatePostfix(input));
    }
    catch (InvalidOperationException)
    {
        Console.WriteLine("Invalid input");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}