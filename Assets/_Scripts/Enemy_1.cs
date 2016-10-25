using UnityEngine;
using System.Collections;

public class Enemy_1 : Enemy
{


	public float WaveFrequency = 2;
	public float WaveWidth = 4;
	public float WaveRotY = 45;

	private float _x0 = -12345;
	private float _birthTime;

	void Start ()
	{
		_x0 = Pos.x;
		_birthTime = Time.time;
	}

	public override void Move()
	{
		Vector3 tempPos = Pos;
		float age = Time.time - _birthTime;
		float theta = Mathf.PI * 2 * age / WaveFrequency;
		float sin = Mathf.Sin(theta);
		tempPos.x = _x0 + WaveWidth * sin;
		Pos = tempPos;

		Vector3 rot = new Vector3(0, sin * WaveRotY, 0);
		transform.rotation = Quaternion.Euler(rot);

		base.Move();
	}
}
