using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RQ_chooseLever : MonoBehaviour {

    public Button start;

    public void Awake()
    {
        //start = GameObject.Find("");
        start.onClick.AddListener(() => gotoOne());
    }

    public void gotoOne()
    {
        SceneManager.LoadScene("no1-1");
    }


}
