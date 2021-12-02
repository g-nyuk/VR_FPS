using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (UnityEngine.AI.NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    UnityEngine.AI.NavMeshAgent pathfinder;
    Transform target;

    void Start()
    {
        pathfinder = GetComponent<UnityEngine.AI.NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어에게 무조건 Player 태그 필요

        StartCoroutine (UpdatePath());
    }
    
    //경로 탐색 프레임 단위 -> 시간단위(0.5초)
    IEnumerator UpdatePath(){
        float refreshRate = .5f; 

        while(target != null){
            Vector3 targetPosition = new Vector3(target.position.x, 0, target.position.z);
            pathfinder.SetDestination(targetPosition);
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
