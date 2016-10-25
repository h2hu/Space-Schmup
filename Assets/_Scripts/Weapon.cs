using UnityEngine;
using System.Collections;


public enum WeaponType
{
	None,
	Blaster,
	Spread,
	Phaser,
	Missle,
	Laser,
	Shield
}

[System.Serializable]
public class WeaponDefinition
{
	public WeaponType Type = WeaponType.None;
	public string Letter;
	public Color Color = Color.white;
	public GameObject ProjectilePrefab;
	public Color ProjectileColor = Color.white;
	public float DamageOnHit = 0;
	public float ContinuousDamage = 0;
	public float DelayBetweenShots = 0;
	public float Velocity = 20;
}

public class Weapon : MonoBehaviour
{
	public static Transform PROJECTILE_ANCHOR;

	public bool ____________________________;

	[SerializeField] private WeaponType _type = WeaponType.Blaster;

	public WeaponDefinition Def;
	public GameObject Collar;
	public float LastShot;


	// Use this for initialization
	void Start()
	{
		Collar = transform.Find("Collar").gameObject;
		SetType(_type);
		if (PROJECTILE_ANCHOR == null)
		{
			GameObject go = new GameObject("_Projectile_Anchor");
			PROJECTILE_ANCHOR = go.transform;
		}

		GameObject parentGO = transform.parent.gameObject;
		if (parentGO.tag == "Hero")
		{
			Hero.S.FireDelegate += Fire;
		}
	}

	void Fire()
	{
		if (!gameObject.activeInHierarchy || (Time.time - LastShot < Def.DelayBetweenShots))
		{
			return;
		}
		Projectile p;
		switch (Type)
		{
			case WeaponType.Blaster:
				p = MakeProjectile();
				p.GetComponent<Rigidbody>().velocity = Vector3.up * Def.Velocity;
				break;
			case WeaponType.Spread:
				p = MakeProjectile();
				p.GetComponent<Rigidbody>().velocity = Vector3.up * Def.Velocity;
				p = MakeProjectile();
				p.GetComponent<Rigidbody>().velocity = new Vector3(-.2f, .9f, 0) * Def.Velocity;
				p = MakeProjectile();
				p.GetComponent<Rigidbody>().velocity = new Vector3(.2f, .9f, 0) * Def.Velocity;
				break;
		}
	}

	public Projectile MakeProjectile()
	{
		GameObject go = Instantiate(Def.ProjectilePrefab);
		if (transform.parent.gameObject.tag == "Hero")
		{
			go.tag = "ProjectileHero";
			go.layer = LayerMask.NameToLayer("ProjectileHero");
		}
		else
		{
			go.tag = "ProjectileEnemy";
			go.layer = LayerMask.NameToLayer("ProjectileEnemy");
		}
		go.transform.position = Collar.transform.position;
		go.transform.parent = PROJECTILE_ANCHOR;
		Projectile p = go.GetComponent<Projectile>();
		p.Type = Type;
		LastShot = Time.time;
		return p;
	}

	public WeaponType Type
	{
		get { return _type; }
		set { SetType(value); }
	}

	public void SetType(WeaponType wt)
	{
		_type = wt;
		if (Type == WeaponType.None)
		{
			gameObject.SetActive(false);
			return;
		}

		gameObject.SetActive(true);
		Def = Main.S.GetWeaponDefinition(_type);
		Collar.GetComponent<Renderer>().material.color = Def.Color;
		LastShot = 0;
	}
}