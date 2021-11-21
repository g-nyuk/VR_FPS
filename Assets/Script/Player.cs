using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Threading;

public class Player : MonoBehaviour
{
    public float movespeed = 5;
    private Rigidbody rb;  
    private GunController gunController;
    public Camera myCamera;
    PlayerController controller;
    private float movementX;
    private float movementY;

    public float rotSpeed = 100.0f;
    private bool clicked = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        myCamera = GetComponent<Camera>();
        gunController = GetComponent<GunController>();
    }

    void OnFire()
    {
        gunController.Shoot();
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }
   
    void FixedUpdate()
    {
        movespeed = movementX * rotSpeed;
        transform.Rotate(new Vector3(0, movespeed, 0));
        //Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        //rb.AddForce(movement * movespeed);
    }

}
