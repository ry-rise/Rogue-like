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
        tex2 = new Texture2D(30, 30);
        //sprd = gameObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
        sprd.color = new Color(0, 0, 0);
        int a = 0;
        for (int k = 0; k < map_height; k += 1)
        {
            for (int i = 0; i < map_width; i += 1)
            {

                spr = Sprite.Create(tex2,
                                    new Rect(0, 0, tex2.width, tex2.height),
                                    new Vector2(0 + a, 0),
                                    100.0f);
                sprd.sprite = spr;
                a += 5;
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
                //spr = Sprite.Create(tex2, 
                //                    new Rect(0, 0, tex2.width, tex2.height),
                //                    new Vector2(0 + a, 0), 
                //                    100.0f);
                //sprd.sprite = spr;
                //map_field[i, k].sprite = spr;
                //map_field[i, k] = sprd;
                //a += 5;

            }
        }
    }

    void Update()
    {
        
        
    }
}
