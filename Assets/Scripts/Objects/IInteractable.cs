using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace Assets.Scripts.Objects
{
    public interface IInteractable
    {
        public UnityEvent OnInteract { get; }
        public void Interact();


    }
}
