using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Nugget : MonoBehaviour
{

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
		Root.instance.endAnimation.DoFireworks();

	}
}
