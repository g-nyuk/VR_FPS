using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Wave[] waves;
    public Enemy enemy;

    Wave currentWave;
    int currentWaveNum;

    int enemyRemainingToSpawn;
    float nextSpawnTime;

    MapGenerator map;

    void Start(){
        map = FindObjectOfType<MapGenerator>();
        NextWave();
    }

    void Update(){
        if(enemyRemainingToSpawn >0 && Time.time > nextSpawnTime){
            enemyRemainingToSpawn--;
            nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

            StartCoroutine(SpawnEnemy());
        }
    }

    IEnumerator SpawnEnemy(){
        float spawnDelay = 5;
        float tileFlashSpeed = 4;

        Transform randomTile = map.GetRandomOpenTile();
        Material tileMat = randomTile.GetComponent<Renderer>().material;
        Color initialColor = tileMat.color;
        Color flashColor = Color.red;
        float spawnTimer = 0;

        while(spawnTimer < spawnDelay){
            tileMat.color = Color.Lerp(initialColor,flashColor, Mathf.PingPong(spawnTimer * tileFlashSpeed, 1));

            spawnTimer += Time.deltaTime;
            yield return null;
        }

        Enemy spawnedEnemy = Instantiate(enemy, randomTile.position + Vector3.up, Quaternion.identity) as Enemy;
    }

    void NextWave(){
        currentWaveNum ++;
        currentWave = waves[currentWaveNum-1];

        enemyRemainingToSpawn = currentWave.enemyCount;
    }

    [System.Serializable]
    public class Wave{
        public int enemyCount;
        public float timeBetweenSpawns;
    }

}
