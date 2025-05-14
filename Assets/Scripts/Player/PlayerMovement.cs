using DungeonGeneration;
using Mirror;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private DungeonGenerator generator;
    private Rigidbody rb;
    [SerializeField] private float walkingSpeed = 7.5f;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float crouchSpeed;
    private float _moveSpeed;

    [SerializeField] private float jumpSpeed;
    [SerializeField] private float jumpRequestMaxTime = 0.2f;
    [SerializeField] private Transform feetPosition;
    [SerializeField] private float groundCheckRadius = 0.05f;
    private bool _canJump = true;
    private bool _jumpRequested;
    private float _jumpRequestTimer;
    private int _maxGroundCheckOverlapColliders = 5;
    private Collider[] _groundCheckOverlapColliders;
    private Collider _playerCollider;

    [SerializeField] private Transform playerCamera;
    [SerializeField] private Transform handsIKPivot;
    [SerializeField] private float lookSpeed = 0.1f;
    [SerializeField] private float lookYLimit = 45.0f;

    private float _rotationY;

    

    [HideInInspector]
    public bool canMove = true;

    void Start()
    {
        

        rb = GetComponent<Rigidbody>();
        generator = FindAnyObjectByType<DungeonGenerator>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _moveSpeed = walkingSpeed;
        _groundCheckOverlapColliders = new Collider[_maxGroundCheckOverlapColliders];
        _playerCollider = GetComponent<Collider>();
    }

    public void HandleMovement(Vector2 movementInput, Vector2 lookInput)
    {
        //var movement = _moveAction.ReadValue<Vector2>();
        rb.linearVelocity = (transform.right * movementInput.x + transform.forward * movementInput.y) * _moveSpeed + rb.linearVelocity.y * Vector3.up;

        // Player and Camera rotation
        if (canMove)
        {
            //var lookInput = _lookAction.ReadValue<Vector2>();
            _rotationY += -lookInput.y * lookSpeed;
            _rotationY = Mathf.Clamp(_rotationY, -lookYLimit, lookYLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(_rotationY, 0, 0);
            handsIKPivot.localRotation = playerCamera.localRotation;
            transform.rotation *= Quaternion.Euler(0, lookInput.x * lookSpeed, 0);
        }

        CheckIfCanJump();
        if (_jumpRequested)
        {
            if (_canJump)
            {
                Jump();
            }
            else
            {
                _jumpRequestTimer -= Time.deltaTime;
                if (_jumpRequestTimer <= 0f)
                {
                    _jumpRequested = false;
                }
            }
        } 
    }

    private void CheckIfCanJump()
    {
        int hitsNum = Physics.OverlapSphereNonAlloc(feetPosition.position, groundCheckRadius, _groundCheckOverlapColliders);
        _canJump = false;
        for(int i = 0; i < hitsNum; i++)
        {
            if (_groundCheckOverlapColliders[i] != _playerCollider)
            {
                _canJump = true;
                return;
            }
        }
    }

    public void OnSprintButtonStateChanged(bool isPressed)
    {
        if(isPressed)
        {
            _moveSpeed = sprintSpeed;
        }
        else
        {
            _moveSpeed = walkingSpeed;
        }
    }

    public void OnJumpButtonPressed()
    {
        if (_canJump)
        {
            Jump();
        }
        else
        {
            _jumpRequested = true;
            _jumpRequestTimer = jumpRequestMaxTime;
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpSpeed, rb.linearVelocity.z);
        _jumpRequested = false;
        _jumpRequestTimer = 0f;
    }

    [Button]
    void SetPos()
    {
        print("SetPos");
        transform.position = FindAnyObjectByType<NetworkStartPosition>().transform.position;
    }
}
