using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlocksGame;
using DG.Tweening;

[RequireComponent(typeof(ClickTarget), typeof(Tower))]
public class DragTowers : MonoBehaviour
{

	[SerializeField]
	AudioClip clip_Drag;

	[SerializeField]
	AudioClip clip_Snap;


	private Tower tower;
	private ClickTarget target;

	[SerializeField]
	bool canDrag = true;    // Global setting!!
	private bool draggable = true;  // Current state!!
	private bool colorAllowed = true;




	private bool colorGrayed = false;









	private enum DragStates
	{
		IDLE = 0,
		DRAGGING
	}
	private DragStates dragstate;

	void Start()
	{
		this.tower = GetComponent<Tower>();
		this.tower.OnCharacterChanged += Tower_OnCharacterChanged;
		this.draggable = this.canDrag;

		this.tower.gridObject.pos = Root.instance.grid.WorldToGridPos(this.transform.position);
		Root.instance.grid.AddObject(GetComponent<GridObject>(), this.tower.gridObject.pos);

		this.dragstate = DragStates.IDLE;
		this.target = GetComponent<ClickTarget>();
		this.target.OnDragStart += Target_OnDragStart;
		this.target.OnDragEnd += Target_OnDragEnd;

		Root.instance.grid.TowerColorChanged += Grid_TowerColorChanged;
	}

	private void Grid_TowerColorChanged(Tower.TowerColor color)
	{
		if (color == Tower.TowerColor.NEUTRAL)
		{
			this.colorAllowed = true;
		}
		else
		{
			this.colorAllowed = (color == this.tower.towerColor);
		}

		if (this.colorAllowed || this.tower.towerColor == Tower.TowerColor.NEUTRAL)
			NormalColor();
		else
			GrayColor();

		this.draggable = this.canDrag && !this.tower.HasCharacter() && this.colorAllowed;
	}

	private void Tower_OnCharacterChanged(Tower tower, bool characterOnObject)
	{
		if (characterOnObject)
		{
			this.draggable = false;
		}
		else
		{
			this.draggable = this.canDrag && !this.tower.HasCharacter() && this.colorAllowed;
		}
	}

	void Update()
	{
		if (!draggable) return;
		if (this.dragstate == DragStates.DRAGGING)
		{
			GridPos snapPos = null;
			try
			{
				snapPos = Root.instance.grid.ScreenToGridPos(Input.mousePosition);
			}
			catch (System.Exception e) { }

			// If we have a valid snapPos, move it there
			if (snapPos != null)
			{
				if (Root.instance.grid.MoveObject(this.tower.gridObject.pos, snapPos))
				{
					DoMove(snapPos);
				}
			}

		}
	}

	//TODO: Check if this doesn't cause bugs!
	private void Target_OnDragStart(Vector3 mousePos, GameObject target)
	{
		/*if (!this.draggable)
		{
			Debug.LogWarning("Tried dragging but draggable: " + this.draggable);
			return;
		}*/
		this.dragstate = DragStates.DRAGGING;
	}

	private void Target_OnDragEnd(Vector3 mousePos, GameObject target)
	{
		/*if (!this.draggable)
		{
			Debug.LogWarning("Tried dragging but draggable: " + this.draggable);
			return;
		}*/
		this.dragstate = DragStates.IDLE;
	}


	private void DoMove(GridPos targetPos)
	{
		AudioSource.PlayClipAtPoint(this.clip_Drag, Vector3.zero);
		this.tower.gridObject.pos = targetPos;
		Vector3 worldPos;
		try
		{
			worldPos = Root.instance.grid.GridToWorldPos(targetPos);
			this.transform.DOMove(worldPos, 0.1f).OnComplete(() =>
			{
				AudioSource.PlayClipAtPoint(this.clip_Snap, Vector3.zero);
			});
		}
		catch (System.Exception e) { }
	}


	private void OnDrawGizmos()
	{
		if (Application.isPlaying)
		{
			try
			{
				Vector3 calcPos = Root.instance.grid.GridToWorldPos(this.tower.gridObject.pos);
				Gizmos.color = (this.dragstate == DragStates.DRAGGING) ? Color.red : Color.white;
				Gizmos.DrawWireCube(calcPos, Vector3.one * 0.5f);
			}
			catch (System.Exception e) { }
		}
	}





	private void GrayColor()
	{
		if (this.colorGrayed == false)
		{
			GetComponent<Renderer>().material.DOColor(Color.gray, 0.25f);
			this.colorGrayed = true;
		}
	}

	private void NormalColor()
	{
		if (this.colorGrayed == true)
		{
			GetComponent<Renderer>().material.DOColor(Color.white, 0.25f);
			this.colorGrayed = false;
		}
	}

}