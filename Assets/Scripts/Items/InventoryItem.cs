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
        [SerializeField] protected Transform grabPoint;
        [field: SerializeField] public OnUseAbility Ability { get; private set; }
        public virtual void Init(EquipPointsProvider provider)
        {
            grabPoint.transform.parent = null;
            transform.SetParent(grabPoint);
            grabPoint.transform.parent = provider.rightHand;
            grabPoint.localPosition = Vector3.zero;
            grabPoint.localRotation = Quaternion.identity;

        }
        public virtual void OnEquip(){ }

        public virtual void UpdateShown()
        {
            gameObject.SetActive(true);
        }

        public virtual void UpdateHidden()
        {
            gameObject.SetActive(false);
        }
    }
}
