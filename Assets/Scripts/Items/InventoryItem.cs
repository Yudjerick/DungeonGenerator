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
    public class InventoryItem: MonoBehaviour
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite InventoryPicture { get; private set; }

        public virtual void OnEquip()
        {

        }

        public virtual void OnUnEquip()
        {

        }
    }
}
