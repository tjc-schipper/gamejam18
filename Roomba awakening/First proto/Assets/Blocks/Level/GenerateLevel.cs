using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DragField))]
public class GenerateLevel : MonoBehaviour
{

	[SerializeField]
	GameObject prefab_FloorBlock;

	[SerializeField]
	GameObject prefab_WallBlock;

	private DragField grid;

	GameObject floor;
	GameObject wall;


	void Awake()
	{
		this.grid = GetComponent<DragField>();
		SpawnFloorBlocks();
		//SpawnWallBlocks();
	}


	private void SpawnFloorBlocks()
	{
		if (this.floor != null)
		{
			Destroy(this.floor);
		}
		this.floor = new GameObject("_FloorBlocks");

		for (int x = 0; x < this.grid.width; x++)
		{
			for (int z = 0; z < this.grid.height; z++)
			{
				GameObject newBlock = Instantiate(
					prefab_FloorBlock,
					new Vector3(
						x * this.grid.blockSize,
						0f,
						z * this.grid.blockSize),
					Quaternion.identity);
				newBlock.transform.SetParent(this.floor.transform);
			}
		}

		this.floor.transform.position = this.grid.refObject.position;
	}

	private void SpawnWallBlocks()
	{
		if (this.wall != null)
		{
			Destroy(this.wall);
		}
		this.wall = new GameObject("_WallBlocks");

		for (int x = 0; x < this.grid.width; x++)
		{
			for (int y = 0; y < 6; y++)
			{
				GameObject newBlock = Instantiate(
					prefab_WallBlock,
					new Vector3(
						x * this.grid.blockSize,
						y * this.grid.blockSize,
						0f
						),
					Quaternion.identity);
				newBlock.transform.SetParent(this.wall.transform);
			}
		}

		this.wall.transform.position = this.grid.refObject.transform.position + new Vector3(0f, 0f, (this.grid.height-0.5f) * this.grid.blockSize);
	}

}
