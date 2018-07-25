using UnityEngine;

public class Map : MonoBehaviour {
    private const int map_width = 5;
    private const int map_height = 5;
    private Texture2D tex2;
    private Sprite spr;
    private SpriteRenderer sprd;
    private GameObject[,] map_field;
    [SerializeField]GameObject prefab;

    void Awake()
    {
        tex2 = new Texture2D(100, 100);
        int pluslength = 0;
        for (int k = 0; k < map_height; k += 1)
        {
            for (int i = 0; i < map_width; i += 1)
            {
                spr = Sprite.Create(tex2,
                                    new Rect(0, 0, tex2.width, tex2.height),
                                    new Vector2(0 + pluslength, 0 + pluslength),
                                    100.0f);
                sprd.sprite = spr;
                pluslength += 100;
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
                //if ()
                //{

                //}
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
