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
    public Action DieEvent;

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

    [Command]
    public void CmdTakeDamage(float damage)
    {
        TakeDamageServer(damage);
    }

    [Server]
    public void TakeDamageServer(float damage)
    {
        Health -= damage;
        if(Health <= 0)
        {
            _health = 0;
            DieServer();
        }
    }

    [Command]
    private void CmdDie()
    {
        DieServer();
    }

    public void DieServer()
    {
        NetworkConnectionToClient connection = connectionToClient;
        print(AliveManager.instance);
        AliveManager.instance.AlivePlayers.Remove(gameObject);
        GameObject newPlayer = Instantiate(deadPlayer, transform.position, transform.rotation);
        AliveManager.instance.DeadPlayers.Add(newPlayer); 
        NetworkServer.ReplacePlayerForConnection(connection, newPlayer, ReplacePlayerOptions.Destroy);
    }

    private void OnDisable()
    {
        DieEvent?.Invoke();
    } 

    private void OnDamage(float oldHealth, float newHealth)
    {

    }

    private void OnHeal(float oldHealth, float newHealth)
    {

    }
}
