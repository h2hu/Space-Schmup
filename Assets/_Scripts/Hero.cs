using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour
{
	public static Hero S;

	public float GameRestartDelay = 2f;
	public float Speed = 30f;
	public float RollMult = -45;
	public float PitchMult = 30;

	[SerializeField]
	private float _shieldLevel = 1;

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
		} }

	public BoundsTest BoundsTestOption = BoundsTest.Center;
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
}