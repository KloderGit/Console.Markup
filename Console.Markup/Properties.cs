using Markup.Interface;
using Markup.RenderElement;

namespace Markup;

public record Bound(int Top = 0, int Bottom = 0, int Left = 0, int Right = 0);

public readonly record struct Margin(int Top = 0, int Bottom = 0, int Left = 0, int Right = 0);
public readonly record struct Padding(int Top = 0, int Bottom = 0, int Left = 0, int Right = 0);

public readonly record struct Border(bool top = false, bool bottom = false, bool left = false,
    bool right = false);