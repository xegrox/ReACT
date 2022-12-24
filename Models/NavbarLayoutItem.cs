namespace ReACT.Models;

public struct NavbarLayoutItem
{
    public readonly string Name;
    public readonly string? Icon;
    public readonly string? Page;

    public NavbarLayoutItem(string name, string? icon = null, string? page = null)
    {
        Name = name;
        Icon = icon;
        Page = page;
    }
}