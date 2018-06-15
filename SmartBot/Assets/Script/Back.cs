using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Back : MonoBehaviour {

	public void back()
    {
        SceneManager.LoadScene("Start");
    }

    public void gotoWantScene(string _name)
    {
        SceneManager.LoadScene(_name);
    }
}
