using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMoveComponent : MonoBehaviour
{
    private GameObject _player;
    private NavMeshAgent _agent;
    private Rigidbody _rb;

    void Start()
    {
        
        _agent = GetComponent<NavMeshAgent>();
        _agent.enabled = false;
        _player = FindAnyObjectByType<TestCubeController>().gameObject;
        _rb = GetComponent<Rigidbody>();
        transform.position = _player.transform.position;
        _agent.enabled = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        //_rb.velocity = (_player.transform.position - transform.position).normalized;
        if(_player != null && _agent.isActiveAndEnabled)
        {
           _agent.SetDestination(_player.transform.position);
        }
        
    }
}
