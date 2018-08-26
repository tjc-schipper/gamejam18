using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridObject))]
public class Tower : MonoBehaviour
{

	public delegate void CharacterEvent(bool characterOnObject);
	public event CharacterEvent OnCharacterChanged;


	[HideInInspector]
	public GridObject gridObject;

	[SerializeField]
	bool walkable = true;

	public enum TowerTypes
	{
		STANDARD = 0,
		TUNNEL
	}
	public TowerTypes towerType = TowerTypes.STANDARD;

	private Character characterOnSurface;

	void Awake()
	{
		this.gridObject = GetComponent<GridObject>();
	}

	public Vector3 GetSurfaceWorldPos()
	{
		try
		{
			Vector3 p = Root.instance.grid.GridToWorldPos(this.gridObject.pos);
			p.y = this.gridObject.GetSurfaceHeight();
			return p;
		}
		catch (System.Exception e)
		{
			return Vector3.zero;
		}
	}


	public bool RegisterCharacter(Character c)
	{
		if (this.characterOnSurface != null)
			return false;
		else
		{
			this.characterOnSurface = c;
			if (this.OnCharacterChanged != null) this.OnCharacterChanged(true);
			return true;
		}
	}

	public bool DeregisterCharacter()
	{
		if (this.characterOnSurface == null)
			return false;
		else
		{
			this.characterOnSurface = null;
			if (this.OnCharacterChanged != null) this.OnCharacterChanged(false);
			return true;
		}
	}
	
	public bool IsWalkable()
	{
		return (this.walkable && this.characterOnSurface == null);
	}

}
