using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

[System.Serializable]
public class Part
{
	public string Name;
	public float Health;
	public string[] ProtectedBy;
	public GameObject go;
	public Material mat;
}

public class Enemy_4 : Enemy
{
	public Vector3[] Points;
	public float TimeStart;
	public float Duration = 4;

	public Part[] Parts;

	void Start()
	{
		Points = new Vector3[2];

		Points[0] = Pos;
		Points[1] = Pos;

		InitMovement();
		Transform t;
		foreach (var part in Parts)
		{
			t = transform.Find(part.Name);
			if (t != null)
			{
				part.go = t.gameObject;
				part.mat = part.go.GetComponent<Renderer>().material;
			}
		}
	}

	void InitMovement()
	{
		Vector3 p1 = Vector3.zero;
		float esp = Main.S.EnemySpawnPadding;
		Bounds cBounds = Utils.CamBounds;

		p1.x = Random.Range(cBounds.min.x + esp, cBounds.max.x - esp);
		p1.y = Random.Range(cBounds.min.y + esp, cBounds.max.y - esp);

		Points[0] = Points[1];
		Points[1] = p1;
		TimeStart = Time.time;
	}


	public override void Move()
	{
		float u = (Time.time - TimeStart) / Duration;

		if (u >= 1)
		{
			InitMovement();
			u = 0;
		}
		u = 1 - Mathf.Pow(1 - u, 2);
		Pos = (1 - u) * Points[0] + u * Points[1];
	}

	void OnCollisionEnter(Collision coll)
	{
		GameObject other = coll.gameObject;
		switch (other.tag)
		{
			case "ProjectileHero":
				Projectile p = other.GetComponent<Projectile>();
				Bounds.center = transform.position + BoundsCenterOffset;
				if (Bounds.extents == Vector3.zero || Utils.ScreenBoundsCheck(Bounds, BoundsTest.OffScreen) != Vector3.zero)
				{
					Destroy(other);
				}
				GameObject goHit = coll.contacts[0].thisCollider.gameObject;
				print("******* OnCollisionEnter " + goHit);
				Part prtHit = FindPart(goHit);

				print("******* OnCollisionEnter prtHit " + prtHit);
				if (prtHit == null)
				{
					goHit = coll.contacts[0].otherCollider.gameObject;
					prtHit = FindPart(goHit);
				}
				print("******* OnCollisionEnter prtHit " + prtHit);

				if (prtHit.ProtectedBy != null)
				{
					foreach (string s in prtHit.ProtectedBy)
					{
						if (!Destroyed(s))
						{
							Destroy(other);
						}
						return;
					}
				}

				prtHit.Health -= Main.W_DEFS[p.Type].DamageOnHit;
				ShowLocalizedDamage(prtHit.mat);
				if (prtHit.Health <= 0)
				{
					prtHit.go.SetActive(false);
				}

				bool allDestroyed = true;
				foreach (var part in Parts)
				{
					if (!Destroyed(part))
					{
						allDestroyed = false;
						break;
					}
				}
				if (allDestroyed)
				{
					Main.S.ShipDestroyed(this);
					Destroy(gameObject);
				}
				Destroy(other);
				break;
		}
	}

	Part FindPart(string n)
	{
		foreach (var part in Parts)
		{
			if (part.Name == n)
			{
				return part;
			}
		}
		return null;
	}

	Part FindPart(GameObject go)
	{

		foreach (var part in Parts)
		{
			print("******* FindPart " + part + " ******** " + go);
			if (part.go == go)
			{
				return part;
			}
		}
		return null;
	}

	bool Destroyed(GameObject go)
	{
		return Destroyed(FindPart(go));
	}

	bool Destroyed(string n)
	{
		return Destroyed(FindPart(n));
	}

	bool Destroyed(Part part)
	{
		if (part == null)
		{
			return true;
		}
		return part.Health <= 0;
	}

	void ShowLocalizedDamage(Material m)
	{
		m.color = Color.red;
		RemainingDamageFrames = ShowDamageForFrames;
	}
}