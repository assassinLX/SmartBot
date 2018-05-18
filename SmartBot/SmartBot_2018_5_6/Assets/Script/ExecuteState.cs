using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class ExecuteState : MonoBehaviour
{
    public GameObject changeScenePanel;
    public Button changeSceneBtn;

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

    public Map mapManager; 

    private int realCookiesNumber;


    private int getCookiesNum
    {
        set{
            realCookiesNumber = value;
            mapManager.personaeText.text = value + "/" + mapManager.Cookies.Length;
        }
        get{
            return realCookiesNumber;
        }
    }



    private void Awake()
    {
        isRun = State.stop;   //开始状态为stop
        CImage_State = Btn_State.transform.GetComponent<CImage>();   //给Cimage赋值，获取CImage组件
        Btn_State.onClick.AddListener(() => Execute());     //监听运行按钮，Ececute有三种状态
        StartRotation = new Quaternion(0, 0, 0, 0);
        UniteStack = new List<string>();
        changeSceneBtn.onClick.AddListener(() => NextScene());
        changeScenePanel.SetActive(false);
        mapManager = GameObject.FindWithTag("Map").GetComponent<Map>();

    }

    private void Start()
    {
        Main = GetComponent<FunctionUI>().instractStack;
        Sub_Main = GetComponent<FunctionUI>().subInstractStack;
    }

    private void Execute()
    {
        if (this.isRun == State.run)  //运行状态为run
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
        Resetting();   //运行完毕，重置
    }

	private void Resetting()   
	{
        isCreateInstruct = false;
        isExecute = false;
        index = 0;
        StopAllCoroutines();
        Main_Character.transform.position = new Vector3(0, 0, 0);
        setAnimator(false, true, false);
        Main_Character.transform.rotation = StartRotation;
        mapManager.cookiesResetting();
        getCookiesNum = 0;
	}



	private bool isExecute = false;
    private void Update()   //执行状态
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
            Debug.Log("update createIns");
            //2,添加循环标示;
            //3,处理指令
            if(isExecute == false){
                executeInstruct();
                isExecute = true;
            }

        }

    }

    bool isCreateInstruct = false;

    private void createInstruct(){   //给统一栈赋值
        if (isCreateInstruct){
            return;
        }else{
            clearInstruct();
            for (int i = 0; i < Main.Length; i++)
            {
                if (Main[i] == "Function")  //主栈遇到function添加ins标记位置
                {
                    if(Sub_Main[0] != null){
                        var str = "ins";
                        UniteStack.Add(str);
                    }
                    for (int t = 0; t < Sub_Main.Length; t++)
                    {
                        if (Sub_Main[t] != null)    //从栈遇到function，就是function
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

    private void executeInstruct(){     //执行 。统一栈的三种指令，普通指令，ins，function
        if (index >= UniteStack.Count) {
            Debug.Log("应该切换状态");
            this.isRun = State.runFinish;
            return;
        }
        if(UniteStack[index] == null){
            return;
        }
        if(UniteStack[index] == "ins"){
            if (index < UniteStack.Count)
            {
                index++;
            }
            pre = index;
        }
        if(UniteStack[index] == "Function"){
            index = pre;
        }
        executeCurrentInstruct();
    }

    private void executeCurrentInstruct(){  
        //处理指令:
        dealInstruct(UniteStack[index]);
    }

    private void callBackInstruct(){
        if(index < UniteStack.Count){
            index++;
        }
        executeInstruct();
    }

    private void dealInstruct(string ins){
        switch (ins)
        {
            case "GoAhead":
                GoAhead();
                break;
            case "Light":
                GoLight();
                break;
            case "LeftRotation":
                GoLeftRotation();
                break;
            case "RightRotation":
                GoRightRotation();
                break;
            case "Jump":
                GoJump();
                break;
            default:
                break;
        }
    }

    private void GoAhead(){
        StartCoroutine(move());
    }

    private IEnumerator move()
    {
        setAnimator(true, false, false);
        yield return new WaitForSeconds(0.2f);

        if (mapManager != null){
            Debug.Log("Move");
            var nextStep = Main_Character.transform.forward + Main_Character.transform.position;//我的方向加我的位置
            var nextStepCube = mapManager.getNextStepCube();

            if(nextStepCube != null){
                var precisenessStep = nextStepCube.transform.position + new Vector3(0,0.5f,0);
                if (Vector3.Distance(nextStep,precisenessStep) < 0.2f)
                {
                    //此时说明我的下一步 可以走
                    Main_Character.transform.DOMove(precisenessStep, 0.3f);
                }
            }
           
        }
        yield return new WaitForSeconds(0.3f);
        setAnimator(false, true, false);
        yield return new WaitForSeconds(0.3f);
        callBackInstruct();
    }

    private void GoLight(){
        StartCoroutine(_light());
        Debug.Log("Light");
    }


    private IEnumerator _light(){
        setAnimator(false, false, true);
        yield return new WaitForSeconds(0.3f);

        if(mapManager != null){
            var cookies = mapManager.Cookies;
            var cookiesResult = mapManager.cookiesResult;
            var personaeText = mapManager.personaeText;

            for (int i = 0; i < cookies.Length ;i++){
                var curCookiesObjPos = cookies[i].transform.position;
                //Debug.Log(Vector3.Distance(curCookiesObjPos, Main_Character.transform.position));
                if(Vector3.Distance(curCookiesObjPos,Main_Character.transform.position) < 0.7f && cookiesResult[i] == false){
                    //人物接触到了饼干
                    getCookiesNum++;
                    personaeText.text = getCookiesNum + "/" + cookies.Length;
                    cookies[i].SetActive(false);
                    cookiesResult[i] = true;
                    if (getCookiesNum >= cookies.Length) {
                        changeScenePanel.SetActive(true);
                        this.isRun = State.runFinish;
                        StopAllCoroutines();
                        setAnimator(false, true, false);
                    }
                }
            }
        }
        yield return new WaitForSeconds(0.3f);
        setAnimator(false, true, false);
        yield return new WaitForSeconds(0.4f);
        callBackInstruct();
    }

    private void GoLeftRotation(){
        Debug.Log("LeftRotation");
        StartCoroutine(leftRotation());
    }

    private IEnumerator leftRotation()
    {
        yield return new WaitForSeconds(0.1f);
        Main_Character.transform.DOBlendableRotateBy(new Vector3(0, -90, 0), 0.3f);
        yield return new WaitForSeconds(0.3f);
        callBackInstruct();
    }

    private void GoRightRotation(){
        Debug.Log("RightRotation");
        StartCoroutine(righitRotation());
    }

    private IEnumerator righitRotation(){
        yield return new WaitForSeconds(0.1f);
        Main_Character.transform.DOBlendableRotateBy(new Vector3(0, 90, 0), 0.3f);
        yield return new WaitForSeconds(0.3f);
        callBackInstruct();
    }


    private void GoJump(){
        Debug.Log("Jump");
        StartCoroutine(jump());
        
    }

    private IEnumerator jump(){
        setAnimator(false, false, true);
        yield return new WaitForSeconds(0.3f);

        
        var nextStep = Main_Character.transform.forward + Main_Character.transform.position;

        if (mapManager != null)
        {
            var nextStepCube = mapManager.getNextStepCube();
            if (nextStepCube != null)
            {
                
                var precisenessStep = nextStepCube.transform.position + new Vector3(0, 0.5f, 0);

                Debug.Log("跳 ： 下一步的距离 "+Vector3.Distance(nextStep, precisenessStep));
                if (Vector3.Distance(nextStep, precisenessStep) < 1.0f && Vector3.Distance(nextStep, precisenessStep) >= 0.5f)
                {
                    //此时说明我的下一步 可以走
                    Main_Character.transform.DOMove(precisenessStep, 0.3f);
                }
            }
          
        }


        yield return new WaitForSeconds(0.3f);
        setAnimator(false, true, false);
        yield return new WaitForSeconds(0.4f);
        callBackInstruct();
    }


    private void NextScene(){
        Debug.Log("NextScene");
        var currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentScene + 1;
        if (nextScene < SceneManager.sceneCountInBuildSettings){
            //print(nextScene);
            SceneManager.LoadScene(nextScene);
        }else{
            SceneManager.LoadScene("Start");
        }
    }

    private void setAnimator(bool isRuning, bool isIdle, bool isJump)
    {
        this.controller.SetBool("isRun", isRuning);
        this.controller.SetBool("isIdle", isIdle);
        this.controller.SetBool("isJump", isJump);
    }
}
