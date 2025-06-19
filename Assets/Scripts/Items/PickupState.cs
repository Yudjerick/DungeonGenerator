using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class PickupState : MonoBehaviour
    {
        [SerializeField] private List<PickupStateElement> elements;

        public StateBundle GetStateBundle()
        {
            var bundle = new StateBundle();
            foreach (var element in elements)
            {
                element.GetState(bundle);
            }
            return bundle;
        }

        public void SetStateFromBundle(StateBundle bundle)
        {
            foreach (var element in elements)
            {
                element.SetState(bundle);
            }
        }
    }
}