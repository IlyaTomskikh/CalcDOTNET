using System.Globalization;

namespace ConsoleApp4;

internal class Number: IToken
{
    public double Value { get; }

    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);

    public Number(double number) => Value = number;
}