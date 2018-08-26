using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlocksGame;
using DG.Tweening;

[RequireComponent(typeof(ClickTarget), typeof(Tower))]
public class DragTowers : MonoBehaviour
{

	private Tower tower;
	private ClickTarget target;

	[SerializeField]
	bool canDrag = true;    // Global setting!!
	private bool draggable = true;  // Current state!!

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
		Root.instance.grid.MoveObject(GetComponent<GridObject>(), this.tower.gridObject.pos);

		this.dragstate = DragStates.IDLE;
		this.target = GetComponent<ClickTarget>();
		this.target.OnDragStart += Target_OnDragStart;
		this.target.OnDragEnd += Target_OnDragEnd;
	}

	private void Tower_OnCharacterChanged(bool characterOnObject)
	{
		if (characterOnObject)
		{
			this.draggable = false;
		}
		else
		{
			this.draggable = this.canDrag;
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
		this.tower.gridObject.pos = targetPos;
		Vector3 worldPos;
		try
		{
			worldPos = Root.instance.grid.GridToWorldPos(targetPos);
			this.transform.DOMove(worldPos, 0.1f);
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

}