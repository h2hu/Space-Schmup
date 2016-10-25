using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BoundsTest
{
	Center,
	OnScreen,
	OffScreen
}

public class Utils : MonoBehaviour
{
	public static Bounds BoundsUnion(Bounds b0, Bounds b1)
	{
		if (b0.size == Vector3.zero)
		{
			return b1;
		}

		if (b1.size == Vector3.zero)
		{
			return b0;
		}

		b0.Encapsulate(b1.min);
		b0.Encapsulate(b1.max);
		return b0;
	}

	public static Bounds CombineBoundsOfChildren(GameObject go)
	{
		Bounds b = new Bounds(Vector3.zero, Vector3.zero);
		if (go.GetComponent<Renderer>() != null)
		{
			b = BoundsUnion(b, go.GetComponent<Renderer>().bounds);
		}
		if (go.GetComponent<Collider>() != null)
		{
			b = BoundsUnion(b, go.GetComponent<Collider>().bounds);
		}

		foreach (Transform transform in go.transform)
		{
			b = BoundsUnion(b, CombineBoundsOfChildren(transform.gameObject));
		}
		return b;
	}

	private static Bounds _camBounds;

	public static Bounds CamBounds
	{
		get
		{
			if (_camBounds.size == Vector3.zero)
			{
				SetCameraBounds();
			}
			return _camBounds;
		}
	}

	public static void SetCameraBounds(Camera cam = null)
	{
		if (cam == null)
		{
			cam = Camera.main;
		}
		Vector3 topLeft = new Vector3(0, 0, 0);
		Vector3 bottomRight = new Vector3(Screen.width, Screen.height, 0);

		Vector3 boundTLN = cam.ScreenToWorldPoint(topLeft);
		Vector3 boundBRF = cam.ScreenToWorldPoint(bottomRight);

		boundTLN.z += cam.nearClipPlane;
		boundBRF.z += cam.farClipPlane;

		Vector3 center = (boundTLN + boundBRF) / 2f;

		_camBounds = new Bounds(center, Vector3.zero);
		_camBounds.Encapsulate(boundTLN);
		_camBounds.Encapsulate(boundBRF);
	}

	public static Vector3 ScreenBoundsCheck(Bounds bnd, BoundsTest test = BoundsTest.Center)
	{
		return BoundsInBoundsCheck(CamBounds, bnd, test);
	}

	public static Vector3 BoundsInBoundsCheck(Bounds bigB, Bounds lilB, BoundsTest test = BoundsTest.OnScreen)
	{
		Vector3 pos = lilB.center;
		Vector3 off = Vector3.zero;


		switch (test)
		{
			case BoundsTest.Center:
				if (bigB.Contains(pos))
				{
					return Vector3.zero;
				}

				for (var i = 0; i < 3; i++)
				{
					if (pos[i] > bigB.max[i])
					{
						off[i] = pos[i] - bigB.max[i];
					}
					else if (pos[i] < bigB.min[i])
					{
						off[i] = pos[i] - bigB.min[i];
					}
				}

				return off;
			case BoundsTest.OnScreen:
				if (bigB.Contains(lilB.min) && bigB.Contains(lilB.max))
				{
					return Vector3.zero;
				}

				for (var i = 0; i < 3; i++)
				{
					if (lilB.max[i] > bigB.max[i])
					{
						off[i] = lilB.max[i] - bigB.max[i];
					}
					else if (lilB.min[i] < bigB.min[i])
					{
						off[i] = lilB.min[i] - bigB.min[i];
					}
				}
				return off;
			case BoundsTest.OffScreen:

				if (bigB.Contains(lilB.min) || bigB.Contains(lilB.max))
				{
					return Vector3.zero;
				}

				for (var i = 0; i < 3; i++)
				{
					if (lilB.min[i] > bigB.max[i])
					{
						off[i] = lilB.min[i] - bigB.max[i];
					}
					else if (lilB.max[i] < bigB.min[i])
					{
						off[i] = lilB.max[i] - bigB.min[i];
					}
				}
				return off;
			default:
				return Vector3.zero;
		}
	}

	public static GameObject FindTaggedParent(GameObject go)
	{
		if (go.tag != "Untagged")
		{
			return go;
		}
		if (go.transform.parent == null)
		{
			return null;
		}
		return FindTaggedParent(go.transform.parent.gameObject);
	}

	public static GameObject FindTaggedParent(Transform t)
	{
		return FindTaggedParent(t.gameObject);
	}

}