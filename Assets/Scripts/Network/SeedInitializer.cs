using Mirror;
using UnityEngine;

public class SeedInitializer: NetworkBehaviour
{
    [SyncVar]
    [SerializeField] private int seed;

    public void GenerateSeed()
    {
        seed = Random.Range(0, int.MaxValue);
    }

    public void SetRandomInitialState()
    {
        Random.InitState(seed);
    }
}

