namespace ConsoleApp4;

internal class Operation: IToken
{
    public string Value { get; }
    public int Priority { get; }
    public string Associativity { get; }


    public override string ToString() => Value;

    public Operation(string op, int priority, string associativity = "left")
    {
        Value = op;
        Priority = priority;
        Associativity = associativity;
    }
}