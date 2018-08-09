using UnityEngine;

public class MapGenerater : MonoBehaviour {
    private const int map_width = 5;
    private const int map_height = 5;
    private Texture2D tex2;
    private Sprite[] spr;
    private SpriteRenderer[] sprd;
    private GameObject[,] map_field;
    [SerializeField]GameObject prefab;

    void Awake()
    {
        tex2 = new Texture2D(32, 32);
        int pluslength = 0;
        for (int k = 0; k < map_height; k += 1)
        {
            for (int i = 0; i < map_width; i += 1)
            {
                map_field = new GameObject[map_width, map_height];
                var obj = GetComponent<SpriteRenderer>();
                //obj.sprite = spr;
                spr[i] = Sprite.Create(tex2,
                                    new Rect(0, 0, tex2.width, tex2.height),
                                    new Vector2(0 + pluslength, 0 + pluslength),
                                    100.0f);
                sprd[i].sprite = spr[i];
                pluslength += 32;
            }
        }
    }

    void Start()
    {
        for (int k = 0; k < map_height; k += 1)
        {
            for (int i = 0; i < map_width; i += 1)
            {
                Instantiate(prefab);
                //map_field[i, k].sprite = spr;
                //map_field[i, k] = sprd;
            }
        }
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
