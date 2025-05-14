using Mirror;
using UnityEngine;

public class OnUseAbility : MonoBehaviour
{
    [Server]
    public virtual void OnUseButtonDownServer()
    {

    }

    [Server]
    public virtual void OnUseButtonUpServer()
    {

    }

    public virtual void OnUseButtonDownCLient()
    {

    }

    public virtual void OnUseButtonUpClient()
    {

    }
}
