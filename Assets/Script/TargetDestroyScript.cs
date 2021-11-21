using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDestroyScript : MonoBehaviour
{
    private int health = 5;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
       if(other.gameObject.CompareTag("Bullet"))
        {
            this.health--;
            other.gameObject.SetActive(false);
            if(this.health == 0)
            {
                this.gameObject.SetActive(false);   
            }
            
        }
    }
}
