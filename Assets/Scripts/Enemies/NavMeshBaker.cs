using Mirror;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class NavMeshBaker : MonoBehaviour
{
    [SerializeField] private NavMeshSurface surface;
    [SerializeField] GameObject enemy;
    public GameObject Enemy;

    [Server]
    public void Bake()
    {
        surface.BuildNavMesh();
    }
}
