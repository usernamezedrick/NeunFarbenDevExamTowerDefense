using UnityEngine;
using UnityEngine.InputSystem; 
using NF.Main.Core;
using UniRx;

namespace NF.Main.Gameplay.PlayerInput
{
    public class PlayerController : MonoExt
    {
       
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
            
        }

       
        private void BuildTower()
        {
            Debug.Log("Build Tower action triggered!");
        }
    }
}
