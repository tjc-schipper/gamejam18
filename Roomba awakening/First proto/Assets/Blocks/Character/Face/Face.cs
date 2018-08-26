using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Face : MonoBehaviour
{

	[SerializeField]
	SpriteRenderer[] eyesNormal;

	[SerializeField]
	SpriteRenderer[] eyesScrunched;

	[SerializeField]
	SpriteRenderer happyMouth;

	[SerializeField]
	SpriteRenderer sadMouth;

	[SerializeField]
	SpriteRenderer scrunchMouth;

	[SerializeField]
	SpriteRenderer openMouth;


	public enum Expressions
	{
		IDLE,
		CLOSED,
		SCRUNCH,
		HAPPY,
		SAD,
		HIDDEN
	}
	private Expressions expression;

	public void SetExpression(Expressions ex)
	{
		this.expression = ex;
		switch (ex)
		{
			case Expressions.IDLE:
				OpenEyes();
				SmileMouth();
				break;
			case Expressions.CLOSED:
				CloseEyes();
				ScrunchMouth();
				break;
			case Expressions.SCRUNCH:
				ScrunchedEyes();
				ScrunchMouth();
				break;
			case Expressions.HAPPY:
				OpenEyes();
				OpenMouth();
				break;
			case Expressions.SAD:
				OpenEyes();
				SadMouth();
				break;
			case Expressions.HIDDEN:
				HiddenEyes();
				HiddenMouth();
				break;
		}
	}


	void Update()
	{

		if (Input.GetKeyDown(KeyCode.H))
			SetExpression(Expressions.IDLE);

		if (Input.GetKeyDown(KeyCode.J))
			SetExpression(Expressions.HAPPY);

		if (Input.GetKeyDown(KeyCode.K))
			SetExpression(Expressions.SAD);

		if (Input.GetKeyDown(KeyCode.L))
			SetExpression(Expressions.SCRUNCH);
	}


	void OnEnable()
	{
		SetExpression(Expressions.IDLE);
	}


	private void Blink()
	{
		Sequence blinkSeq = DOTween.Sequence();
		blinkSeq.Append(DOVirtual.Float(1f, 0.1f, 0.1f, (float v) =>
		{
			foreach (SpriteRenderer r in this.eyesNormal)
			{
				r.transform.localScale = new Vector3(v, 1f, 1f);
			}
		}));
		blinkSeq.Append(DOVirtual.Float(0.1f, 1f, 0.4f, (float v) =>
		{
			foreach (SpriteRenderer r in this.eyesNormal)
			{
				r.transform.localScale = new Vector3(v, 1f, 1f);
			}
		}));
		blinkSeq.Play();
	}

	private void OpenEyes()
	{
		this.NormalEyes();
		foreach (SpriteRenderer r in this.eyesNormal)
		{
			r.transform.localScale = new Vector3(0.1f, 1f, 1f);
			r.transform.DOScaleX(1f, 0.25f);
		}
	}

	private void CloseEyes()
	{
		this.NormalEyes();
		foreach (SpriteRenderer r in this.eyesNormal)
		{
			r.transform.localScale = new Vector3(1f, 1f, 1f);
			r.transform.DOScaleX(0.1f, 0.25f);
		}
	}




	#region Eyes

	private void HiddenEyes()
	{
		foreach (SpriteRenderer r in this.eyesScrunched)
		{
			r.gameObject.SetActive(false);
		}
		foreach (SpriteRenderer r in this.eyesNormal)
		{
			r.gameObject.SetActive(false);
		}
	}

	private void NormalEyes()
	{
		foreach (SpriteRenderer r in this.eyesScrunched)
		{
			r.gameObject.SetActive(false);
		}
		foreach (SpriteRenderer r in this.eyesNormal)
		{
			r.gameObject.SetActive(true);
		}
	}

	private void ScrunchedEyes()
	{
		foreach (SpriteRenderer r in this.eyesScrunched)
		{
			r.gameObject.SetActive(true);
		}
		foreach (SpriteRenderer r in this.eyesNormal)
		{
			r.gameObject.SetActive(false);
		}
	}

	#endregion

	#region Mouth

	private void SmileMouth()
	{
		if (this.happyMouth != null) this.happyMouth.gameObject.SetActive(true);
		if (this.sadMouth != null) this.sadMouth.gameObject.SetActive(false);
		if (this.scrunchMouth != null) this.scrunchMouth.gameObject.SetActive(false);
		if (this.openMouth != null) this.openMouth.gameObject.SetActive(false);
	}

	private void SadMouth()
	{
		if (this.happyMouth != null) this.happyMouth.gameObject.SetActive(false);
		if (this.sadMouth != null) this.sadMouth.gameObject.SetActive(true);
		if (this.scrunchMouth != null) this.scrunchMouth.gameObject.SetActive(false);
		if (this.openMouth != null) this.openMouth.gameObject.SetActive(false);
	}

	private void ScrunchMouth()
	{
		if (this.happyMouth != null) this.happyMouth.gameObject.SetActive(false);
		if (this.sadMouth != null) this.sadMouth.gameObject.SetActive(false);
		if (this.scrunchMouth != null) this.scrunchMouth.gameObject.SetActive(true);
		if (this.openMouth != null) this.openMouth.gameObject.SetActive(false);
	}

	private void OpenMouth()
	{
		if (this.happyMouth != null) this.happyMouth.gameObject.SetActive(false);
		if (this.sadMouth != null) this.sadMouth.gameObject.SetActive(false);
		if (this.scrunchMouth != null) this.scrunchMouth.gameObject.SetActive(false);
		if (this.openMouth != null) this.openMouth.gameObject.SetActive(true);
	}

	private void HiddenMouth()
	{
		if (this.happyMouth != null) this.happyMouth.gameObject.SetActive(false);
		if (this.sadMouth != null) this.sadMouth.gameObject.SetActive(false);
		if (this.scrunchMouth != null) this.scrunchMouth.gameObject.SetActive(false);
		if (this.openMouth != null) this.openMouth.gameObject.SetActive(false);
	}

	#endregion
}
