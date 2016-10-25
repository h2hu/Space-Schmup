using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour
{
	public static Hero S;

	public float Speed = 30f;
	public float RollMult = -45;
	public float PitchMult = 30;
	public float ShieldLevel = 1;
	public bool _____________________;

	public Bounds Bounds;

	void Awake()
	{
		S = this;
		Bounds = Utils.CombineBoundsOfChildren(gameObject);
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
		Vector3 off = Utils.ScreenBoundsCheck(Bounds, BoundsTest.OnScreen);
		if (off != Vector3.zero)
		{
			pos -= off;
			transform.position = pos;
		}

		transform.rotation = Quaternion.Euler(yAxis * PitchMult, xAxis * RollMult, 0);
	}
}