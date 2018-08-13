using UnityEngine;
//マップ生成
public class MapGenerator : MonoBehaviour {
    private const int map_width = 5;
    private const int map_height = 5;
   
    [SerializeField]private Sprite spr;
    private GameObject[,] map_field;
    private float[] posx;
    void Awake()
    {
        for (int k = 0; k < map_height; k += 1)
        {
            for (int i = 0; i < map_width; i += 1)
            {
                new GameObject("map_field").AddComponent<SpriteRenderer>().sprite = spr;
                for (int a = 0; a < map_height * map_width; a += 1)
                {
                    posx[a] = map_field[i, k].transform.position.x;
                    if (a != 0) { posx[a] += 5; }
                }
            }
        }
        
    }

    void Start()
    {
        //for (int k = 0; k < map_height; k += 1)
        //{
        //    for (int i = 0; i < map_width; i += 1)
        //    {
        //        Instantiate(prefab);
        //        //map_field[i, k].sprite = spr;
        //        //map_field[i, k] = sprd;
        //    }
        //}
    }

    void Update()
    {
        //for (int k = 0; k < map_height; k += 1)
        //{
        //    for (int i = 0; i < map_width; i += 1)
        //    {
        //        Instantiate(prefab);
        //    }
        //}
    }

}
