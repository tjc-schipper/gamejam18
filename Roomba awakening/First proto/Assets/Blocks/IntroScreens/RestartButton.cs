using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{

	public void DoRestart()
	{
		int buildIndex = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene(buildIndex);
	}

	public void RestartGame()
	{
		// Prevent double soundtrack playing!
		Soundtrack ost = GameObject.FindObjectOfType<Soundtrack>();
		DestroyImmediate(ost.gameObject);

		SceneManager.LoadScene(0);  // Go all the way back to the beginning!
	}
}
