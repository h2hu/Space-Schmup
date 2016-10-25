using UnityEngine;
using System.Collections;

public class Enemy_2 : Enemy
{
	public Vector3[] Points;
	public float BirthTime;
	public float LifeTime = 10;
	public float SinEccentricity = .6f;

	void Start()
	{
		Points = new Vector3[2];
		Vector3 cbMin = Utils.CamBounds.min;
		Vector3 cbMax = Utils.CamBounds.max;

		Vector3 v = Vector3.zero;
		v.x = cbMin.x - Main.S.EnemySpawnPadding;
		v.y = Random.Range(cbMin.y, cbMax.y);
		Points[0] = v;

		v = Vector3.zero;
		v.x = cbMax.x + Main.S.EnemySpawnPadding;
		v.y = Random.Range(cbMin.y, cbMax.y);
		Points[1] = v;
		print( "Fuck " +Points[0] + Points[1]);
		if (Random.value < .5f)
		{
			Points[0].x *= -1;
			Points[1].x *= -1;
		}
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
		u = u + SinEccentricity * Mathf.Sin(u * Mathf.PI * 2);
		Pos = (1 - u) * Points[0] + u * Points[1];

	}
}
