using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class Map : MonoBehaviour {

    public GameObject[] gameObjectMaps;
    public GameObject Main_Character;

    public GameObject[] Cookies;

    [HideInInspector]
    public bool[] cookiesResult;
    public Text personaeText;

    public GameObject UniteUI;
    public GameObject Mask;
    public GameObject [] Boms;

    public delegate void Active();
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


    public void mapSelectUpOrDown(string command,Active callBack){
        if(command == "Down"){
            StartCoroutine(mapFromMoveDown(callBack)); 
        }else{
            StartCoroutine(mapFromMoveUp()); 
        }
    }
    private IEnumerator mapFromMoveDown(Active callBack){
        foreach(var cube in Boms){
            var curPos = cube.transform.position - new Vector3(0,30,0);
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(setMapPosition(cube,curPos)); 
        }
        foreach(var cube in gameObjectMaps){
            var curPos = cube.transform.position - new Vector3(0,30,0);
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(setMapPosition(cube,curPos)); 
        }
        yield return new WaitForSeconds(0.5f);
        var curObjPos = Main_Character.transform.position - new Vector3(0,30,0);
        Main_Character.transform.DOMove(curObjPos,0.3f);
        yield return new WaitForSeconds(0.3f);
        callBack();
    }
    private IEnumerator mapFromMoveUp(){
        //降低坐标
        var starObjPos = Main_Character.transform.position - new Vector3(0,30,0);
        Main_Character.transform.position = starObjPos;
        if(Boms != null){
            foreach(var cube in Boms){
                var curPos = cube.transform.position - new Vector3(0,30,0);
                cube.transform.position = curPos;
            }
        }
        foreach(var cube in gameObjectMaps){
             var curPos = cube.transform.position - new Vector3(0,30,0);
             cube.transform.position = curPos;
        }
        foreach(var cube in Cookies){
             var curPos = cube.transform.position - new Vector3(0,30,0);
             cube.transform.position = curPos;
        }
        //恢复
        UniteUI = GameObject.Find("UniteUI");
        var Canvas = UniteUI.transform.FindChild("Canvas");
        Mask = (GameObject)Resources.Load("Mask/MaskPanel");
        var maskObj = GameObject.Instantiate(Mask,Canvas.transform);

        foreach(var cube in gameObjectMaps){
            var curPos = cube.transform.position + new Vector3(0,30,0);
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(setMapPosition(cube,curPos));   
        }

        foreach(var cube in Boms){
            var curPos = cube.transform.position + new Vector3(0,30,0);
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(setMapPosition(cube,curPos));   
        }

        foreach(var cube in Cookies){
            var curPos = cube.transform.position + new Vector3(0,30,0);
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(setMapPosition(cube,curPos));   
        }

        yield return new WaitForSeconds(0.3f);
        var endObjPos = Main_Character.transform.position + new Vector3(0,30,0);
        Main_Character.transform.DOMove(endObjPos,0.3f);
        Destroy(maskObj);
    }


    private IEnumerator setMapPosition(GameObject curObj,Vector3 pos){
        yield return new WaitForSeconds(0.3f);
        curObj.transform.DOMove(pos,0.3f);
    }



}
