using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FunctionUI : MonoBehaviour {

    public GameObject Fct_background;  
    public GameObject Sub_Fct_background;
    

    public string[] instructionStack;  //主指令栈
    public string[] subInstructionStack;   //从指令栈
    public Button[] instructionBtns;    //指令按钮

    public string[] currentStack;   //当前指令栈

   // private Button chooseLever;


    private void Awake()
    {
        int currentInstructionStackNumber = Fct_background.transform.childCount;  //通过上指令框的btn数字得到栈的长度
        instructionStack = new string[currentInstructionStackNumber];  //开辟一个新的数组，赋给上指令栈
        int current_Sub_InstructionStackNumber = Sub_Fct_background.transform.childCount;
        subInstructionStack = new string[current_Sub_InstructionStackNumber];

        foreach (var btn in instructionBtns)
        {
            btn.onClick.AddListener(() => instractRespond(btn.name));   //给指令按钮添加监听，
        }
        //可能会变动------------------------------------------------
            for(var i = 0;i < Fct_background.transform.GetComponentsInChildren<Button>().Length; i++)
            {
                if (Fct_background.transform.GetComponentsInChildren<Button>()[i].gameObject != Fct_background)
                {
                    var current = Fct_background.transform.GetComponentsInChildren<Button>()[i];
                    current.onClick.AddListener(() => 
                    popStack(Fct_background, Sub_Fct_background, instructionStack, current.name));
                }
            }
            for(var i = 0; i < Sub_Fct_background.transform.GetComponentsInChildren<Button>().Length;i++)
            {
                if(Sub_Fct_background.transform.GetComponentsInChildren<Button>()[i].gameObject != Sub_Fct_background)
                {
                Button current = Sub_Fct_background.transform.GetComponentsInChildren<Button>()[i];
                current.onClick.AddListener(() =>
                    popStack(Sub_Fct_background, Fct_background, subInstructionStack,current.name));
                }
            }
        //----------------------------------------------------------

        Fct_background.GetComponent<Button>().onClick.AddListener(() 
            => chooseCurrentinstructionStack(Fct_background, Sub_Fct_background, instructionStack));

        Sub_Fct_background.GetComponent<Button>().onClick.AddListener(()
            => chooseCurrentinstructionStack(Sub_Fct_background, Fct_background, subInstructionStack));

        this.currentStack = instructionStack;
    }


    private void popStack(GameObject gameObj,GameObject setGameObjectColor, string[] CurrentStack, string name)
    {
        chooseCurrentinstructionStack(gameObj, setGameObjectColor, CurrentStack);
        Debug.Log(name);
        var index = int.Parse(name.Substring(name.Length-1, 1));
        popInCurrentStack(index);
    }

    private void popInCurrentStack(int index)
    {
        this.currentStack[index]=null;
        for(var i = 0; i < this.currentStack.Length-1 ;i++)
        {
            if(this.currentStack[i] == null)
            {
                if(this.currentStack[i+1] != null)
                {

                    this.currentStack[i] = this.currentStack[i + 1];
                    this.currentStack[i + 1] = null;
                }
            }
        }
    }

    private void chooseCurrentinstructionStack(GameObject gameObj,
        GameObject setGameObjectColor, string [] CurrentStack)
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
    
    private void instractRespond(string name)  //响应指令点击
    {
        if(GetCurrentInstructionNumber() >= this.currentStack.Length)   //已经使用的栈的数目大于当前的栈的长度
        {
            return;
        }
        else
        {
            for(int i = 0; i < this.currentStack.Length; i++)  // i小于当前栈的长度，就遍历一边，给空的赋值
            {
                if(this.currentStack[i] == null)
                {
                    this.currentStack[i] = name;
                    break;
                }
            }
        }
    }
    
    private int GetCurrentInstructionNumber()
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
            current_Fct_CImages[i].displaySprite(instructionStack[i]);
        }

        var current_SubFct_CImages = Sub_Fct_background.transform.GetComponentsInChildren<CImage>();
        for (int i = 0; i < current_SubFct_CImages.Length; i++)
        {
            current_SubFct_CImages[i].displaySprite(subInstructionStack[i]);
           
        }

    }


}
