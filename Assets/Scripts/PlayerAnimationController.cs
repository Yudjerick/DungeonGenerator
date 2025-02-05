using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : NetworkBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator animator;
    private bool _isRunning = false;
    private void Update()
    {
        if (isLocalPlayer)
        {
            if(rb.linearVelocity.magnitude > 0.05f && !_isRunning)
            {
                animator.SetBool("isRunning", true);
                _isRunning = true;
            }
            else if(rb.linearVelocity.magnitude <= 0.05f && _isRunning)
            {
                animator.SetBool("isRunning", false);
                _isRunning = false;
            }
            
        }
        
    }
}
