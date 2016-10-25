using System;
using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour
{
	public float RotationsPerSecond = .1f;
	public bool _________________________;
	public int LevelShown = 0;

	// Update is called once per frame
	void Update()
	{
		var currLevel = Mathf.FloorToInt(Hero.S.ShieldLevel);
		if (LevelShown != currLevel)
		{
			LevelShown = currLevel;
			Material mat = GetComponent<Renderer>().material;
			mat.mainTextureOffset = new Vector2(.2f * LevelShown, 0);
		}
		Console.Out.Write("Update");
		var rZ = (RotationsPerSecond * Time.time * 360) % 360f;
		transform.rotation = Quaternion.Euler(0, 0, rZ);
	}
}