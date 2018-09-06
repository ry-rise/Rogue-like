﻿using UnityEngine;
//マップ生成
public class MapGenerator : MonoBehaviour {
    private const int map_width = 8;
    private const int map_height = 8;
    [SerializeField] private GameObject[] floorPrefab;
    [SerializeField] private GameObject[] wallPrefab;
    [SerializeField] private GameObject exitPrefab;
    private Transform mapHolder;
    #region マップ作成(32*32)
    public void Awake()
    {
        mapHolder = new GameObject("Map").transform;
        for (int x = -1; x < map_width + 1; x += 1)
        {
            for (int y = -1; y < map_height + 1; y += 1)
            {
                //床
                GameObject toInstantiate = floorPrefab[0]/*[Random.Range(0, floorPrefab.Length)]*/;
                //出口
                if (x == 5 && y == 5)
                {
                    toInstantiate = exitPrefab;
                }
                //外壁
                if (x == -1 || x == map_width || y == -1 || y == map_height) 
                {
                    toInstantiate = wallPrefab[0]/*[Random.Range(0, wallPrefab.Length)]*/;
                }
                GameObject instance = Instantiate(toInstantiate,
                                                  new Vector2(x, y),
                                                  Quaternion.identity,
                                                  mapHolder) as GameObject;
                instance.transform.localScale = new Vector2(3,3);
            }
        }
    }
    #endregion
    #region マップ生成
    //[SerializeField]private Sprite spr;
    //private GameObject[,] map_field;

    //void Awake()
    //{
    //    //map_field = new GameObject[map_width,map_height];
    //    for (int k = 0; k < map_height; k += 1)
    //    {
    //        for (int i = 0; i < map_width; i += 1)
    //        {
    //            new GameObject("map_field").AddComponent<SpriteRenderer>().sprite = spr;
    //            //map_field[i, k].AddComponent<SpriteRenderer>().sprite = spr;
    //            //obj.transform.parent = transform;
    //            //obj.transform.position = new Vector2(i * 1.15F, -k * 1.15F);
    //            //map_field[i, k] = obj;
    //        }
    //    }
    //}
    #endregion
    

}
