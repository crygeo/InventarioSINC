namespace Shared.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class IdTypeAttribute : Attribute
{
    public IdTypeAttribute(Type targetType)
    {
        TargetType = targetType;
    }

    public Type TargetType { get; }
}