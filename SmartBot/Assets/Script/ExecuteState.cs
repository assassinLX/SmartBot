using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExecuteState : MonoBehaviour {

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
        Debug.Log("UpdateRun:八月中秋白露，行人路上凄凉；");
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
        var targe = Main_Character.transform.position + Vector3.forward;
        var position = Main_Character.transform.position;
        var RelativeLocaltion = targe - position;
        for (float i = 0; i < RelativeLocaltion.magnitude; i += Time.deltaTime)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            Main_Character.transform.position += Vector3.forward * Time.deltaTime * 2;
            Debug.Log("1111");
        }
    }

}
