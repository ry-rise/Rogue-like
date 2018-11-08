using UnityEngine;
//マップ生成
public class MapGenerator : MonoBehaviour
{
    #region 変数
    public int MapWidth { get; private set; } = 80;
    public int MapHeight { get; private set; } = 80;
    private const int minRoomWidth = 6;
    private const int minRoomHeight = 6;
    private const int maxRoomWidth = 10;
    private const int maxRoomHeight = 10;
    private const int minRoomAmount = 20;
    private const int maxRoomAmount = 25;
    private const int roadPoint = 1;
    //private const int wall = -1;
    //private const int road = 0;
    //private const int floor = 0;
    //private const int player = 1;
    public enum STATE
    {
        FLOOR,
        ROAD,
        PLAYER,
        ITEM,
        ENEMY,
        EXIT,
        WALL = -1
    }
    //public STATE state; 
    [SerializeField] private GameObject[] floorPrefab;
    [SerializeField] private GameObject[] wallPrefab;
    [SerializeField] private GameObject exitPrefab;
    private Transform mapHolder;
    #endregion
    #region マップ生成
    public int[,] mapStatus;
    private Player playerPos;
    private GameManager gameManager;
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        mapHolder = new GameObject("Map").transform;
        InitializeMap();
        RoomCreate();
        CreateDungeon();
    }
    private void Start()
    {
        playerPos = gameManager.playerObject.GetComponent<Player>();
    }

    private void InitializeMap()
    {
        mapStatus = new int[MapWidth, MapHeight];
        //一旦、すべて壁で初期化
        for (int y = 0; y < MapHeight; y += 1)
        {
            for (int x = 0; x < MapWidth; x += 1)
            {
                mapStatus[x, y] = (int)STATE.WALL;
            }
        }
    }

    private void RoomCreate()
    {
        //通路を作る
        int roomAmount = Random.Range(minRoomAmount, maxRoomAmount);
        int[] roadAggPointX = new int[roadPoint];
        int[] roadAggPointY = new int[roadPoint];
        for (int i = 0; i < roadAggPointX.Length; i += 1)
        {
            roadAggPointX[i] = Random.Range(1, MapWidth);
            roadAggPointY[i] = Random.Range(1, MapHeight);
            mapStatus[roadAggPointY[i], roadAggPointX[i]] = (int)MapGenerator.STATE.ROAD;
        }
        //部屋を作る
        for (int i = 0; i < roomAmount; i += 1)
        {
            int roomHeight = Random.Range(minRoomHeight, maxRoomHeight);
            int roomWidth = Random.Range(minRoomWidth, maxRoomWidth);
            int roomPointX = Random.Range(2, MapWidth - maxRoomWidth - 2);
            int roomPointY = Random.Range(2, MapHeight - maxRoomHeight - 2);
            int roadStartPointX = Random.Range(roomPointX, roomPointX + roomWidth);
            int roadStartPointY = Random.Range(roomPointY, roomPointY + roomHeight);
            bool roomCheck = CheckRoomCreate(roomWidth, roomHeight, roomPointX, roomPointY);
            if (roomCheck == false)
            {
                CreateRoad(roadStartPointX, roadStartPointY, roadAggPointX[Random.Range(0, 0)], roadAggPointY[Random.Range(0, 0)]);
            }
        }
    }

    private bool CheckRoomCreate(int roomWidth, int roomHeight, int roomPointX, int roomPointY)
    {
        bool createFloor = false;
        for (int y = 0; y < roomHeight; y += 1)
        {
            for (int x = 0; x < roomWidth; x += 1)
            {
                if (mapStatus[roomPointY + y, roomPointX + x] == (int)STATE.FLOOR)
                {
                    createFloor = true;
                }
                else
                {
                    mapStatus[roomPointY + y, roomPointX + x] = (int)STATE.FLOOR;
                }
            }
        }
        return createFloor;
    }
    private void CreateRoad(int roadStartPointX, int roadStartPointY, int meetPointX, int meetPointY)
    {
        bool isRight;
        if (roadStartPointX > meetPointX)
        {
            isRight = true;
        }
        else
        {
            isRight = false;
        }
        bool isUnder;
        if (roadStartPointY > meetPointY)
        {
            isUnder = false;
        }
        else
        {
            isUnder = true;
        }

        if (Random.Range(0, 2) == 0)
        {
            while (roadStartPointX != meetPointX)
            {
                mapStatus[roadStartPointY, roadStartPointX] = (int)STATE.ROAD;
                if (isRight == true)
                {
                    roadStartPointX--;
                }
                else
                {
                    roadStartPointX++;
                }
            }
            while (roadStartPointY != meetPointY)
            {
                mapStatus[roadStartPointY, roadStartPointX] = (int)STATE.ROAD;
                if (isUnder == true)
                {
                    roadStartPointY++;
                }
                else
                {
                    roadStartPointY--;
                }
            }
        }

        else
        {
            while (roadStartPointY != meetPointY)
            {
                mapStatus[roadStartPointY, roadStartPointX] = (int)STATE.ROAD;
                if (isUnder == true)
                {
                    roadStartPointY++;
                }
                else
                {
                    roadStartPointY--;
                }
            }
            while (roadStartPointX != meetPointX)
            {
                mapStatus[roadStartPointY, roadStartPointX] = (int)STATE.ROAD;
                if (isRight == true)
                {
                    roadStartPointX--;
                }
                else
                {
                    roadStartPointX++;
                }
            }
        }
    }
    private void CreateDungeon()
    {
        for (int y = 0; y < MapHeight; y += 1)
        {
            for (int x = 0; x < MapWidth; x += 1)
            {
                if (mapStatus[x, y] == (int)STATE.WALL)
                {
                    GameObject instance = Instantiate(wallPrefab[0],
                                                      new Vector2(x, y),
                                                      Quaternion.identity,
                                                      mapHolder) as GameObject;
                    instance.transform.localScale = new Vector2(3, 3);
                }
                else if (mapStatus[x, y] == (int)STATE.PLAYER)
                {
                    GameObject instance = Instantiate(floorPrefab[0],
                                                      new Vector2(x, y),
                                                      Quaternion.identity,
                                                      mapHolder) as GameObject;
                    instance.transform.localScale = new Vector2(3, 3);
                    playerPos.transform.position = new Vector2(x, y);
                }
                else
                {
                    GameObject instance = Instantiate(floorPrefab[0],
                                                      new Vector2(x, y),
                                                      Quaternion.identity,
                                                      mapHolder) as GameObject;
                    instance.transform.localScale = new Vector2(3, 3);
                }
            }
        }
    }
    #endregion
}
