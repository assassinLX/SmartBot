using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class start1Content : MonoBehaviour {

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
}
