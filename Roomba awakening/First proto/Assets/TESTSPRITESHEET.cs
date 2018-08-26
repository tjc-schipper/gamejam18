using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TESTSPRITESHEET : MonoBehaviour
{

	private Animator animator;
	Tween tween;

	// Use this for initialization
	void Start()
	{
		this.animator = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			if (this.tween == null)
			{
				this.animator.SetBool("b_grounded", true);
			}
		}
	}

	public void AT_LeftGround()
	{
		Tween sideTween = this.transform.DOMoveX(this.transform.position.x + 1f, 1f).SetEase(Ease.Linear);

		this.tween = this.transform.DOMoveY(this.transform.position.y + 1f, 0.5f);
		this.tween.SetEase(Ease.OutQuad);
		this.tween.OnComplete(() =>
		{
			this.tween = this.transform.DOMoveY(this.transform.position.y - 1f, 0.5f);
			this.tween.SetEase(Ease.InQuad);
			this.tween.OnComplete(() =>
			{
				this.animator.SetBool("b_grounded", false);
				this.tween = null;
			});
		});
	}
}
