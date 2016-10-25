using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour
{

	public static Main S;

	public GameObject[] PrefabEnemies;
	public float EnemiesSpawnPerSecond = .5f;
	public float EnemySpawnPadding = 1.5f;

	public bool ____________________;

	public float EnemySpawnRate;

	void Awake()
	{
		S = this;
		Utils.SetCameraBounds(this.GetComponent<Camera>());
		EnemySpawnRate = 1f / EnemiesSpawnPerSecond;
		Invoke("SpawnEnemy", EnemySpawnRate);
	}

	void SpawnEnemy()
	{
		int ndx = Random.Range(0, PrefabEnemies.Length);
		GameObject go = Instantiate(PrefabEnemies[ndx]) as GameObject;

		Vector3 pos = Vector3.zero;
		float xMin = Utils.CamBounds.min.x + EnemySpawnPadding;
		float xMax = Utils.CamBounds.max.x - EnemySpawnPadding;

		pos.x = Random.Range(xMin, xMax);
		pos.y = Utils.CamBounds.max.y + EnemySpawnPadding;
		go.transform.position = pos;

		Invoke("SpawnEnemy", EnemySpawnRate);
	}

}
