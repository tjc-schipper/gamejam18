using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soundtrack : MonoBehaviour {

	[SerializeField]
	AudioClip ost;
	
	// Use this for initialization
	void Start () {
		AudioSource src = gameObject.AddComponent<AudioSource>();
		src.clip = this.ost;
		src.loop = true;
		src.volume = 0.1f;
		src.spatialize = false;
		src.Play();

		DontDestroyOnLoad(this.gameObject);
	}
}
