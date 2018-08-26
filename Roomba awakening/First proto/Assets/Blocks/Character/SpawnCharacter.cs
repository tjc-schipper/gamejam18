using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCharacter : MonoBehaviour {

	[SerializeField]
	GameObject prefab_Character;

	[SerializeField] Tower spawnTower;
	
	public Character currentCharacter;
	
	public void DoSpawn()
	{
		if (this.currentCharacter != null)
		{
			Destroy(this.currentCharacter.gameObject);
		}
		
		this.currentCharacter = Instantiate(prefab_Character, Vector3.zero, Quaternion.identity).GetComponent<Character>();
		this.currentCharacter.Spawn(this.spawnTower.gridObject.pos);
	}
}
