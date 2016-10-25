using UnityEngine;
using System.Collections;

public class Enemy_3 : Enemy
{
	public Vector3[] Points;
	public float BirthTime;
	public float LifeTime = 10;

	// Use this for initialization
	void Start()
	{
		Points = new Vector3[3];
		Points[0] = Pos;
		float xMin = Utils.CamBounds.min.x + Main.S.EnemySpawnPadding;
		float xMax = Utils.CamBounds.max.x - Main.S.EnemySpawnPadding;


		Vector3 v = Vector3.zero;
		v.x = Random.Range(xMin, xMax);
		v.y = Random.Range(Utils.CamBounds.min.y, 0);
		Points[1] = v;

		v = Vector3.zero;
		v.x = Random.Range(xMin, xMax);
		v.y = Pos.y;
		Points[2] = v;
		BirthTime = Time.time;
	}

	public override void Move()
	{
		float u = (Time.time - BirthTime) / LifeTime;

		if (u > 1)
		{
			Destroy(gameObject);
			return;
		}
		Vector3 p01, p12;
		u = u - .2f * Mathf.Sin(u * Mathf.PI * 2);
		p01 = (1 - u) * Points[0] + u * Points[1];
		p12 = (1 - u) * Points[1] + u * Points[2];
		Pos = (1 - u) * p01 + u * p12;
	}
}