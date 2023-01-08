namespace CodeChops.Crossblade;

internal static class RazorHelpers
{
    public static string? If(bool predicate, string output)
    {
        return predicate ? output : null;
    }

    public static string? If(string? value, string output)
	{
        return value is not null ? output : null;
	}
}
