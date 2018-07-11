using UnityEngine;

public class Map : MonoBehaviour {
    private const int map_width = 10;
    private const int map_height = 10;
    private Texture2D tex2;
    private Sprite spr;
    private SpriteRenderer sprd;
    GameObject[,] map_field;

    void Awake()
    {
        tex2 = new Texture2D(30, 30);
        map_field = new GameObject[map_width, map_height];
        sprd = gameObject.AddComponent<SpriteRenderer>();
        sprd.color = new Color(0, 0, 0);
        
    }

    void Start () {
        
	}

    void Update()
    {
        int a = 0;
        for (int k = 0; k < map_field.GetLength(0); k += 1)
        {
            for (int i = 0; i < map_field.GetLength(0); i += 1)
            {
                spr = Sprite.Create(tex2, new Rect(0, 0, tex2.width, tex2.height),
                                    new Vector2(0 + a, 0), 100.0f);
                //sprd.sprite = spr;
                //map_field[i, k].sprite = spr;
                //map_field[i, k] = sprd;
                //a += 5;

            }
        }
    }
}
