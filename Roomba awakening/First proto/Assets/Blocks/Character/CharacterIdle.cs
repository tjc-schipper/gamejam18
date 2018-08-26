using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIdle : MonoBehaviour
{

	[SerializeField]
	Transform[] sideSprites;

	[SerializeField]
	[Range(0f, 0.75f)]
	float squish = 0.25f;

	[SerializeField]
	[Range(0f, 0.25f)]
	float widen = 0.1f;

	[SerializeField]
	[Range(0f, 1f)]
	float yAdjust = 0.1f;

	float timer = 0f;
	private const float DURATION = 2f;
	Vector3[] startScales;


	void Awake()
	{
		this.startScales = new Vector3[this.sideSprites.Length];
		for (int i = 0; i < this.sideSprites.Length; i++)
		{
			this.startScales[i] = this.sideSprites[i].transform.localScale;
		}
	}

	// Update is called once per frame
	void Update()
	{
		timer += Time.deltaTime;
		float f = (Mathf.Cos((timer / DURATION) * 2f * Mathf.PI) + 1f) / 2.0f;
		float sq = 1f - (1f - f) * squish;
		float wi = 1f + (1f - f) * widen;

		for (int i = 0; i < this.sideSprites.Length; i++)
		{
			Vector3 s = this.startScales[i];
			this.sideSprites[i].localScale = new Vector3(wi * s.x, sq * s.y, 1f * s.z);
			this.sideSprites[i].localPosition = new Vector3(0f, (1f - f) * this.yAdjust, 0f);
		}
	}

	void OnDisable()
	{
		for (int i = 0; i < this.sideSprites.Length; i++)
		{
			this.sideSprites[i].localScale = this.startScales[i];
			this.sideSprites[i].localPosition = new Vector3(0f, 0f, 0f);
		}
	}
}
