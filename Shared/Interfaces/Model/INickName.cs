namespace Shared.Interfaces.ModelsBase;

public interface INickName
{
    string Name { get; set; }
    string NickName { get; set; }

    string DisplayName => string.IsNullOrEmpty(NickName) ? Name : NickName;
}