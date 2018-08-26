using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
public class FloatingCloud : MonoBehaviour
{

	[SerializeField]
	float minSpeed = 0.5f;

	[SerializeField]
	float maxSpeed = 2f;

	[SerializeField]
	float minY = -2f;

	[SerializeField]
	float maxY = 8f;

	bool right = false;
	float speed = 0f;

	private SpriteRenderer r;

	private const float OFFSCREEN_X = 5f;

	public void Init(Sprite _sprite)
	{
		this.r = GetComponent<SpriteRenderer>();
		this.r.sprite = _sprite;

		this.right = Random.Range(0f, 1f) > 0.5f;
		this.speed = Random.Range(minSpeed, maxSpeed);

		float x = (right) ? -OFFSCREEN_X : OFFSCREEN_X;
		float y = Random.Range(minY, maxY);

		Vector3 offset = new Vector3(x, y, 20f);
		this.transform.localPosition = offset;

		Tween t = this.transform.DOLocalMoveX(-x, (2f * OFFSCREEN_X) / speed);
		t.SetEase(Ease.Linear);
		t.OnComplete(() =>
		{
			Destroy(this.gameObject);
		});
	}
}
