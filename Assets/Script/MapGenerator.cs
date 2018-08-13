using UnityEngine;
//マップ生成
public class MapGenerator : MonoBehaviour {
    private const int map_width = 5;
    private const int map_height = 5;
   
    [SerializeField]private Sprite spr;
    private GameObject[,] map_field;

    void Awake()
    {
        //map_field = new GameObject[map_width,map_height];
        for (int k = 0; k < map_height; k += 1)
        {
            for (int i = 0; i < map_width; i += 1)
            {
                new GameObject("map_field").AddComponent<SpriteRenderer>().sprite = spr;
                //map_field[i, k].AddComponent<SpriteRenderer>().sprite = spr;
                //obj.transform.parent = transform;
                //obj.transform.position = new Vector2(i * 1.15F, -k * 1.15F);
                //map_field[i, k] = obj;
            }
        }
        
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

}
