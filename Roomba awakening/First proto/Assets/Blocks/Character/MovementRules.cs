using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlocksGame;

public abstract class MovementRules : MonoBehaviour {

	public enum Modalities
	{
		HOP = 0,
		JUMP_UP,
		JUMP_DOWN,
		FALL,   //????
		BLOCKED, //CANNOT MOVE!
		JUMP_UP_HACK
	}

	public abstract bool IsValidMove(GridPos from, GridPos to, out Modalities mod);

}
