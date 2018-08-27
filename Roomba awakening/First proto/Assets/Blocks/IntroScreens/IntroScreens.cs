using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScreens : MonoBehaviour
{

	[SerializeField]
	SpriteRenderer one;

	[SerializeField]
	SpriteRenderer two;

	[SerializeField]
	SpriteRenderer three;

	int i = 0;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Advance();
		}
	}


	void Advance()
	{
		if (i == 0)
		{
			this.one.enabled = false;
		}
		else if (i == 1)
		{
			this.two.enabled = false;
		}
		else if (i == 2)
		{
			Debug.Log("IS THIS IS THE LOGS?!");
			SceneManager.LoadScene("l1");
		}
		i++;



	}
}
