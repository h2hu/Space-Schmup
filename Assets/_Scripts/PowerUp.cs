using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour
{
	public Vector2 RotMinMax = new Vector2(15, 90);
	public Vector2 DriftMinMax = new Vector2(.25f, 2);
	public float LifeTime = 6f;
	public float FadeTime = 4f;

	public bool _____________________________________;

	public WeaponType Type;
	public GameObject Cube;
	public TextMesh Letter;
	public Vector3 RotPerSecond;
	public float BirthTime;

	void Awake()
	{
		Cube = transform.Find("Cube").gameObject;
		Letter = GetComponent<TextMesh>();

		Vector3 vel = Random.onUnitSphere;
		vel.z = 0;
		vel.Normalize();
		vel *= Random.Range(DriftMinMax.x, DriftMinMax.y);
		GetComponent<Rigidbody>().velocity = vel;

		transform.rotation = Quaternion.identity;


		RotPerSecond = new Vector3(Random.Range(RotMinMax.x, RotMinMax.y),
			Random.Range(RotMinMax.x, RotMinMax.y),
			Random.Range(RotMinMax.x, RotMinMax.y));

		InvokeRepeating("CheckOffscreen", 2, 2);

		BirthTime = Time.time;
	}

	// Update is called once per frame
	void Update()
	{
		Cube.transform.rotation = Quaternion.Euler(RotPerSecond * Time.time);

		float u = (Time.time - (BirthTime + LifeTime)) / FadeTime;

		if (u >= 1)
		{
			Destroy(gameObject);
			return;
		}

		if (u > 0)
		{
			Color c = Cube.GetComponent<Renderer>().material.color;

			c.a = 1 - u;
			Cube.GetComponent<Renderer>().material.color = c;

			c = Letter.color;
			c.a = 1 - (u * .5f);
			Letter.color = c;
		}
	}

	public void SetType(WeaponType wt)
	{
		print("PowerUp SetType : " + wt);
		WeaponDefinition def = Main.S.GetWeaponDefinition(wt);
		Cube.GetComponent<Renderer>().material.color = def.Color;

		Letter.text = def.Letter;
		Type = wt;
	}


	public void AbsorbedBy(GameObject target)
	{
		Destroy(gameObject);
	}

	void CheckOffscreen()
	{
		if (Utils.ScreenBoundsCheck(Cube.GetComponent<Collider>().bounds, BoundsTest.OffScreen) != Vector3.zero)
		{
			Destroy(gameObject);
		}
	}
}