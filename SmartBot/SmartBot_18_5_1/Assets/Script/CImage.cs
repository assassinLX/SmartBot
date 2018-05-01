using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CImage : Image {
    
    public void displaySprite(string _name){
        if (_name == null) {
            this.color = new Color(1, 1, 1, 0);
            this.sprite = null;
        }else{
            this.sprite = (Sprite)Resources.Load("Icon/"+_name,typeof(Sprite));
            this.color = new Color(1, 1, 1, 1);
        }
    }
}
