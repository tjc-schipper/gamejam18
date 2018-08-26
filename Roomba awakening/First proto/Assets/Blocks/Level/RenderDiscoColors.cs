using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderDiscoColors : MonoBehaviour
{

	private bool effectEnabled = false;
	public Material discoMaterial;

	public float shakeScale = 0.1f;
	Vector3 basePos;

	private const float FIREWORKS_INTERVAL = 1f;
	private const float MAX_SHAKE = 2f;
	private const float MIN_SCALE = 0.01f;
	private const float SAFETY = 0.001f;

	Coroutine decreaseCR;

	public void DoEffects()
	{
		this.effectEnabled = true;
		this.basePos = this.transform.position;
		StartCoroutine(CR_ScreenShakes());
	}


	void Update()
	{
		if (effectEnabled)
		{
			this.transform.position = this.basePos + Random.insideUnitSphere * shakeScale;
		}
	}

	private IEnumerator CR_ScreenShakes()
	{
		this.shakeScale = MIN_SCALE;
		while (this.effectEnabled)
		{
			yield return new WaitForSeconds(1f);
			this.decreaseCR = StartCoroutine(CR_DecreaseScale());
		}
	}

	private IEnumerator CR_DecreaseScale()
	{
		shakeScale = MAX_SHAKE;
		while (shakeScale > MIN_SCALE + SAFETY)
		{
			shakeScale = Mathf.Lerp(shakeScale, MIN_SCALE, 0.1f);
			yield return new WaitForEndOfFrame();
		}
	}

	void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		if (effectEnabled)
		{
			Graphics.Blit(src, dst, discoMaterial);
		}
		else
		{
			Graphics.Blit(src, dst);
		}
	}
}