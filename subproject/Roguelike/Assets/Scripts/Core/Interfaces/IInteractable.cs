using UnityEngine;

namespace Roguelike.Core.Interfaces
{
    public interface IInteractable
    {
        void Interact(GameObject interactor);
    }
}