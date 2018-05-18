using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class FunctionUI : MonoBehaviour {

    public GameObject Fct_background;  
    public GameObject Sub_Fct_background;
    

    public string[] instractStack;  //主栈
    public string[] subInstractStack;   //从栈
    public Button[] instractBtns;    

    public string[] currentStack;   

   // private Button chooseLever;


    private void Awake()
    {
        int currentInstractStackNumber = Fct_background.transform.childCount;  //当前栈的长度=界面上指令框的子对象btn的数目
        instractStack = new string[currentInstractStackNumber];  //开辟一个新的数组，赋给当前的上指令栈
        int current_Sub_InstractStackNumber = Sub_Fct_background.transform.childCount;
        subInstractStack = new string[current_Sub_InstractStackNumber];

        foreach (var btn in instractBtns)
        {
            btn.onClick.AddListener(() => instractRespond(btn.name)); //给指令按钮添加监听，将btn名字传给instractRespond
        }
        //可能会变动------------------------------------------------
            for(var i = 0;i < Fct_background.transform.GetComponentsInChildren<Button>().Length; i++)
            {
                if (Fct_background.transform.GetComponentsInChildren<Button>()[i].gameObject != Fct_background)
                {
                    var current = Fct_background.transform.GetComponentsInChildren<Button>()[i];
                    current.onClick.AddListener(() => 
                    popStack(Fct_background, Sub_Fct_background, instractStack, current.name));
                }
            }
            for(var i = 0; i < Sub_Fct_background.transform.GetComponentsInChildren<Button>().Length;i++)
            {
                if(Sub_Fct_background.transform.GetComponentsInChildren<Button>()[i].gameObject != Sub_Fct_background)
                {
                Button current = Sub_Fct_background.transform.GetComponentsInChildren<Button>()[i];
                current.onClick.AddListener(() =>
                    popStack(Sub_Fct_background, Fct_background, subInstractStack,current.name));
                }
            }
        //----------------------------------------------------------

        Fct_background.GetComponent<Button>().onClick.AddListener(() 
            => chooseCurrentinstractStack(Fct_background, Sub_Fct_background, instractStack));

        Sub_Fct_background.GetComponent<Button>().onClick.AddListener(()
            => chooseCurrentinstractStack(Sub_Fct_background, Fct_background, subInstractStack));

        this.currentStack = instractStack;   //点击的栈赋给当前栈，使用当前栈管理指令进栈
    }

    public GameObject helpGameObject;
    public GameObject goBackGameObject;
    private void Start()
    {
        helpGameObject = GameObject.Find("help");
        var helpBtn = helpGameObject.GetComponent<Button>();
        if (helpBtn == null)
        {
            helpGameObject.AddComponent<Button>();
            helpBtn = helpGameObject.GetComponent<Button>();
        }

        goBackGameObject = GameObject.Find("BackToLevel2");
        var goBackBtn = goBackGameObject.GetComponent<Button>();
        if (goBackBtn == null)
        {
            goBackGameObject.AddComponent<Button>();
            goBackBtn = goBackGameObject.GetComponent<Button>();
        }
        helpBtn.onClick.AddListener(() => SceneManager.LoadScene("help"));
        goBackBtn.onClick.AddListener(() => SceneManager.LoadScene("Choose"));
    }

    private void popStack(GameObject gameObj,GameObject setGameObjectColor, string[] CurrentStack, string name)
    {
        chooseCurrentinstractStack(gameObj, setGameObjectColor, CurrentStack);
        Debug.Log(name);
        var index = int.Parse(name.Substring(name.Length-1, 1));  //取btn最后一个字符串，然后转为int
      
        popInCurrentStack(index);
    }

    private void popInCurrentStack(int index)
    {
        this.currentStack[index]=null; //当前栈某一个元素为空
        for(var i = 0; i < this.currentStack.Length-1 ;i++)
        {
            if(this.currentStack[i] == null) //当前栈的某一个元素为空，将后一位赋给前一位，并将后一位赋空
            {
                if(this.currentStack[i+1] != null)
                {

                    this.currentStack[i] = this.currentStack[i + 1];
                    this.currentStack[i + 1] = null;
                }
            }
        }
    }

    private void chooseCurrentinstractStack(GameObject gameObj,
        GameObject setGameObjectColor, string [] CurrentStack)     //切换当前栈
    {
        this.currentStack = CurrentStack;
        foreach (var image in gameObj.transform.GetComponentsInChildren<Image>())
        {
            if(image.gameObject == gameObj)
            {
                continue;
            }
            //image.color = new Color(0.74f,0.72f,0,1f); //被选中的颜色
            image.color = new Color(241.0f/255.0f,228.0f/255.0f,153.0f/255.0f);
        }

        foreach (var image in setGameObjectColor.transform.GetComponentsInChildren<Image>())
        {
            if (image.gameObject == setGameObjectColor)
            {
                continue;
            }
            //image.color = new Color( 0.76f , 0.76f, 0.76f, 1);  //没有被选择的颜色
            image.color = new Color(204.0f/255.0f,189.0f/255.0f,144.0f/255.0f,1);
        }
    }
    
    private void instractRespond(string name)  //响应指令点击,进栈
    {
        if(GetCurrentInstractNumber() >= this.currentStack.Length)   //已经使用的栈的数目大于当前的栈的长度
        {
            return;
        }
        else
        {
            for(int i = 0; i < this.currentStack.Length; i++)  // i小于当前栈的长度，就遍历一边，给空的赋值
            {
                if(this.currentStack[i] == null)
                {
                    this.currentStack[i] = name;  //把名字赋给当前栈，进栈
                    break;
                }
            }
        }
    }
    
    private int GetCurrentInstractNumber()
    {
        var i = 0;
        foreach(var stack in this.currentStack)
        {
            if(stack == "" || stack == null)
            {
                i = i+1;  //没有使用的栈的数目
            }
        }
        return (this.currentStack.Length - i); //已经使用的栈的数目
    }


    private void FixedUpdate()  //FixedUpdate是固定时间
    {
        UpdateUI();
    }

    private void UpdateUI()  //每一帧被调用
    {
        var current_Fct_CImages = Fct_background.transform.GetComponentsInChildren<CImage>();
        for(int i = 0; i < current_Fct_CImages.Length; i++)
        {
            current_Fct_CImages[i].displaySprite(instractStack[i]);
        }

        var current_SubFct_CImages = Sub_Fct_background.transform.GetComponentsInChildren<CImage>();
        for (int i = 0; i < current_SubFct_CImages.Length; i++)
        {
            current_SubFct_CImages[i].displaySprite(subInstractStack[i]);
           
        }

    }


}
