using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine.EventSystems;

public class Enemy : MonoBehaviour
{
	public float Speed = 10f;
	public float FireRate = .3f;
	public float Health = 10;
	public int Score = 100;

	public int ShowDamageForFrames = 2;
	public float PowerUpDropChance = 1;

	public bool _____________________;

	public Color[] OriginalColors;
	public Material[] Materials;
	public int RemainingDamageFrames;

	public Bounds Bounds;
	public Vector3 BoundsCenterOffset;

	public Vector3 Pos
	{
		get { return this.transform.position; }
		set { this.transform.position = value; }
	}


	void Awake()
	{
		Materials = Utils.GetAllMaterials(gameObject);
		OriginalColors = new Color[Materials.Length];
		for (int i = 0; i < Materials.Length; i++)
		{
			OriginalColors[i] = Materials[i].color;
		}
		InvokeRepeating("CheckOffscreen", 0f, 2f);
	}

	void Update()
	{
		Move();
		if (RemainingDamageFrames > 0)
		{
			RemainingDamageFrames--;
			if (RemainingDamageFrames == 0)
			{
				UnShowDamage();
			}
		}
	}

	public virtual void Move()
	{
		Vector3 tempPos = Pos;
		tempPos.y -= Speed * Time.deltaTime;
		Pos = tempPos;
	}

	void CheckOffscreen()
	{
		if (Bounds.size == Vector3.zero)
		{
			Bounds = Utils.CombineBoundsOfChildren(gameObject);
			BoundsCenterOffset = Bounds.center - transform.position;
		}

		Bounds.center = transform.position + BoundsCenterOffset;
		Vector3 off = Utils.ScreenBoundsCheck(Bounds, BoundsTest.OffScreen);
		if (off != Vector3.zero)
		{
			if (off.y < 0)
			{
				Destroy(gameObject);
			}
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		GameObject other = collision.gameObject;
		switch (other.tag)
		{
			case "ProjectileHero":
				Projectile p = other.GetComponent<Projectile>();
				Bounds.center = transform.position + BoundsCenterOffset;
				if ((Bounds.extents == Vector3.zero) || Utils.ScreenBoundsCheck(Bounds, BoundsTest.OffScreen) != Vector3.zero)
				{
					Destroy(other);
					break;
				}
				ShowDamage();
				Health -= Main.W_DEFS[p.Type].DamageOnHit;
				if (Health <= 0)
				{
					Main.S.ShipDestroyed(this);
					Destroy(gameObject);
				}
				Destroy(other);
				break;
		}
	}

	void ShowDamage()
	{
		foreach (var material in Materials)
		{
			material.color = Color.red;
		}
		RemainingDamageFrames = ShowDamageForFrames;
	}

	void UnShowDamage()
	{
		for (int i = 0; i < Materials.Length; i++)
		{
			Materials[i].color = OriginalColors[i];
		}
	}
}