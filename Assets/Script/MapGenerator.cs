﻿using UnityEngine;
//マップ生成
public class MapGenerator : MonoBehaviour {
    [SerializeField] private int map_width = 8;
    [SerializeField] private int map_height = 8;
    [SerializeField] private GameObject[] floorPrefab;
    [SerializeField] private GameObject[] wallPrefab;
    #region マップ作成
    private void Awake()
    {
        for(int x = -1; x < map_width; x += 1)
        {
            for(int y = -1; y < map_height; y += 1)
            {

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
    void Start()
    {
        
    }

    void Update()
    {
        
    }

}
