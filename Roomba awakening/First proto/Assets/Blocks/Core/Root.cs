using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{

	public Camera topCam;

	public Camera sideCam;

	public DragField grid;

	public EndAnimation endAnimation;

	public SpawnCharacter spawnCharacter;


	public Transform refObject
	{
		get
		{
			return this.transform;
		}
	}



	void Start()
	{
		Vector3 gridCenterPos = this.grid.GetCenterPos();
		this.topCam.transform.position = gridCenterPos + new Vector3(0f, 10f, 0f);
		this.sideCam.transform.position = gridCenterPos + new Vector3(0f, 0f, -10f);
		Invoke("DelayedStart", 0.1f);
	}

	void DelayedStart()
	{
		this.spawnCharacter.DoSpawn();
	}


	private static Root _instance;

	public static Root instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<Root>();
			}
			return _instance;
		}
	}
}
