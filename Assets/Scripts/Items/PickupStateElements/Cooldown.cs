using UnityEngine;

namespace Assets.Scripts.Items.PickupStateElements
{
    public class Cooldown : PickupStateElement
    {
        [SerializeField] private float _cooldown;
        
        public override StateBundle GetState(StateBundle initial)
        {
            var result = initial;
            if(result == null)
            {
                result = new StateBundle();
            }
            result.PutFloat("cooldown", _cooldown);
            return result;

        }

        public override void SetState(StateBundle state)
        {
            _cooldown = state.GetFloat("cooldown");
        }

        private void Update()
        {
            if( _cooldown > 0 )
            {
                _cooldown -= Time.deltaTime;
                if( _cooldown < 0 )
                {
                    _cooldown = 0;
                }
            }
        }
    }
}