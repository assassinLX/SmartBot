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
        agent.isStopped = true;
        Main_Character.transform.SetPositionAndRotation(StarPosition, StartRotation);
        StopAllCoroutines();
        setAnimator(false, true, false);
    }

    private void Update()
    {
        if (this.isRun == State.run)
        {
            //"停止";
            CImage_State.displaySprite("运行时");
        }
        else if (this.isRun == State.stop)
        {
            //"运行";
            CImage_State.displaySprite("开始运行");
        }
        else
        {
            //"运行完毕";
            CImage_State.displaySprite("运行完成");
        }

        if (this.isRun == State.runFinish || this.isRun == State.stop)
        {

            return;
        }
        else
        {
            StartCoroutine(UpdateRun(this.Main));
          
        }

    }

    private IEnumerator UpdateRun(string[] currentStack)
    {
        this.isRun = State.runFinish;
        foreach (var item in currentStack)
        {
            DealIns(item);
            yield return new WaitForSeconds(0.9f);

        }
    }

    private void DealIns(string ins){
        switch (ins)
        {
            case "GoAhead":
                Debug.Log("GoAhead");
                GoAhead();
                break;
            case "Light":
                Debug.Log("GoLight");
                GoLight();
                break;
            case "LeftRotation":
                Debug.Log("LeftRotation");
                GoLeftRotation();
                break;
            case "RightRotation":
                Debug.Log("RightRotation");
                GoRightRotaiton();
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


    void GoAhead()
    {
        StartCoroutine(move());
    }
    
    IEnumerator move()
    {
        setAnimator(true, false, false);
        yield return new WaitForSeconds(0.2f);

        agent.isStopped = false;
        var targe = CurrentVector.transform.position;
        agent.SetDestination(targe);
        //应该获取当前的我的位置 到目标位置的路径

        yield return new WaitForSeconds(0.3f);
        setAnimator(false, true, false);
        //Debug.Log("Move");
    }
    
    
    void GoLight(){
        StartCoroutine(light());
    }

    IEnumerator light(){
        setAnimator(false,false,true);
        yield return new WaitForSeconds(0.2f);

        var passObj = GameObject.FindWithTag(Content.PASS_GAME_TOLLGATE);
        if ( Vector3.Distance(passObj.transform.position,Main_Character.transform.position) < 1){
            var nextTollagteName = passObj.GetComponent<NextObj>().NextTollGate;
            SceneManager.LoadScene(nextTollagteName);
        }
        yield return new WaitForSeconds(0.3f);
        setAnimator(false,true,false);
    }



    void GoLeftRotation(){
        StartCoroutine(leftRotation());
    }

    IEnumerator leftRotation(){
        yield return new WaitForSeconds(0.1f);
        Main_Character.transform.Rotate(new Vector3(0,90,0));
    }

    void GoRightRotaiton(){
        StartCoroutine(rightRotation());
    }

    IEnumerator rightRotation(){
        yield return new WaitForSeconds(0.1f);
        Main_Character.transform.Rotate(new Vector3(0, -90, 0));
    }


    void Function()
    {
        StartCoroutine(UpdateRun(this.Sub_Main));
    }


    private void setAnimator(bool isRun,bool isIdle,bool isJump)
    {
        this.controller.SetBool("isRun",isRun);
        this.controller.SetBool("isIdle", isIdle);
        this.controller.SetBool("isJump", isJump);
    }


}
