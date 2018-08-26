using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlocksGame;

public class DragField : MonoBehaviour
{

	public Transform refObject
	{
		get
		{
			return this.transform;
		}
	}

	private GridObject[,] grid;

	public float blockSize = 1f;
	public int width = 10;
	public int height = 10;

	void Awake()
	{
		this.grid = new GridObject[this.width, this.height];
	}

	private bool IsValidPos(GridPos pos)
	{
		return
			(pos.x < this.width && pos.z < this.height)
			&&
			(pos.x >= 0 && pos.z >= 0);
	}

	public bool IsEmpty(GridPos pos)
	{
		if (!IsValidPos(pos))
		{
			throw new System.ArgumentOutOfRangeException();
		}
		return (this.grid[pos.x, pos.z] == null);
	}

	public bool IsAdjacent(GridPos from, GridPos to)
	{
		return (Mathf.Abs(from.x - to.x) + Mathf.Abs(from.z - to.z)) == 1;
	}




	#region Tower manipulation

	public bool MoveObject(GridPos start, GridPos end)
	{
		if (!IsValidPos(start) || !IsValidPos(end))
		{
			throw new System.ArgumentOutOfRangeException();
		}

		// Check whether square is adjacent
		if (!IsAdjacent(start, end))
			return false;

		// Check whether squares are valid
		if (this.grid[end.x, end.z] != null)
			return false;
		if (this.grid[start.x, start.z] == null)
			return false;

		GridObject t = this.grid[start.x, start.z];
		this.grid[start.x, start.z] = null;
		this.grid[end.x, end.z] = t;

		// Let caller know it's okay to move. They handle movement themselves.
		return true;
	}

	public bool MoveObject(GridObject t, GridPos pos)
	{
		if (!IsValidPos(pos))
		{
			throw new System.ArgumentOutOfRangeException();
		}

		if (this.grid[pos.x, pos.z] != null)
			return false;

		this.grid[pos.x, pos.z] = t;
		return true;
	}

	public bool RemoveObject(GridPos pos)
	{
		if (!IsValidPos(pos))
		{
			throw new System.ArgumentOutOfRangeException();
		}

		GridObject t = GetObjectAt(pos);
		if (t != null)
		{
			this.grid[pos.x, pos.z] = null;
			return true;
		}
		else
			return false;
	}

	public GridObject GetObjectAt(GridPos pos)
	{
		if (!IsValidPos(pos))
		{
			throw new System.ArgumentOutOfRangeException();
		}

		return this.grid[pos.x, pos.z];
	}

	#endregion


	public Vector3 GridToWorldPos(GridPos pos)
	{
		if (!IsValidPos(pos))
		{
			throw new System.ArgumentOutOfRangeException();
		}

		return this.refObject.position + new Vector3(
			pos.x * this.blockSize,
			0f,
			pos.z * this.blockSize
			);
	}

	public GridPos WorldToGridPos(Vector3 worldPos)
	{
		Vector3 refPos = worldPos - this.refObject.position;
		int x = Mathf.RoundToInt(refPos.x / this.blockSize);
		int z = Mathf.RoundToInt(refPos.z / this.blockSize);

		GridPos gridPos = new GridPos(x, z);
		if (!IsValidPos(gridPos))
		{
			throw new System.ArgumentOutOfRangeException();
		}
		return gridPos;
	}

	public GridPos ScreenToGridPos(Vector3 screenPos)
	{
		return WorldToGridPos(ScreenToWorldPos(screenPos));
	}

	public Vector3 ScreenToWorldPos(Vector3 screenPos)
	{
		return Root.instance.topCam.ScreenToWorldPoint(screenPos);
	}


	public Vector3 GetCenterPos(bool includeHeight = true)
	{
		return this.refObject.position - new Vector3(0.5f, 0f, 0.5f) + new Vector3(
			(this.width * this.blockSize) / 2f,
			(includeHeight) ? 3 * this.blockSize : 0f,
			(this.height * this.blockSize) / 2f
			);
	}


	void OnDrawGizmos()
	{
		Gizmos.DrawCube(
			this.refObject.position + new Vector3(this.width / 2f, 0f, this.height / 2f) * this.blockSize - new Vector3(0.5f, 0f, 0.5f),
			new Vector3(this.width * this.blockSize, 0.1f, this.height * this.blockSize));
	}

}
