using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour {
    public Sprite _sprite;

    // Use this for initialization
    public static ColorManager instance = null;

    void Awake()
    {
        if (ColorManager.instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);    //不释放自己，第二件事，不释放我找到的图
    }

}
