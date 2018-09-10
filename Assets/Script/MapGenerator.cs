using UnityEngine;
//マップ生成
public class MapGenerator : MonoBehaviour {
    private const int map_width = 100;
    private const int map_height = 100;
    [SerializeField] private GameObject[] floorPrefab;
    [SerializeField] private GameObject[] wallPrefab;
    [SerializeField] private GameObject exitPrefab;
    private Transform mapHolder;
    //#region マップ作成(32*32)
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
    //#endregion
    #region マップ生成
    private GameObject[,] map_field;

    void Awake()
    {
        mapHolder = new GameObject("Map").transform;
        map_field = new GameObject[map_width,map_height];
        //一旦、すべて壁で初期化
        for (int y = 0; y < map_height; y += 1)
        {
            for (int x = 0; x < map_width; x += 1)
            {
                GameObject toInstantiate = wallPrefab[0];
                GameObject instance = Instantiate(toInstantiate,
                                      new Vector2(x, y),
                                      Quaternion.identity,
                                      mapHolder) as GameObject;
                instance.transform.localScale = new Vector2(3, 3);
            }
        }
    }
    #endregion
    

}
