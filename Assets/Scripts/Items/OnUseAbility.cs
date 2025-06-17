using Assets.Scripts.Items;
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

    public virtual void SetStateFromBundle(StateBundle bundle)
    {

    }

    public virtual StateBundle GetStateBundle()
    {
        return null;
    }
}
