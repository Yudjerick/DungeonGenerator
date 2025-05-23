using DungeonGeneration;
using Mirror;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TestCubeController : NetworkBehaviour
{
    [SerializeField] private DungeonGenerator generator;
    private Rigidbody rb;
    public float walkingSpeed = 7.5f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;

    void Start()
    {
        
        rb = GetComponent<Rigidbody>();
        generator = FindAnyObjectByType<DungeonGenerator>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Invoke("SetPos", 1f);
        if (!isLocalPlayer)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            rb.linearVelocity = (transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical")) * walkingSpeed + rb.linearVelocity.y * Vector3.up;

            // Player and Camera rotation
            if (canMove)
            {
                rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
            }
        }
        
    }

    [Button]
    void SetPos()
    {
        print("SetPos");
        transform.position = FindAnyObjectByType<NetworkStartPosition>().transform.position; 
        
    }
}
