using Mirror;
using NaughtyAttributes;
using UnityEngine;

public class DeadPlayerController : NetworkBehaviour
{
    [SerializeField] private Transform camera;
    [SerializeField] private GameObject observedPlayer;
    [SerializeField] private PlayerHealth observedPlayerHealth;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextPlayerView();
        }
    }

    public void NextPlayerView()
    {
        var players = AliveManager.instance.AlivePlayers;
        var idx = AliveManager.instance.AlivePlayers.IndexOf(observedPlayer);
        if (observedPlayerHealth != null)
        {
            observedPlayerHealth.DieEvent -= OnObservedDie;
        }
        if (idx == players.Count - 1)
        {
            StartObserveSelfCorpse();
        }
        else
        {
            observedPlayer = players[idx + 1];
            observedPlayerHealth = observedPlayer.GetComponent<PlayerHealth>();
            observedPlayerHealth.DieEvent += OnObservedDie;
            camera.transform.SetParent(observedPlayer.transform.GetChild(0), false);
        }
        
    }
    
    private void OnObservedDie()
    {
        if (observedPlayerHealth != null)
        {
            observedPlayerHealth.DieEvent -= OnObservedDie;
        }
        StartObserveSelfCorpse();
    }

    private void StartObserveSelfCorpse()
    {
        camera.SetParent(transform);
        camera.localPosition = Vector3.zero;
        camera.localRotation = Quaternion.identity;
        observedPlayer = null;
        observedPlayerHealth = null;
    }


    public override void OnStartClient()
    {
        base.OnStartClient();
    }
}
