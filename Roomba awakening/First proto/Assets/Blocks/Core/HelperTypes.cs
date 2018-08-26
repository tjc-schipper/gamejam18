using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlocksGame
{
	public class GridPos
	{
		public int x;
		public int z;

		public GridPos(int _x, int _z)
		{
			this.x = _x;
			this.z = _z;
		}

		public GridPos(float _x, float _z)
		{
			this.x = Mathf.RoundToInt(_x);
			this.z = Mathf.RoundToInt(_z);
		}

		public GridPos(Vector2 pos)
		{
			this.x = (int)pos.x;
			this.z = (int)pos.y;
		}

		public GridPos(Vector3 pos)
		{
			this.x = (int)pos.x;
			this.z = (int)pos.z;
		}


		#region Conversions

		public static implicit operator GridPos(Vector2 v)
		{
			return new GridPos(v);
		}

		public static implicit operator GridPos(Vector3 v)
		{
			return new GridPos(v);
		}


		#endregion



		#region Operators

		public static GridPos operator +(GridPos a, GridPos b)
		{
			return new GridPos(
				a.x + b.x,
				a.z + b.z
				);
		}

		public static GridPos operator -(GridPos a, GridPos b)
		{
			return new GridPos(
				a.x - b.x,
				a.z - b.z
				);
		}

		public static GridPos operator *(GridPos a, int b)
		{
			return new GridPos(
				a.x * b,
				a.z * b
				);
		}

		#endregion
	}

}