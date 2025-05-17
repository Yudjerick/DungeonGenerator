using Mirror;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class HeadIKSettings : NetworkBehaviour
{
    [SerializeField] private MultiPositionConstraint constraint;
    public override void OnStartClient()
    {
        if(!isLocalPlayer)
        {
            constraint.weight = 0f;
        }
        Destroy(this);
    }
}
