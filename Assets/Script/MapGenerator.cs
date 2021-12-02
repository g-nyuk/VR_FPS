using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Transform tilePrefab;
    public Transform obstaclePrefab;
    public Transform navmeshFloor;
    public Transform navmeshMaskPrefeb;
    public Vector2 mapSize;
    public Vector2 maxMapSize;
    public float minObstacleHeight;
    public float maxObstacleHeight;

    [Range(0,1)]
    public float outlinePercent;
    [Range(0,1)]
    public float obstaclePercent;

    public float tileSize;

    List<Coord> allTileCoords;
    Queue<Coord> shuffledTileCoords; // 장애물 생성을 위한 무작위 쿼드셔플
    Queue<Coord> shuffledOpenTileCoords; // 스파이크볼 랜덤 스폰을 위한 오픈쿼드 셔플
    Transform[,] tileMap;

    public int seed = 10;
    Coord mapCentre;

    void Start(){
        GenerateMap();
    }

    public void GenerateMap(){
        
        tileMap = new Transform[(int)mapSize.x,(int)mapSize.y];
        System.Random prng = new System.Random(seed); // 랜덤 높이를 위한 변수 생성
        GetComponent<BoxCollider>().size = new Vector3(mapSize.x * tileSize, .05f, mapSize.y *tileSize);

        //좌표들 생성
        allTileCoords = new List<Coord>();
        for( int x= 0; x<mapSize.x; x++){
            for(int y=0; y<mapSize.y; y++){
                allTileCoords.Add(new Coord(x,y));
            }
        }
        shuffledTileCoords = new Queue<Coord> (Utility.shuffleArray(allTileCoords.ToArray(), seed));
        mapCentre = new Coord((int)mapSize.x/2, (int)mapSize.y/2);

        //맵홀더 오브젝트 생성
        string holderName = "Generated Map"; 
        if(transform.Find(holderName)){
            DestroyImmediate(transform.Find(holderName).gameObject);
        }

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        //맵에 타일들 스폰
        for (int x=0; x<mapSize.x; x++){
            for(int y=0; y<mapSize.y; y++){
                Vector3 tilePosition = CoordToPosition(x,y);
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right*90)) as Transform;
                newTile.localScale = Vector3.one * (1-outlinePercent) * tileSize;
                newTile.parent = mapHolder;
                tileMap[x,y] = newTile;
            }
        }

        //맵에 장애물 스폰
        bool[,] obstacleMap = new bool[(int)mapSize.x,(int)mapSize.y];

        int obstacleCount = (int)(mapSize.x * mapSize.y *obstaclePercent);
        int currentObstacleCount = 0;
        List<Coord> allOpenCoords = new List<Coord> (allTileCoords);

        for(int i =0; i<obstacleCount; i++){
            Coord randomCoord = GetRandomCoord(); // 스파이크볼 랜덤스폰을 위해 전체타일에서 장애물 생성된 아이를 빼줄예정.
            obstacleMap[randomCoord.x,randomCoord.y] = true;
            currentObstacleCount ++;

            if(randomCoord != mapCentre && MapIsFullyAccessible(obstacleMap,currentObstacleCount)){
                float obstacleHeight = Mathf.Lerp(minObstacleHeight,maxObstacleHeight,(float)prng.NextDouble());
                Vector3 obstaclePosition = CoordToPosition(randomCoord.x, randomCoord.y);

                Transform newObstacle = Instantiate(obstaclePrefab, obstaclePosition + Vector3.up * obstacleHeight/2, Quaternion.identity) as Transform;
                newObstacle.parent = mapHolder;
                newObstacle.localScale = new Vector3 ((1-outlinePercent) * tileSize, obstacleHeight, (1-outlinePercent)*tileSize);

                allOpenCoords.Remove(randomCoord);//전체 타일에서 장애물 생성된 아이를 빼줌. 
            }
            else{
                obstacleMap[randomCoord.x,randomCoord.y] = false;
                currentObstacleCount --;
            }
        }

        shuffledOpenTileCoords = new Queue<Coord> (Utility.shuffleArray(allOpenCoords.ToArray(), seed)); // 오픈타일 중에서 랜덤으로 스파이크볼 스폰위치 생성

        //네브매쉬 마스크 적용
        Transform maskLeft = Instantiate(navmeshMaskPrefeb, Vector3.left * (mapSize.x + maxMapSize.x)/4 * tileSize, Quaternion.identity) as Transform;
        maskLeft.parent = mapHolder;
        maskLeft.localScale = new Vector3((maxMapSize.x - mapSize.x)/2,1,mapSize.y)*tileSize;

        Transform maskRight = Instantiate(navmeshMaskPrefeb, Vector3.right * (mapSize.x + maxMapSize.x)/4 * tileSize, Quaternion.identity) as Transform;
        maskRight.parent = mapHolder;
        maskRight.localScale = new Vector3((maxMapSize.x - mapSize.x)/2,1,mapSize.y)*tileSize;

        Transform maskTop = Instantiate(navmeshMaskPrefeb, Vector3.forward * (mapSize.y + maxMapSize.y)/4 * tileSize, Quaternion.identity) as Transform;
        maskTop.parent = mapHolder;
        maskTop.localScale = new Vector3(maxMapSize.x,1,(maxMapSize.y-mapSize.y)/2)*tileSize;

        Transform maskBottom = Instantiate(navmeshMaskPrefeb, Vector3.back * (mapSize.y + maxMapSize.y)/4 * tileSize, Quaternion.identity) as Transform;
        maskBottom.parent = mapHolder;
        maskBottom.localScale = new Vector3(maxMapSize.x,1,(maxMapSize.y-mapSize.y)/2)*tileSize;


        //Navmesh Floor 전체적용
        navmeshFloor.localScale = new Vector3(maxMapSize.x,maxMapSize.y)*tileSize;
    }

    bool MapIsFullyAccessible(bool[,] obstacleMap, int currentObstacleCount){
        bool[,] mapFlags = new bool[obstacleMap.GetLength(0),obstacleMap.GetLength(1)];
        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(mapCentre);
        mapFlags[mapCentre.x,mapCentre.y] = true;

        int accessibleTileCount = 1;

        while(queue.Count > 0){
            Coord tile = queue.Dequeue();

            for(int x= -1; x<= 1; x++){
                for(int y= -1; y<=1; y++){
                    int neighbourX = tile.x + x;
                    int neighbourY = tile.y + y;
                    if( x == 0 || y==0){
                        if(neighbourX >= 0 && neighbourX < obstacleMap.GetLength(0) && neighbourY >= 0 && neighbourY < obstacleMap.GetLength(1)){
                            if(!mapFlags[neighbourX,neighbourY] && !obstacleMap[neighbourX,neighbourY]){
                                mapFlags[neighbourX,neighbourY] = true;
                                queue.Enqueue(new Coord(neighbourX,neighbourY));
                                accessibleTileCount ++;
                            }
                        }
                    }
                }
            }
        }

        int targetAccessibleTileCount = (int)(mapSize.x * mapSize.y - currentObstacleCount);
        return targetAccessibleTileCount == accessibleTileCount;
    }

    Vector3 CoordToPosition(int x, int y){
        return new Vector3 (-mapSize.x/2 + 0.5f + x, 0, -mapSize.y/2 + 0.5f + y) * tileSize;
    }

    public Coord GetRandomCoord(){
        Coord randomCoord = shuffledTileCoords.Dequeue ();
        shuffledTileCoords.Enqueue(randomCoord);
        return randomCoord;
    }

    public Transform GetRandomOpenTile(){
        Coord randomCoord = shuffledOpenTileCoords.Dequeue ();
        shuffledOpenTileCoords.Enqueue(randomCoord);
        return tileMap[randomCoord.x,randomCoord.y];
    }

    public struct Coord{
        public int x;
        public int y;

        public Coord(int _x, int _y){
            x = _x;
            y = _y;
        }

        public static bool operator == (Coord c1, Coord c2){
            return c1.x == c2.x && c1.y == c2.y;
        }
        public static bool operator != (Coord c1, Coord c2){
            return !(c1 == c2);
        }
    }
    
}
