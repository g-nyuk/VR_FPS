using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public LayerMask collisionMask;
    float speed = 10;

    public void Setspeed(float newSpeed)
    {
        speed = newSpeed;
    }
    // void Start()
    // {
    //     transform.Rotate(90, 0, 0);
    // }

    void Update()
    {
        float moveDistance = speed * Time.deltaTime;
        //checkCollisions(moveDistance);
        transform.Translate(Vector3.forward * moveDistance);
        
    }

    void checkCollisions(float moveDistance)
    {
        Ray ray = new Ray(transform.position,transform.forward);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, moveDistance, collisionMask, QueryTriggerInteraction.Collide))
        {
            OnHitObject(hit);
        }
    }

    void OnHitObject(RaycastHit hit)
    {
        GameObject.Destroy(gameObject);
        hit.collider.gameObject.SetActive(false);
    }
}
