using System.Linq;
using UnityEngine;
//マップ生成
public sealed class MapGenerator : MonoBehaviour
{
    #region 変数
    [SerializeField] private GameObject[] floorPrefab;
    [SerializeField] private GameObject[] wallPrefab;
    [SerializeField] private GameObject poisonPrefab;
    [SerializeField] private GameObject exitPrefab;
    public int MapWidth { get; private set; } = 80;
    public int MapHeight { get; private set; } = 80;
    public int MinRoomWidth { get; private set; } = 6;
    public int MinRoomHeight { get; private set; } = 6;
    public int MaxRoomWidth { get; private set; } = 10;
    public int MaxRoomHeight { get; private set; } = 10;
    public int MinRoomAmount { get; private set; } = 20;
    public int MaxRoomAmount { get; private set; } = 25;
    public int RoadPoint { get; private set; } = 1;
    private int RandomX;
    private int RandomY;
    public enum STATE
    {
        //NONE,
        FLOOR,
        ROAD,
        PLAYER,
        ITEM,
        ENEMY,
        TRAP_POISON,
        EXIT,
        WALL = -1
    }

    
    private Transform mapHolder;
    public int[,] MapStatusType;
    //public int[,] MapStatusRoom;
    //public int[,] MapStatusTrap;
    //private Player playerPos;
    private GameManager gameManager; 
    #endregion
    #region マップ生成
    public void Awake()
    {
        gameManager = GameObject.Find("Manager").GetComponent<GameManager>();
        mapHolder = new GameObject("Map").transform;
        InitializeMap();
        RoomCreate();
        CreateDungeon();
    }
    private void Start()
    {
        //playerPos = gameManager.playerObject.GetComponent<Player>();
    }

    public void InitializeMap()
    {
        MapStatusType = new int[MapWidth, MapHeight];
        //MapStatusTrap = new int[MapWidth, MapHeight];
        //MapStatusRoom = new int[MapWidth, MapHeight];
        //一旦、すべて壁で初期化
        for (int y = 0; y < MapHeight; y += 1)
        {
            for (int x = 0; x < MapWidth; x += 1)
            {
                MapStatusType[x, y] = (int)STATE.WALL;
                //MapStatusTrap[x, y] = (int)STATE.NONE;
                //MapStatusRoom[x, y] = (int)STATE.WALL;
            }
        }
    }

    public void RoomCreate()
    {
        //通路を作る
        int roomAmount = Random.Range(MinRoomAmount, MaxRoomAmount);
        int[] roadAggPointX = new int[RoadPoint];
        int[] roadAggPointY = new int[RoadPoint];
        foreach(int i in roadAggPointX)
        //for (int i = 0; i < roadAggPointX.Length; i += 1)
        {
            roadAggPointX[i] = Random.Range(1, MapWidth);
            roadAggPointY[i] = Random.Range(1, MapHeight);
            //XYを反対に
            MapStatusType[roadAggPointX[i], roadAggPointY[i]] = (int)STATE.ROAD;
        }
        //部屋を作る
        foreach(int i in Enumerable.Range(0,roomAmount))
        //for (int i = 0; i < roomAmount; i += 1)
        {
            int roomHeight = Random.Range(MinRoomHeight, MaxRoomHeight);
            int roomWidth = Random.Range(MinRoomWidth, MaxRoomWidth);
            int roomPointX = Random.Range(2, MapWidth - MaxRoomWidth - 2);
            int roomPointY = Random.Range(2, MapHeight - MaxRoomHeight - 2);
            int roadStartPointX = Random.Range(roomPointX, roomPointX + roomWidth);
            int roadStartPointY = Random.Range(roomPointY, roomPointY + roomHeight);
            bool roomCheck = CheckRoomCreate(roomWidth, roomHeight, roomPointX, roomPointY);
            if (roomCheck == false)
            {
                CreateRoad(roadStartPointX, roadStartPointY, roadAggPointX[Random.Range(0, 0)], roadAggPointY[Random.Range(0, 0)]);
            }
        }
        //FLOORのところにExitをランダムで配置
        while (true)
        {
            RandomX = Random.Range(0, MapWidth);
            RandomY = Random.Range(0, MapHeight);
            if (MapStatusType[RandomX, RandomY] == (int)STATE.FLOOR)
            {
                MapStatusType[RandomX, RandomY] = (int)STATE.EXIT;
                break;
            }
            else
            {
                continue;
            }
        }
        //FLOORのところにTrapをランダムで配置
        while (true)
        {
            RandomX = Random.Range(0, MapWidth);
            RandomY = Random.Range(0, MapHeight);
            if (MapStatusType[RandomX, RandomY] == (int)STATE.FLOOR)
            {
                //MapStatusTrap[RandomX, RandomY] = (int)STATE.TRAP_POISON;
                MapStatusType[RandomX, RandomY] = (int)STATE.TRAP_POISON;
                break;
            }
            else
            {
                continue;
            }
        }
    }
    #region XYを反対に
    private bool CheckRoomCreate(int roomWidth, int roomHeight, int roomPointX, int roomPointY)
    {
        bool createFloor = false;
        for (int y = 0; y < roomHeight; y += 1)
        {
            for (int x = 0; x < roomWidth; x += 1)
            {
                if (MapStatusType[ roomPointX + x,roomPointY + y] == (int)STATE.FLOOR)
                {
                    createFloor = true;
                }
                else
                {
                    MapStatusType[ roomPointX + x,roomPointY + y] = (int)STATE.FLOOR;
                    //MapStatusRoom[ roomPointX + x,roomPointY + y] = x;
                }
            }
        }
        return createFloor;
    }
    private void CreateRoad(int roadStartPointX, int roadStartPointY, int meetPointX, int meetPointY)
    {
        bool isRight;
        if (roadStartPointX > meetPointX) { isRight = true; }
        else { isRight = false; }
        bool isUnder;
        if (roadStartPointY > meetPointY) { isUnder = false; }
        else { isUnder = true; }

        if (Random.Range(0, 2) == 0)
        {
            while (roadStartPointX != meetPointX)
            {
                MapStatusType[roadStartPointX, roadStartPointY] = (int)STATE.ROAD;
                if (isRight == true) { roadStartPointX -= 1; }
                else { roadStartPointX += 1; }
            }
            while (roadStartPointY != meetPointY)
            {
                MapStatusType[roadStartPointX, roadStartPointY] = (int)STATE.ROAD;
                if (isUnder == true) { roadStartPointY += 1; }
                else { roadStartPointY -= 1; }
            }
        }

        else
        {
            while (roadStartPointY != meetPointY)
            {
                MapStatusType[roadStartPointX, roadStartPointY] = (int)STATE.ROAD;
                if (isUnder == true) { roadStartPointY += 1; }
                else { roadStartPointY -= 1; }
            }
            while (roadStartPointX != meetPointX)
            {
                MapStatusType[roadStartPointX, roadStartPointY] = (int)STATE.ROAD;
                if (isRight == true) { roadStartPointX -= 1; }
                else { roadStartPointX += 1; }
            }
        }
    }
    #endregion
    public void CreateDungeon()
    {
        for (int y = 0; y < MapHeight; y += 1)
        {
            for (int x = 0; x < MapWidth; x += 1)
            {
                if (MapStatusType[x, y] == (int)STATE.WALL)
                {
                    GameObject instance = Instantiate(wallPrefab[0],
                                                      new Vector2(x, y),
                                                      Quaternion.identity,
                                                      mapHolder) as GameObject;
                }
                else if (MapStatusType[x, y] == (int)STATE.EXIT)
                {
                    GameObject instance = Instantiate(exitPrefab,
                                                      new Vector2(x, y),
                                                      Quaternion.identity,
                                                      mapHolder) as GameObject;
                }
                else if(MapStatusType[x,y]==(int)STATE.TRAP_POISON)
                {
                    GameObject instance = Instantiate(poisonPrefab,
                                                    new Vector2(x, y),
                                                    Quaternion.identity,
                                                    mapHolder) as GameObject;
                }
                else if(MapStatusType[x,y]==(int)STATE.FLOOR)
                {
                    GameObject instance = Instantiate(floorPrefab[0],
                                                      new Vector2(x, y),
                                                      Quaternion.identity,
                                                      mapHolder) as GameObject;
                }
                else if(MapStatusType[x,y]==(int)STATE.ROAD)
                {
                    GameObject instance = Instantiate(floorPrefab[0],
                                                      new Vector2(x, y),
                                                      Quaternion.identity,
                                                      mapHolder) as GameObject;
                }
                else { continue; }
            }
        }
    }
    #endregion
}
