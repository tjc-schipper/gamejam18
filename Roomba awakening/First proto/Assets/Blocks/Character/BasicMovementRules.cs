using System;
using System.Collections;
using System.Collections.Generic;
using BlocksGame;
using UnityEngine;

public class BasicMovementRules : MovementRules
{

	public override bool IsValidMove(GridPos from, GridPos to, out Modalities mod)
	{
		bool valid = true;

		valid &= Root.instance.grid.IsAdjacent(from, to);
		GridObject fromObject = Root.instance.grid.GetObjectAt(from);
		GridObject toObject = Root.instance.grid.GetObjectAt(to);

		if (toObject == null)
		{
			mod = Modalities.FALL;
			return false;
		}

		//int fromHeight = fromObject.GetHeight();
		int fromHeight = Root.instance.spawnCharacter.currentCharacter.GetCharacterHeight();
		int toHeight = toObject.GetHeight();

		Tower fromTower = fromObject.GetComponent<Tower>();
		Tower toTower = toObject.GetComponent<Tower>();

		if (toTower == null || !toTower.IsWalkable())
		{
			if (toHeight < fromHeight)
			{
				mod = Modalities.FALL;
			}
			else
			{
				mod = Modalities.BLOCKED;
			}
			return false;
		}
		else
		{
			if (fromTower.towerType == Tower.TowerTypes.TUNNEL)
			{
				if (toHeight == fromHeight)
				{
					mod = Modalities.HOP;
					return true;
				}
				else
				{
					mod = Modalities.BLOCKED;
					return false;
				}
			}
			else
			{
				if (toTower.towerType == Tower.TowerTypes.STANDARD)
				{
					if (toHeight > fromHeight)
					{
						if (toHeight == fromHeight + 1)
						{
							mod = Modalities.JUMP_UP;
							return true;
						}
						else
						{
							mod = Modalities.BLOCKED;
							return false;
						}
					}
					else if (toHeight < fromHeight)
					{
						if (toHeight == fromHeight - 1)
						{
							mod = Modalities.JUMP_DOWN;
							return true;
						}
						else
						{
							mod = Modalities.FALL;
							return false;
						}
					}
					else
					{
						mod = Modalities.HOP;
						return true;
					}
				}
				else if (toTower.towerType == Tower.TowerTypes.TUNNEL)
				{
					if (toHeight == fromHeight)
					{
						mod = Modalities.HOP;
						return true;
					}
					else if (toHeight == fromHeight - 1 && fromHeight == 2)	//HACK to support jumping on top of tunnel!
					{
						mod = Modalities.JUMP_UP_HACK;
						return true;
					}
					else
					{
						mod = Modalities.BLOCKED;
						return false;
					}
				}
				else
				{
					// Apparently it's an unknown tower type?!
					mod = Modalities.BLOCKED;
					return false;
				}
			}
		}
	}
}