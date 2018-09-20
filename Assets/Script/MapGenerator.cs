using UnityEngine;
//マップ生成
public class MapGenerator : MonoBehaviour {
    private const int map_width = 100;
    private const int map_height = 100;
    private const int min_room_width = 4;
    private const int min_room_height = 4;
    private const int max_room_width = 8;
    private const int max_room_height = 8;
    private const int min_room_amount = 5;
    private const int max_room_amount = 10;
    [SerializeField] private GameObject[] floorPrefab;
    [SerializeField] private GameObject[] wallPrefab;
    [SerializeField] private GameObject exitPrefab;
    private Transform mapHolder;
    #region マップ作成(32*32)
    //public void Awake()
    //{
    //    mapHolder = new GameObject("Map").transform;
    //    for (int x = -1; x < map_width + 1; x += 1)
    //    {
    //        for (int y = -1; y < map_height + 1; y += 1)
    //        {
    //            //床
    //            GameObject toInstantiate = floorPrefab[0]/*[Random.Range(0, floorPrefab.Length)]*/;
    //            //出口
    //            if (x == 5 && y == 5)
    //            {
    //                toInstantiate = exitPrefab;
    //            }
    //            //外壁
    //            if (x == -1 || x == map_width || y == -1 || y == map_height) 
    //            {
    //                toInstantiate = wallPrefab[0]/*[Random.Range(0, wallPrefab.Length)]*/;
    //            }
    //            GameObject instance = Instantiate(toInstantiate,
    //                                              new Vector2(x, y),
    //                                              Quaternion.identity,
    //                                              mapHolder) as GameObject;
    //            instance.transform.localScale = new Vector2(3,3);
    //        }
    //    }
    //}
    #endregion
    #region マップ生成
    private GameObject[,] map_field;
    
    void Awake()
    {
        mapHolder = new GameObject("Map").transform;
        InitializeMap();
        //部屋を作る
        int room_amount = Random.Range(min_room_amount, max_room_amount);
        for(int i = 0; i < room_amount; i += 1)
        {
            int room_height = Random.Range(min_room_height, max_room_height);
            int room_width = Random.Range(min_room_width, max_room_width);

        }
    }
    private void InitializeMap()
    {
        map_field = new GameObject[map_width, map_height];
        //一旦、すべて壁で初期化
        for (int y = 0; y < map_height; y += 1)
        {
            for (int x = 0; x < map_width; x += 1)
            {
                map_field[x, y] = wallPrefab[0];
                GameObject toInstantiate = wallPrefab[0];
                GameObject instance = Instantiate(map_field[x, y],
                                      new Vector2(x, y),
                                      Quaternion.identity,
                                      mapHolder) as GameObject;
                instance.transform.localScale = new Vector2(3, 3);
            }
        }
    }
    private void abc()
    {
        
    }
    #endregion


}
