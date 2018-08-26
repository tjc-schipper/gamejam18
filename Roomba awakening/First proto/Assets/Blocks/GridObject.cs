using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlocksGame;

public class GridObject : MonoBehaviour
{

	[SerializeField]
	int height = 1;

	public GridPos pos;

	public int GetHeight()
	{
		return this.height;
	}

	public float GetSurfaceHeight()
	{
		return Root.instance.grid.refObject.transform.position.y + this.height;
	}

}
