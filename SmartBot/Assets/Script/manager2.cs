using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class manager2 : MonoBehaviour {

    public string mingzi;

    // Use this for initialization
    public static manager2 instance = null;

    void Awake()
    {
        if (manager2.instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);    //不释放自己，第二件事，不释放我找到的图
    }

    public void qietu(string name) {
        mingzi = name;
        Debug.Log(mingzi);
        SceneManager.LoadScene("pintu");
    }

}
