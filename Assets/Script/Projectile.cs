using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public LayerMask collisionMask;
    float speed = 100;

    public void Setspeed(float newSpeed)
    {
        speed = newSpeed;
    }
    void Start()
    {
        transform.Rotate(90, 0, 0);
    }

    void Update()
    {
        //스파이크볼과 맞았는지 체크
        float moveDistance = speed * Time.deltaTime;
        //checkCollisions(moveDistance);
        transform.Translate(Vector3.up * moveDistance);
        
    }

    //스파이크볼과 맞았는지 체크
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
