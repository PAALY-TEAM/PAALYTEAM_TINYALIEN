using UnityEngine;

namespace Interfaces
{
    public interface IInteractable
    {
        GameObject Player { get; set; }
    
        bool CanInteract { get; set; }

        void Interact();
    }
}
