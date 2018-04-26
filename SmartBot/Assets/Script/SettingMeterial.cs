using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingMeterial : MonoBehaviour {

	public Material other2;
	public GameObject _manager;

	void Start () {
		_manager = GameObject.Find ("Manager");
		var sprite = _manager.GetComponent<ColorManager> ()._sprite;
		other2.mainTexture = sprite.texture;
	}
	
 
}
