using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndAnimation : MonoBehaviour
{

	[SerializeField]
	GameObject prefab_Fireworks;

	[SerializeField]
	RenderDiscoColors[] discoShaders;

	public void DoFireworks()
	{
		StartCoroutine(CR_Sequence());
		foreach (RenderDiscoColors r in this.discoShaders)
		{
			r.DoEffects();
		}
	}

	private IEnumerator CR_Sequence()
	{
		GameObject fireWorks = Instantiate(prefab_Fireworks, Root.instance.grid.GetCenterPos(false), Quaternion.AngleAxis(-90f, this.transform.right));
		yield return new WaitForSeconds(5f);
		LevelTransition();
	}

	private void LevelTransition()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.U))
			DoFireworks();
	}
}
