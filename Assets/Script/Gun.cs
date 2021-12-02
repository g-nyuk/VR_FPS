using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    public Transform muzzle;
    public Projectile projectile;
    public float msBetweenShots = 100; //100ms
    public float muzzleVelocity = 35; //speed that bullet will leave the gun

    private Text roundInfo ;
    public int round = 30;
    float nextShotTime;

    void Start()
    {
        roundInfo = GameObject.Find("RoundInfo").GetComponent<Text>();
    }
    public void Shoot()
    {
        if(round > 0)
        {
            if (Time.time > nextShotTime)
            {
                nextShotTime = Time.time + msBetweenShots / 1000;
                Projectile newProjectile = Instantiate(projectile, muzzle.position, muzzle.rotation) as Projectile;
                newProjectile.Setspeed(muzzleVelocity);
                round--;
            }
        }
        else
        {
            Reload();
        }

    } 
    public void Reload()
    {
        Debug.Log("Reloading..");
        if(nextShotTime < Time.time)
        {
            nextShotTime += 2;
            round = 30;
        }
    }
    void Update()
    {
        roundInfo.text = "Round = " + round.ToString() + " / 30";
    }
}
