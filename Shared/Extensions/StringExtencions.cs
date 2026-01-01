namespace Shared.Extensions;

public static class StringExtencions
{
    public static void AddOrRemove(this List<string> str, string value)
    {
        if (str.Contains(value))
            str.Remove(value);
        else
            str.Add(value);
    }
}