using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{

	[SerializeField] private WeaponType _type;

	public WeaponType Type
	{
		get { return _type; }
		set { SetType(value); }
	}

	void Awake()
	{
		InvokeRepeating("CheckOffScreen", 2f, 2f);
	}

	public void SetType(WeaponType eType)
	{
		_type = eType;
		WeaponDefinition def = Main.S.GetWeaponDefinition(_type);
		GetComponent<Renderer>().material.color = def.ProjectileColor;
	}

	void CheckOffScreen()
	{
		if (Utils.ScreenBoundsCheck(GetComponent<Collider>().bounds, BoundsTest.OffScreen) != Vector3.zero)
		{
			Destroy(gameObject);
		}
	}
}
