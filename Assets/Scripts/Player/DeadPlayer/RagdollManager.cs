using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RagdollManager : MonoBehaviour
{
    [SerializeField] private Collider mainCollider;
    [SerializeField] private Rigidbody mainRb;
    [SerializeField] private Animator mainAnimator;

    [SerializeField] private List<Rigidbody> rbs;
    [SerializeField] private List<Collider> colliders;
    [SerializeField] private List<CharacterJoint> joints;
    void Start()
    {
        rbs = GetComponentsInChildren<Rigidbody>(includeInactive: true).ToList();
        colliders = GetComponentsInChildren<Collider>(includeInactive: true).ToList();
        joints = GetComponentsInChildren<CharacterJoint>(includeInactive: true).ToList();
    }

    [Button]
    public void EnableRagdollMode()
    {
        mainAnimator.enabled = false;
        mainRb.isKinematic = true;
        mainCollider.enabled = false;
        foreach (var rb in rbs)
        {
            rb.isKinematic = false;
        }
        foreach(var collider in colliders)
        {
            collider.enabled = true;
        }
    }

    [Button]
    public void DisableRagdollMode()
    {
        mainAnimator.enabled = true;
        mainRb.isKinematic = false;
        mainCollider.enabled = true;
        foreach (var rb in rbs)
        {
            rb.isKinematic = true;
        }
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }
    }
}
