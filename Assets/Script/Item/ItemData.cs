using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string ItemText { get; set; }
    public int Have { get; set; }
    public Sprite ItemImage { get; set; }
    public ItemData(int id,string name,string itemtext,int have,Sprite itemimage)
    {
        ID = id;
        Name = name;
        ItemText = itemtext;
        Have = have;
        ItemImage = itemimage;
    }
}
