using Assets.Scripts.Objects;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour, IInteractable
{
    [SerializeField] private UnityEvent _onInteract;
    public UnityEvent OnInteract => _onInteract;

    public void Interact()
    {
        Debug.Log($"{name} was interacted with");
        _onInteract.Invoke();
    }
}
