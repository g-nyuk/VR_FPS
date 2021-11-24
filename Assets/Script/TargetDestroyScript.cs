using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TargetDestroyScript : MonoBehaviour
{
    public int health = 4;

    private Player player;
    private int score = 0;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
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
                player.addScore();
            }
        }
        
    }
}
