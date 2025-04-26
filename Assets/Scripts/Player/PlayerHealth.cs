using Mirror;
using NaughtyAttributes;
using System;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] private GameObject deadPlayer;
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

    public override void OnStartClient()
    {
        Invoke(nameof(CmdDie), 5f);
    }

    public void Die()
    {
        CmdDie();
    }

    [Command]
    private void CmdDie()
    {
        NetworkConnectionToClient connection = connectionToClient;
        GameObject newPlayer = Instantiate(deadPlayer, transform.position, transform.rotation);
        NetworkServer.ReplacePlayerForConnection(connection, newPlayer, ReplacePlayerOptions.Destroy);
    }

    private void OnDamage(float oldHealth, float newHealth)
    {

    }

    private void OnHeal(float oldHealth, float newHealth)
    {

    }
}
