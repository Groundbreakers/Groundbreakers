namespace Core
{
    using UnityEngine.EventSystems;

    public interface ITileSelectMessageTarget : IEventSystemHandler
    {
        void Select();

        void Deselect();
    }
}