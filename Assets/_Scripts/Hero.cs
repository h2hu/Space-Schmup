using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour
{
	public static Hero S;

	public float GameRestartDelay = 2f;
	public float Speed = 30f;
	public float RollMult = -45;
	public float PitchMult = 30;

	[SerializeField] private float _shieldLevel = 1;

	public float ShieldLevel
	{
		get { return _shieldLevel; }
		set
		{
			_shieldLevel = Mathf.Min(value, 4);
			if (value < 0)
			{
				Destroy(gameObject);
				Main.S.DelayedRestart(GameRestartDelay);
			}
		}
	}

	public BoundsTest BoundsTestOption = BoundsTest.Center;

	public Weapon[] Weapons;
	public bool _____________________;

	public Bounds Bounds;
	public GameObject LastTriggerGo;

	public delegate void WeaponFireDelegate();

	public WeaponFireDelegate FireDelegate;

	void Awake()
	{
		S = this;
		Bounds = Utils.CombineBoundsOfChildren(gameObject);

	}

	void Start()
	{
		ClearWeapons();
		Weapons[0].SetType(WeaponType.Blaster);
	}

	void Update()
	{
		var xAxis = Input.GetAxis("Horizontal");
		var yAxis = Input.GetAxis("Vertical");

		var pos = transform.position;
		pos.x += xAxis * Speed * Time.deltaTime;
		pos.y += yAxis * Speed * Time.deltaTime;
		transform.position = pos;

		Bounds.center = transform.position;
		Vector3 off = Utils.ScreenBoundsCheck(Bounds, BoundsTestOption);
		if (off != Vector3.zero)
		{
			pos -= off;
			transform.position = pos;
		}

		transform.rotation = Quaternion.Euler(yAxis * PitchMult, xAxis * RollMult, 0);

		if (Input.GetAxis("Jump") == 1 && FireDelegate != null)
		{
			FireDelegate();
		}
	}

	void OnTriggerEnter(Collider other)
	{
		GameObject go = Utils.FindTaggedParent(other.gameObject);
		if (go != null)
		{
			if (go == LastTriggerGo)
			{
				return;
			}
			LastTriggerGo = go;
			if (go.tag == "Enemy")
			{
				ShieldLevel--;
				Destroy(go);
			}
			else if (go.tag == "PowerUp")
			{
				AbsorbPowerUp(go);
			}
			else
			{
				print("Triggered : " + go.name);
			}
		}
		else
		{
			print("Triggered : " + other.gameObject.name);
		}
	}

	public void AbsorbPowerUp(GameObject go)
	{
		print("AbsorbPowerUp : " + go.name);
		PowerUp pu = go.GetComponent<PowerUp>();
		print("AbsorbPowerUp : " + pu.Type);
		switch (pu.Type)
		{
			case WeaponType.Shield:
				ShieldLevel++;
				break;
			default:
				if (pu.Type == Weapons[0].Type)
				{
					Weapon w = GetEmptyWeaponSlot();
					if (w != null)
					{
						w.SetType(pu.Type);
					}
				}
				else
				{
					ClearWeapons();
					Weapons[0].SetType(pu.Type);
				}
				break;
		}
		pu.AbsorbedBy(gameObject);
	}


	Weapon GetEmptyWeaponSlot()
	{
		for (int i = 0; i < Weapons.Length; i++)
		{
			if (Weapons[i].Type == WeaponType.None)
			{
				return Weapons[i];
			}
		}
		return null;
	}

	void ClearWeapons()
	{
		foreach (var weapon in Weapons)
		{
			weapon.SetType(WeaponType.None);
		}
	}
}