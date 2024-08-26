using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class NavMeshBaker : MonoBehaviour
{
    [SerializeField] private NavMeshSurface surface;
    public GameObject Enemy;

    [Button]
    public void Bake()
    {
        surface.BuildNavMesh();
    }
}
