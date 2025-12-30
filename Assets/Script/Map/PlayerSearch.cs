using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSearch : MonoBehaviour 
{
	public class NodeA
	{

	}
	public int CalculateCost(Vector2 playetPos,Vector2 enemyPos)
	{
		int cost=Mathf.Abs((int)enemyPos.x - (int)playetPos.x) + 
		         Mathf.Abs((int)enemyPos.y - (int)playetPos.y);
		return cost;
	}
}
