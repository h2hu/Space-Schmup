using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour
{
	public GameObject Poi;
	public GameObject[] Panels;
	public float ScrollSpeed = -30f;
	public float MotionMult = .25f;

	private float _panelHt;
	private float _depth;

	void Start()
	{
		_panelHt = Panels[0].transform.localScale.y;
		_depth = Panels[0].transform.position.z;

		Panels[0].transform.position = new Vector3(0, 0, _depth);
		Panels[1].transform.position = new Vector3(0, _panelHt, _depth);
	}

	void Update()
	{
		float tY, tX = 0;
		tY = Time.time * ScrollSpeed % _panelHt + (_panelHt * .5f);

		if (Poi != null)
		{
			print("Update ? " + Poi.transform.position.x + "********* ");
			tX = -Poi.transform.position.x * MotionMult;
			print("Update ? " + MotionMult + "********* " + tX);
		}

		Panels[0].transform.position = new Vector3(tX, tY, _depth);

		if (tY >= 0)
		{
			Panels[1].transform.position = new Vector3(tX, tY - _panelHt, _depth);
		}
		else
		{
			Panels[1].transform.position = new Vector3(tX, tY + _panelHt, _depth);
		}

	}
}