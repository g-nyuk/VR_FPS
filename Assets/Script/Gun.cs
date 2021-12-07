using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class Gun : MonoBehaviour
{   
    public SteamVR_Action_Boolean m_FireAction = null; //Grab Pinch is the trigger, select from inspecter
    private SteamVR_Behaviour_Pose m_Pose = null;

    public AudioSource shootSound;

    public Transform muzzle;
    public Projectile projectile;
    public float msBetweenShots = 10; //100ms
    public float muzzleVelocity = 35; //speed that bullet will leave the gun

    private Text roundInfo ;
    public int round = 30;
    float nextShotTime;

    private void Awake()
    {
        m_Pose = GetComponentInParent<SteamVR_Behaviour_Pose>();
    }
    void Start()
    {
        roundInfo = GameObject.Find("RoundInfo").GetComponent<Text>();
    }
    public void Shoot()
    {
        if (Time.time > nextShotTime)
        {
            nextShotTime = Time.time + msBetweenShots / 1000;
            Projectile newProjectile = Instantiate(projectile, muzzle.position, muzzle.rotation) as Projectile;
            newProjectile.Setspeed(muzzleVelocity);
            shootSound.Play();
        }

    } 
    void Update()
    {
        if(m_FireAction.GetStateDown(m_Pose.inputSource))
            Shoot();
        
        roundInfo.text = "Bullet = " + round.ToString() + " / 30";
    }

}
