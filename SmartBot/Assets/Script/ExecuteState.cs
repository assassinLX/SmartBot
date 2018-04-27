using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ExecuteState : MonoBehaviour
{
    public Vector3 StarPosition;

    public GameObject CurrentVector;
    public NavMeshAgent agent;

    public Button Btn_State;

    private CImage CImage_State;

    public enum State
    {
        run, stop, runFinish
    }
    public State isRun;

    public string[] Main;
    public string[] Sub_Main;
    public List<string> UniteStack;

    public GameObject Main_Character;
    public Animator controller;

    private Quaternion StartRotation;

    private void Awake()
    {
        isRun = State.stop;
        CImage_State = Btn_State.transform.GetComponent<CImage>();
        Btn_State.onClick.AddListener(() => Execute());
        StartRotation = new Quaternion(0, 0, 0, 0);
        StarPosition = new Vector3(-1.62f, -5.56f, 1.63f);
        UniteStack = new List<string>();
    }

    private void Start()
    {
        Main = GetComponent<FunctionUI>().instractStack;
        Sub_Main = GetComponent<FunctionUI>().subInstractStack;
    }

    private void Execute()
    {
        if (this.isRun == State.run)
        {
            this.isRun = State.stop;
        }
        else if (this.isRun == State.runFinish)
        {
            this.isRun = State.stop;
        }
        else
        {
            this.isRun = State.run;
        }
        if (Main[0] == null && this.isRun == State.run)
        {
            this.isRun = State.runFinish;
        }

        isCreateInstruct = false;
        isExecute = false;
        index = 0;
        StopAllCoroutines();
    }

    private bool isExecute = false;

    private void Update()
    {
        if (this.isRun == State.run)
        {
            //print("运行时");
            //"停止";
            CImage_State.displaySprite("运行时");
        }
        else if (this.isRun == State.stop)
        {
            //print("停止");
            //"运行";
            CImage_State.displaySprite("开始运行");
        }
        else
        {
            //print("运行完成");
            //"运行完毕";
            CImage_State.displaySprite("运行完成");
        }

        if (this.isRun == State.runFinish || this.isRun == State.stop)
        {
            return;
        }
        else
        {
            //1,创建出 指令集;
            createInstruct();
            //2,添加循环标示;
            //3,处理指令
            if(isExecute == false){
                executeInstruct();
                isExecute = true;
            }

        }

    }

    bool isCreateInstruct = false;

    private void createInstruct(){
        if (isCreateInstruct){
            return;
        }else{
            clearInstruct();
            for (int i = 0; i < Main.Length; i++)
            {
                if (Main[i] == "Function")
                {
                    var str = "ins";
                    UniteStack.Add(str);
                    for (int t = 0; t < Sub_Main.Length; t++)
                    {
                        if (Sub_Main[t] != null)
                        {
                            UniteStack.Add(Sub_Main[t]);
                        }
                    }
                }
                else
                {
                    if(Main[i] != null){
                        UniteStack.Add(Main[i]);
                    }
                }
            }
            isCreateInstruct = true;
        }
    }

    private void clearInstruct(){
        var len = UniteStack.Count;
        for (int i = 0; i < len;i++){
            UniteStack.RemoveAt(0);
        }
    }


    public int index = 0;
    public int pre = -1;

    private void executeInstruct(){
        if (index >= UniteStack.Count) {
            Debug.Log("应该切换状态");
            this.isRun = State.runFinish;
            return;
        }
        if(UniteStack[index] == null){
            return;
        }
        if(UniteStack[index] == "ins"){
            index++;
            pre = index;
        }
        if(UniteStack[index] == "Function"){
            index = pre;
        }
        executeCurrentInstruct();
    }

    private void executeCurrentInstruct(){
        Debug.Log("executeCurrentInstruct : " + UniteStack[index]);

        //延时函数：
        StartCoroutine(callBackInstruct());
    }

    private IEnumerator callBackInstruct()
    {
        yield return new WaitForSeconds(2);
        if(index < UniteStack.Count){
            index++;
        }
        executeInstruct();
    }





}
