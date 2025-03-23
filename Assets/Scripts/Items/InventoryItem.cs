using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class InventoryItem: NetworkBehaviour, Interactable
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite InventoryPicture { get; private set; }
        public bool IsInteractable { get => _isInteractable; set => _isInteractable = value; }
        public GameObject Player { get => _player; set => _player = value; }

        [SyncVar] private bool _isInteractable = true;
        [SyncVar] private GameObject _player;

        public Action OnHoverEnterEvent;
        public Action OnHoverExitEvent;

        private void Start()
        {
        }

        public virtual bool Interact(InteractionController controller)
        {
            return controller.AddItemToInventory(this);
        }

        public void OnHoverEnter(InteractionController controller)
        {
            OnHoverEnterEvent?.Invoke();
        }

        public void OnHoverExit(InteractionController controller)
        {
            OnHoverExitEvent?.Invoke();
        }

        [Command(requiresAuthority = false)]
        public void CmdUse()
        {
            UseOnServer();
            RpcUse();
        }

        protected virtual void UseOnServer()
        {
            //_player.GetComponentInChildren<PlayerAnimationController>().SwingWeapon();
        }

        [ClientRpc]
        protected void RpcUse()
        {
            UseOnClient();
        }

        protected virtual void UseOnClient()
        {
            _player.GetComponentInChildren<PlayerAnimationController>().SwingWeapon();
            print("item used client");
        }
    }
}
