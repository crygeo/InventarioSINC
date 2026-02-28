namespace Utilidades.Interfaces;

public interface ISelectable
{
    bool IsSelectable { get; set; }
    bool IsSelect { get; set; }

    void Select()
    {
        IsSelect = IsSelectable;
    }

    bool Selectable()
    {
        return IsSelectable;
    }
}