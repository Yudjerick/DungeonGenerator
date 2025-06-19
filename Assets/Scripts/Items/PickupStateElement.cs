using UnityEngine;

namespace Assets.Scripts.Items
{
    public abstract class PickupStateElement: MonoBehaviour
    {
        public abstract StateBundle GetState(StateBundle initial);

        public abstract void SetState(StateBundle state);
    }
}