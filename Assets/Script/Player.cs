using System.Collections;
using UnityEngine;

[RequireComponent (typeof(PlayerController))] 
[RequireComponent (typeof(GunController))] 
public class Player : MonoBehaviour
{
    public float movespeed = 5;

    Camera viewCamera;
    PlayerController controller;
    GunController gunController;

    void Start()
    {
        controller = GetComponent<PlayerController>(); // pc attach to player
        gunController = GetComponent<GunController>();
        viewCamera = Camera.main;
    }

    void Update()
    {
        //Movement Input
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"),0,Input.GetAxisRaw("Vertical")); //GetAxisRaw : doesn't do any default smoothing
        Vector3 moveVelocity = moveInput.normalized * movespeed; // input + dircetion 
        controller.Move(moveVelocity);

        //Look Input
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition); // screen position to mouse 
        Plane groundPlane = new Plane(Vector3.up,Vector3.zero);
        float rayDistance;

        if(groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            //Debug.DrawLine(ray.origin, point, Color.red);
            controller.LookAt(point);
        }

        //Weapon input 
        if(Input.GetMouseButton(0))
        {
            gunController.Shoot();
        }
    }
}
