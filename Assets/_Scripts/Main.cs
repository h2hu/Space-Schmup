using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{

	public static Main S;
	public static Dictionary<WeaponType, WeaponDefinition> W_DEFS;

	public GameObject[] PrefabEnemies;
	public float EnemiesSpawnPerSecond = .5f;
	public float EnemySpawnPadding = 1.5f;
	public WeaponDefinition[] WeaponDefinitions;

	public bool ____________________;

	public WeaponType[] ActiveWeaponTypes;
	public float EnemySpawnRate;

	void Awake()
	{
		S = this;
		Utils.SetCameraBounds(this.GetComponent<Camera>());
		EnemySpawnRate = 1f / EnemiesSpawnPerSecond;
		Invoke("SpawnEnemy", EnemySpawnRate);

		W_DEFS = new Dictionary<WeaponType, WeaponDefinition>();
		foreach (var definition in WeaponDefinitions)
		{
			W_DEFS[definition.Type] = definition;
		}
	}

	void Start()
	{
		ActiveWeaponTypes = new WeaponType[WeaponDefinitions.Length];
		for (int i = 0; i < WeaponDefinitions.Length; i++)
		{
			ActiveWeaponTypes[i] = WeaponDefinitions[i].Type;
		}
	}

	void SpawnEnemy()
	{
		int ndx = Random.Range(0, PrefabEnemies.Length);
		GameObject go = Instantiate(PrefabEnemies[ndx]);

		Vector3 pos = Vector3.zero;
		float xMin = Utils.CamBounds.min.x + EnemySpawnPadding;
		float xMax = Utils.CamBounds.max.x - EnemySpawnPadding;

		pos.x = Random.Range(xMin, xMax);
		pos.y = Utils.CamBounds.max.y + EnemySpawnPadding;
		go.transform.position = pos;

		Invoke("SpawnEnemy", EnemySpawnRate);
	}

	public void DelayedRestart(float delay)
	{
		Invoke("Restart", delay);
	}

	public void Restart()
	{
		SceneManager.LoadScene("_Scene_0");
	}


	public WeaponDefinition GetWeaponDefinition(WeaponType weaponType)
	{
		if (W_DEFS.ContainsKey(weaponType))
		{
			return W_DEFS[weaponType];
		}
		return new WeaponDefinition();
	}
}
