namespace CodeChops.Crossblade;

public enum Position
{
    Static,
    Relative,
    Absolute,
    Sticky,
    Fixed,
    Initial,
    Inherit,
    Unset,
    Revert,
}

public static class PositionExtensions
{
    public static string GetAttribute(this Position position) 
        => position.ToString().ToLowerInvariant();
}