using Mirror;
using System;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    [field: SerializeField] public float MaxHealth { get; private set; }

    [SyncVar(hook = nameof(HealthChangedHook))] private float _health; 
    public float Health { get => _health; set => _health = value; }

    public Action<float,float> HealthChangedEvent;

    public override void OnStartServer()
    {
        Health = MaxHealth;
    }

    private void HealthChangedHook(float oldHealth, float newHealth)
    {
        HealthChangedEvent?.Invoke(oldHealth, newHealth);
        if(oldHealth < newHealth)
        {
            OnHeal(oldHealth, newHealth);
        }
        else if(oldHealth > newHealth)
        {
            OnDamage(oldHealth, newHealth);
        }
    }

    private void OnDamage(float oldHealth, float newHealth)
    {

    }

    private void OnHeal(float oldHealth, float newHealth)
    {

    }
}
