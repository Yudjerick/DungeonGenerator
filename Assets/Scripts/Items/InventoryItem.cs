using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class InventoryItem: MonoBehaviour, Interactable
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite InventoryPicture { get; private set; }

        public bool Interact(InteractionController controller)
        {
            var inventory = controller.gameObject.GetComponent<Inventory>();
            var result = inventory.AddItem(this);
            if (result)
            {
                transform.SetParent(controller.transform);
                gameObject.SetActive(false);
            }
            return result;
        }
    }
}
