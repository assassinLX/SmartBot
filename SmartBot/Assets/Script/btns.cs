using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class btns : MonoBehaviour
{
    public ScrollRect _ScrollRect;

    public RectTransform[] contents;


    public void chooseSeason(int id)
    {
        hide(id);
        _ScrollRect.content = contents[id - 1];
    }

    void hide(int id)
    {
        int id_targe = id - 1;
        for (int i = 0; i < contents.Length; i++)
        {
            if (id_targe == i)
            {
                contents[i].gameObject.SetActive(true);
            }
            else
            {
                contents[i].gameObject.SetActive(false);
            }

        }
    }  //春夏秋冬底下四个滑动条的内容显示和隐藏

    public void Close()
    {
        SceneManager.LoadScene("star");
        Destroy(GameObject.Find("Manager"));
    }


    public void TiaoZhuJieMian()
    {
        SceneManager.LoadScene("enter");
    }

    public void TiaoTuSe()
    {
        SceneManager.LoadScene("star");
        Destroy(GameObject.Find("Manager"));
    }

    public void TiaoEnter()
    {
        SceneManager.LoadScene("Enter");
        }

    public void TiaoStart2()
    {
        SceneManager.LoadScene("start2");
        Destroy(GameObject.Find("manager"));
    }

}
