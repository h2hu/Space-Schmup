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

	public bool _____________________;
	public Bounds Bounds;
	public Vector3 BoundsCenterOffset;

	public Vector3 Pos
	{
		get { return this.transform.position; }
		set { this.transform.position = value; }
	}


	void Awake()
	{
		InvokeRepeating("CheckOffscreen", 0f, 2f);
	}
	void Update()
	{
		Move();
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
}