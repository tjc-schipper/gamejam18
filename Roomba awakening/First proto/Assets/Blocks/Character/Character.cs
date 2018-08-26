using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlocksGame;
using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;

[RequireComponent(typeof(GridObject))]
public class Character : MonoBehaviour
{

	#region Audio
	[SerializeField]
	AudioClip clip_Dash;

	[SerializeField]
	AudioClip[] clip_Jump;

	[SerializeField]
	AudioClip[] clip_Nope;

	#endregion



	public delegate void AnimationEvent();
	public event AnimationEvent OnJump;
	public event AnimationEvent OnLand;

	private const float JUMP_DURATION = 0.5f;
	private const float JUMP_HEIGHT = 1.5f;
	private const float JUMP_WINDUP_DELAY = 0.2f;
	private const float HOP_DURATION = 0.5f;

	[SerializeField]
	KeyCode jumpKey;

	[SerializeField]
	KeyCode turnKey;


	[SerializeField]
	MovementRules movementRules;

	[SerializeField]
	private Animator[] animators;

	private enum Facings
	{
		LEFT = 0,
		BACK = 1,
		RIGHT = 2,
		FRONT = 3
	}
	private Facings facing = Facings.FRONT;

	private readonly GridPos[] directions = new GridPos[]
	{
		new GridPos(-1, 0),
		new GridPos(0, 1),
		new GridPos(1, 0),
		new GridPos(0, -1)
	};

	private GridObject selfGridObject;

	private Tower tower;

	private bool grounded = true;


	public int GetCharacterHeight()
	{
		return this.selfGridObject.GetHeight();
	}

	public void SetGrounded(bool g)
	{
		this.grounded = g;
		foreach (Animator a in this.animators)
		{
			a.SetBool("b_grounded", g);
		}
	}


	void Awake()
	{
		this.selfGridObject = GetComponent<GridObject>();
	}

	void Start()
	{
		int facingIndex = (int)this.facing;
		for (int i = 0; i < this.animators.Length; i++)
		{
			this.animators[i].gameObject.SetActive(i == facingIndex);
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(jumpKey))
		{
			if (this.grounded)
				Jump();
		}

		if (Input.GetKeyDown(turnKey))
		{
			DoTurn();
		}
	}

	void OnDestroy()
	{
		if (this.tower != null)
		{
			this.tower.DeregisterCharacter();
		}
	}

	public void Spawn(GridPos pos)
	{
		this.selfGridObject.pos = pos;
		this.tower = Root.instance.grid.GetObjectAt(this.selfGridObject.pos).GetComponent<Tower>();
		this.selfGridObject.SetHeight(this.tower.gridObject.GetHeight());

		Vector3 worldPos;
		try
		{
			worldPos = this.tower.GetSurfaceWorldPos();
			this.transform.position = worldPos;
		}
		catch (System.Exception e)
		{
			Debug.LogError("Character failed to spawn!");
			Debug.LogError(e);
			return;
		}

		// Make sure the game knows we're standing on something!
		this.tower.RegisterCharacter(this);
		SetGrounded(true);
	}


	#region Character actions

	private void DoTurn()
	{
		int index = (int)this.facing;
		this.animators[index].gameObject.SetActive(false);
		index = (index + 1) % 4;
		this.animators[index].gameObject.SetActive(true);
		this.facing = (Facings)index;
		this.transform.Rotate(this.transform.up, 90f);
	}

	private void Jump()
	{
		GridPos targetPos = this.selfGridObject.pos + this.directions[(int)this.facing];
		GridObject targetGridObject = Root.instance.grid.GetObjectAt(this.selfGridObject.pos + this.directions[(int)this.facing]);
		if (targetGridObject == null) return;
		Tower targetTower = targetGridObject.GetComponent<Tower>();

		MovementRules.Modalities modality;

		if (this.movementRules.IsValidMove(this.selfGridObject.pos, targetPos, out modality))
		{
			targetTower.RegisterCharacter(this);
			this.tower.DeregisterCharacter();
			this.tower = targetTower;   //FIXME!

			int newHeight = targetTower.gridObject.GetHeight();
			if (modality == MovementRules.Modalities.JUMP_UP_HACK)
				newHeight = 3;

			this.selfGridObject.SetHeight(newHeight);

			switch (modality)
			{
				case MovementRules.Modalities.JUMP_UP:
					JumpUp(targetTower);
					break;
				case MovementRules.Modalities.JUMP_DOWN:
					JumpDown(targetTower);
					break;
				case MovementRules.Modalities.HOP:
					Hop(targetTower);
					break;
				case MovementRules.Modalities.JUMP_UP_HACK:
					JumpUp(targetTower, true);
					break;
			}
		}
		else
		{
			switch (modality)
			{
				case MovementRules.Modalities.BLOCKED:
					DoBump();
					break;
				case MovementRules.Modalities.FALL:
					DoFear();
					break;
			}
		}
	}

	#endregion



	#region Routines

	private void JumpUp(Tower target, bool hack = false)
	{
		if (target != null)
		{
			AudioClip c = this.clip_Jump[Random.Range(0, this.clip_Jump.Length)];
			AudioSource.PlayClipAtPoint(c, Vector3.zero);

			this.selfGridObject.pos = target.gridObject.pos;
			StartCoroutine(SetUpJumpAnimation(target, true, hack));
		}

	}

	private void JumpDown(Tower target)
	{
		if (target != null)
		{
			AudioClip c = this.clip_Jump[Random.Range(0, this.clip_Jump.Length)];
			AudioSource.PlayClipAtPoint(c, Vector3.zero);

			this.selfGridObject.pos = target.gridObject.pos;
			StartCoroutine(SetUpJumpAnimation(target, true));
		}
	}

	private void Hop(Tower target)
	{
		if (target != null)
		{
			this.selfGridObject.pos = target.gridObject.pos;
			StartCoroutine(SetUpJumpAnimation(target, false));
			AudioSource.PlayClipAtPoint(clip_Dash, Vector3.zero);
		}
	}

	private void DoBump()
	{
		AudioClip c = this.clip_Nope[Random.Range(0, this.clip_Nope.Length)];
		AudioSource.PlayClipAtPoint(c, Vector3.zero);
	}

	private void DoFear()
	{
		AudioClip c = this.clip_Nope[Random.Range(0, this.clip_Nope.Length)];
		AudioSource.PlayClipAtPoint(c, Vector3.zero);
	}

	#endregion


	private IEnumerator SetUpJumpAnimation(Tower end, bool jump = true, bool tunnelHack = false)
	{
		SetGrounded(false);
		yield return new WaitForSeconds(JUMP_WINDUP_DELAY);

		Vector3 targetPos;
		try
		{
			targetPos = end.GetSurfaceWorldPos();
		}
		catch (System.Exception e)
		{
			yield break;
		}

		if (tunnelHack)
		{
			targetPos.y += 2;
		}

		if (jump)
		{
			// Move vertically
			Vector3 apogee = Vector3.Lerp(this.transform.position, targetPos, 0.5f) + new Vector3(0f, JUMP_HEIGHT, 0f);
			Tween t = this.transform.DOMoveY(apogee.y, JUMP_DURATION / 2f);
			t.SetEase(Ease.OutQuad);
			t.OnComplete(() =>
			{
				t = this.transform.DOMoveY(targetPos.y, JUMP_DURATION / 2f);
				t.SetEase(Ease.InQuad);
				t.OnComplete(() =>
				{
					SetGrounded(true);
				});
			});
		}
		else
		{
			// Manually set grounded back to true after 1 second
			DOVirtual.DelayedCall(HOP_DURATION, () =>
			{
				SetGrounded(true);
			});
		}

		// Move horizontally
		Ease horizontalEase = (jump) ? Ease.Linear : Ease.InOutQuad;
		if (this.facing == Facings.BACK || this.facing == Facings.FRONT)
			this.transform.DOMoveZ(targetPos.z, JUMP_DURATION).SetEase(horizontalEase);
		else
			this.transform.DOMoveX(targetPos.x, JUMP_DURATION).SetEase(horizontalEase);
	}



	void OnDrawGizmos()
	{
		if (Application.isPlaying)
		{
			Vector3 arrowPoint;
			try
			{
				arrowPoint = Root.instance.grid.GridToWorldPos(this.selfGridObject.pos + this.directions[(int)this.facing]);
				arrowPoint.y = this.transform.position.y;
			}
			catch (System.Exception e)
			{
				return;
			}

			Gizmos.color = Color.cyan;
			Gizmos.DrawLine(this.transform.position, arrowPoint);
		}
	}
}
