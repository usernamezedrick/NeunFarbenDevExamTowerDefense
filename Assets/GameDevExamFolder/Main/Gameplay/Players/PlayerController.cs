using UnityEngine;
using UnityEngine.InputSystem; // For the new Input System
using NF.Main.Core;
using UniRx;

namespace NF.Main.Gameplay.PlayerInput
{
    public class PlayerController : MonoExt
    {
        // Fully qualify the PlayerInput type to avoid namespace conflicts.
        private UnityEngine.InputSystem.PlayerInput _playerInput;

        public override void Initialize()
        {
            base.Initialize();
            _playerInput = GetComponent<UnityEngine.InputSystem.PlayerInput>();
            Debug.Log("PlayerController Initialized.");
        }

        public override void OnSubscriptionSet()
        {
            base.OnSubscriptionSet();
            // Example: Subscribe to an input action (ensure your actions are defined in the Input System)
            // AddEvent(_playerInput.actions["Build"].AsObservable(), _ => BuildTower());
        }

        // Example method for handling build input
        private void BuildTower()
        {
            Debug.Log("Build Tower action triggered!");
        }
    }
}
