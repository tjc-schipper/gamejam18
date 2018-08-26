using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Clouds : MonoBehaviour
{

	[SerializeField]
	Sprite[] clouds;

	[SerializeField]
	GameObject prefab_Cloud;

	[SerializeField]
	float interval = 3f;

	void Start()
	{
		StartCoroutine(CR_SpawnTimer());
	}

	private IEnumerator CR_SpawnTimer()
	{
		while (true)
		{
			SpawnCloud();
			yield return new WaitForSeconds(3f);
		}
	}

	void SpawnCloud()
	{
		int i = Random.Range(0, this.clouds.Length);
		GameObject newCloud = Instantiate(prefab_Cloud, this.transform);
		newCloud.GetComponent<FloatingCloud>().Init(this.clouds[i]);
	}
}
