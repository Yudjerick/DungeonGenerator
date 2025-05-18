using Mirror;
using System;
using UnityEngine;

public class BaseHealth : NetworkBehaviour
{
    
    [field: SerializeField] public float MaxHealth { get; private set; }

    [SerializeField]
    [SyncVar(hook = nameof(HealthChangedHook))] protected float _health;
    public float Health { get => _health; set => _health = value; }

    public Action<float, float> HealthChangedEvent;

    private void HealthChangedHook(float oldHealth, float newHealth)
    {
        HealthChangedEvent?.Invoke(oldHealth, newHealth);
        if (oldHealth < newHealth)
        {
            OnHeal(oldHealth, newHealth);
        }
        else if (oldHealth > newHealth)
        {
            OnDamage(oldHealth, newHealth);
        }
    }

    public override void OnStartServer()
    {
        Health = MaxHealth;
    }

    [Command]
    public void CmdTakeDamage(float damage)
    {
        TakeDamageServer(damage);
    }

    [Server]
    public virtual void TakeDamageServer(float damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            _health = 0;
            DieServer();
        }
    }

    [Server]
    public virtual void DieServer()
    {
        NetworkServer.Destroy(gameObject);
    }
    protected virtual void OnDamage(float oldHealth, float newHealth)
    {

    }

    protected virtual void OnHeal(float oldHealth, float newHealth)
    {

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
