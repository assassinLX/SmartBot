using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookieRotation : MonoBehaviour {

	private void Update()
	{
        transform.RotateAround(Vector3.up,Time.deltaTime);
	}

}
