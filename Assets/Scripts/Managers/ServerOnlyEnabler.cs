using Mirror;
using UnityEngine;

public class ServerOnlyEnabler: NetworkBehaviour
{
    [SerializeField] private MonoBehaviour script;

    public override void OnStartClient()
    {
        if(script is null)
        {
            return;
        }
        if(isClientOnly)
        {
            script.enabled = false;
        }
    }
}
