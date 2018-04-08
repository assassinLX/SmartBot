using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ExecuteState : MonoBehaviour {
    public Vector3 StarPosition;
    public GameObject CurrentVector;
    public NavMeshAgent agent;

    public Button Btn_State;
    private Text Text_State;
    
    public enum State
    {
        run,stop, runFinish
    }
    public State isRun;

    public string[] Main;
    public string[] Sub_Main;

    public GameObject Main_Character;
    public Animator controller;

    private void Awake()
    {
        isRun = State.stop;
        Text_State = Btn_State.transform.GetChild(0).GetComponent<Text>();
        Btn_State.onClick.AddListener(()=> Execute());

    }

    private void Start()
    {
        Main = GetComponent<FunctionUI>().instractStack;
        Sub_Main = GetComponent<FunctionUI>().subInstractStack;

    }

    private void Execute()
    {
        if(this.isRun == State.run)
        {
            this.isRun = State.stop;
        }
        else if(this.isRun == State.runFinish)
        {
            this.isRun = State.stop;
        }
        else
        {
            this.isRun = State.run;
        }
        if(Main[0] == null && this.isRun == State.run)
        {
            this.isRun = State.runFinish;
        }
        agent.isStopped = true;
        Main_Character.transform.position = StarPosition;
        setAnimator(false, false, false);
    }

    private void Update()
    {
        if (this.isRun == State.run)
        {
            Text_State.text = "停止";
        }
        else if(this.isRun == State.stop)
        {
            Text_State.text = "运行";
        }
        else
        {
            Text_State.text = "运行完毕";
        }

        if(this.isRun == State.runFinish || this.isRun == State.stop)
        {
            return;
        }
        else
        {
            UpdateRun(this.Main);
        }

    }

    private void UpdateRun(string [] currentStack)
    {
        
        foreach (var item in currentStack)
        {
            switch (item)
            {
                case "GoAhead":
                    Debug.Log("GoAhead");
                    GoAhead();
                    break;
                case "Light":
                    Debug.Log("Light");
                    break;
                case "LeftRotation":
                    Debug.Log("LeftRotation");
                    break;
                case "RightRotation":
                    Debug.Log("RightRotation");
                    break;
                case "Jump":
                    Debug.Log("Jump");
                    break;
                case "Function":
                    Function();
                    Debug.Log("Function");
                    break;
            }
        }
        this.isRun = State.runFinish;
    }

    
    void GoAhead()
    {
        StartCoroutine(move());
    }
    
    IEnumerator move()
    {
        setAnimator(true, false, false);
        agent.isStopped = false;
        yield return new WaitForSeconds(0.2f);
        var targe = CurrentVector.transform.position;
        agent.SetDestination(targe);
        //应该获取当前的我的位置 到目标位置的路径
        yield return new WaitForSeconds(0.1f);
        setAnimator(false, true, false);
    }
    
    void Light()
    {

    }

    void LeftRotation()
    {

    }

    void RightRotation()
    {

    }

    void Jump()
    {

    }

    void Function()
    {
        UpdateRun(this.Sub_Main);
    }


    private void setAnimator(bool isRun,bool isIdle,bool isJump)
    {
        this.controller.SetBool("isRun",isRun);
        this.controller.SetBool("isIdle", isIdle);
        this.controller.SetBool("isJump", isJump);
    }

}
