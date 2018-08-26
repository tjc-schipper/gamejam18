using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Nugget : MonoBehaviour
{

	[SerializeField]
	AudioClip[] clip_Yay;

	[SerializeField]
	Transform sprite;

	bool triggered = false;


	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Character") && triggered == false)
		{
			triggered = true;
			DoPickup();
		}
	}


	private void DoPickup()
	{
		AudioClip clip = this.clip_Yay[Random.Range(0, this.clip_Yay.Length)];
		AudioSource.PlayClipAtPoint(clip, Vector3.zero);

		Root.instance.endAnimation.DoFireworks();
		SpriteRenderer r = this.sprite.GetComponent<SpriteRenderer>();
		float baseScale = this.sprite.localScale.magnitude;

		Sequence seq = DOTween.Sequence();
		seq.Append(this.sprite.DOScale(1.5f * baseScale, 1f).SetEase(Ease.OutQuad));
		seq.Append(DOVirtual.Float(1f, 0f, 2f, (float v) =>
		{
			this.sprite.localScale = Vector3.one * baseScale * v;
			Color c = r.color;
			c.a = v;
			r.color = c;
		}));
		seq.Play();
	}
}
