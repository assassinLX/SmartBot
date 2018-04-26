using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChooseTerms : MonoBehaviour {

  

	public void Choose(string name)
    {
        Debug.Log(name);
        Sprite Sprite_term = (Sprite)Resources.Load("Time/"+name,typeof(Sprite));   //找到图片
        GetComponent<ColorManager>()._sprite = Sprite_term;  //赋值
        SceneManager.LoadScene("Main");   //动态加载
    }

   
}
