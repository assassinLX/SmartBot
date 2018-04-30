using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour {

    public GameObject[] gameObjectMaps;
    public GameObject Main_Character;

    public GameObject[] Cookies;

    [HideInInspector]
    public bool[] cookiesResult;
    public Text personaeText;

	private void Awake()
	{
        personaeText.text = "0/" + Cookies.Length;
        cookiesResult = new bool[Cookies.Length];
        cookiesResetting();
	}

    public void cookiesResetting(){
        for (int i = 0; i < Cookies.Length ;i++){
            cookiesResult[i] = false;
            Cookies[i].SetActive(true);
        }
    }


    public GameObject getNextStepCube(){
        var nextStepPosition = Main_Character.transform.position + Main_Character.transform.forward;

        GameObject StepCube = null;

        if(gameObjectMaps[0] != null){

            StepCube = gameObjectMaps[0];

            var distanceInit = Vector3.Distance(nextStepPosition,gameObjectMaps[0].transform.position);

            for (int i = 0; i < gameObjectMaps.Length ;i++){
                var distance = Vector3.Distance(gameObjectMaps[i].transform.position,nextStepPosition);
                if(distance <= distanceInit){
                    StepCube = gameObjectMaps[i];
                    distanceInit = distance;
                }
            }

        }else{
            Debug.LogError("gameObjectMaps 为空！！！！！");
        }
    
        return StepCube;
    }

}
