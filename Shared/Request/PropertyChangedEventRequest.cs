namespace Shared.Request;

public sealed class PropertyChangedEventRequest
{
    public string EntityId { get; init; } = default!;
    public string Selector { get; init; } = default!;
    public object NewValue { get; init; } = default!;
}
