using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
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
    public int health = 10;
    private Text scoreText;

    private Text gameoverText;
    private Text healthText;
    private int score = 0;
    public float rotSpeed = 100.0f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        myCamera = GetComponent<Camera>();
        gunController = GetComponent<GunController>();
        scoreText = GameObject.Find("Score").GetComponent<Text>();
        gameoverText = GameObject.Find("GameOver").GetComponent<Text>();
        healthText = GameObject.Find("Health").GetComponent<Text>();
        healthText.text = "HP : " + health.ToString();
        Time.timeScale = 3;
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
    void OnFire()
    {
        gunController.Shoot();
    }
    void OnReload()
    {
        gunController.Reload();
    }
    public void addScore()
    {
        score++;
        scoreText.text = "Score : " + score.ToString();
        
    }
    void OnTriggerEnter(Collider other)
    {
       if(other.gameObject.CompareTag("Target"))
        {
            health--;
            
            healthText.text = "HP : " + health.ToString();
            other.gameObject.SetActive(false);
            if(health == 0)
            {
                gameoverText.text = "GAME OVER";
                Time.timeScale = 1;
            }

        }
        
    }
}