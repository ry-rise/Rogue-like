using UnityEngine;
//マップ生成
public class MapGenerator : MonoBehaviour
{
    #region 変数
    private const int map_width = 80;
    private const int map_height = 80;
    private const int min_room_width = 6;
    private const int min_room_height = 6;
    private const int max_room_width = 10;
    private const int max_room_height = 10;
    private const int min_room_amount = 20;
    private const int max_room_amount = 30;
    private const int road_point = 1;
    private const int wall = -1;
    private const int road = 0;
    private const int floor = 0;
    private const int player = 1;
    [SerializeField] private GameObject[] floorPrefab;
    [SerializeField] private GameObject[] wallPrefab;
    [SerializeField] private GameObject exitPrefab;
    private Transform mapHolder;
    #endregion
    #region マップ作成(32*32)
    ////public void Awake()
    ////{
    ////    mapHolder = new GameObject("Map").transform;
    ////    for (int x = -1; x < map_width + 1; x += 1)
    ////    {
    ////        for (int y = -1; y < map_height + 1; y += 1)
    ////        {
    ////            //床
    ////            GameObject toInstantiate = floorPrefab[0]/*[Random.Range(0, floorPrefab.Length)]*/;
    ////            //出口
    ////            if (x == 5 && y == 5)
    ////            {
    ////                toInstantiate = exitPrefab;
    ////            }
    ////            //外壁
    ////            if (x == -1 || x == map_width || y == -1 || y == map_height) 
    ////            {
    ////                toInstantiate = wallPrefab[0]/*[Random.Range(0, wallPrefab.Length)]*/;
    ////            }
    ////            GameObject instance = Instantiate(toInstantiate,
    ////                                              new Vector2(x, y),
    ////                                              Quaternion.identity,
    ////                                              mapHolder) as GameObject;
    ////            instance.transform.localScale = new Vector2(3,3);
    ////        }
    ////    }
    ////}
    #endregion
    #region マップ生成
    private int[,] map_status;
    private Player player_pos; 

    void Awake()
    {
        player_pos = GameObject.Find("Player").GetComponent<Player>();
        mapHolder = new GameObject("Map").transform;
        InitializeMap();
        RoomCreate();
        CreateDungeon();
    }

    private void InitializeMap()
    {
        map_status = new int[map_width, map_height];
        //一旦、すべて壁で初期化
        for (int y = 0; y < map_height; y += 1)
        {
            for (int x = 0; x < map_width; x += 1)
            {
                map_status[x, y] = wall;
                //GameObject toInstantiate = wallPrefab[0];
                //GameObject instance = Instantiate(toInstantiate,
                //                      new Vector2(x, y),
                //                      Quaternion.identity,
                //                      mapHolder) as GameObject;
                //instance.transform.localScale = new Vector2(3, 3);
            }
        }
    }

    private void RoomCreate()
    {
        //通路を作る
        int room_amount = Random.Range(min_room_amount, max_room_amount);
        int[] road_agg_pointX = new int[road_point];
        int[] road_agg_pointY = new int[road_point];
        for (int i = 0; i < road_agg_pointX.Length; i += 1)
        {
            road_agg_pointX[i] = Random.Range(1,map_width/*map_width / 3, map_width * 4 / 3*/);
            road_agg_pointY[i] = Random.Range(1,map_height/*map_height / 3, map_height * 4 / 3*/);
            map_status[road_agg_pointY[i], road_agg_pointX[i]] = road;
        }
        //部屋を作る
        for (int i = 0; i < room_amount; i += 1)
        {
            int room_height = Random.Range(min_room_height, max_room_height);
            int room_width = Random.Range(min_room_width, max_room_width);
            int room_pointX = Random.Range(2, map_width - max_room_width - 2);
            int room_pointY = Random.Range(2, map_height - max_room_height - 2);
            int road_start_pointX = Random.Range(room_pointX, room_pointX + room_width);
            int road_start_pointY = Random.Range(room_pointY, room_pointY + room_height);
            bool RoomCheck = CheckRoomCreate(room_width, room_height, room_pointX, room_pointY);
            if (RoomCheck == false)
            {
                CreateRoad(road_start_pointX, road_start_pointY, road_agg_pointX[Random.Range(0, 0)], road_agg_pointY[Random.Range(0, 0)]);
            }
        }
    }

    private bool CheckRoomCreate(int room_width, int room_height, int room_pointX, int room_pointY)
    {
        bool create_floor = false;
        for (int y = 0; y < room_height; y += 1)
        {
            for (int x = 0; x < room_width; x += 1)
            {
                if (map_status[room_pointY + y, room_pointX + x] == floor)
                {
                    create_floor = true;
                }
                else
                {
                    map_status[room_pointY + y, room_pointX + x] = floor;
                }
            }
        }
        return create_floor;
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
                map_status[roadStartPointY, roadStartPointX] = road;
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
                map_status[roadStartPointY, roadStartPointX] = road;
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
                map_status[roadStartPointY, roadStartPointX] = road;
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
                map_status[roadStartPointY, roadStartPointX] = road;
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
        for (int y = 0; y < map_height; y += 1)
        {
            for (int x = 0; x < map_width; x += 1)
            {
                if (map_status[x, y] == wall)
                {
                    GameObject instance = Instantiate(wallPrefab[0],
                                                      new Vector2(x, y),
                                                      Quaternion.identity,
                                                      mapHolder) as GameObject;
                    instance.transform.localScale = new Vector2(3, 3);
                }
                else if (map_status[x,y]==player)
                {
                    GameObject instance = Instantiate(floorPrefab[0],
                                                      new Vector2(x, y),
                                                      Quaternion.identity,
                                                      mapHolder) as GameObject;
                    instance.transform.localScale = new Vector2(3, 3);
                    player_pos.transform.position = new Vector2(x, y);
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
