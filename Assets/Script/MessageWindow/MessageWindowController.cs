/**
 * MessageWindow.cs
 * 
 * メッセージウィンドウに対する処理を行う. 
 *
 * @author ys.ohta
 * @version 1.0
 * @date 2016/08/XX
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Text;
using System;

/**
 * MessageWindowController
 * 
 */
public class MessageWindowController : MonoBehaviour
{

    //メッセージウィンドウの表示・非表示
    public bool enables = true;
    //メッセージウィンドウオブジェクト
    public GameObject messageWindow;

    //メッセージウィンドウに表示するテキストシナリオ
    [SerializeField]
    private string[] scenario;
    //シナリオのページ数
    private int page = 0;
    //ページ内の表示されている文字数
    private int index = 0;
    //ページ内のすべての文字が表示されたかどうか
    private bool isEnd = false;
    //現在の時刻
    private float currentTime;
    //表示するメッセージ
    [SerializeField]
    private Text message;
    //表示する話者名
    [SerializeField]
    private Text speaker;
    //ページ切り替え待ちシンボル
    [SerializeField]
    private Text waitSimbol;
    //ページ切り替え待ちシンボルの表示・非表示
    private bool waitSimbolenabled = false;
    //ページ切り替え待ちシンボル点滅スピード
    public float speed = 0.15f;
    //文字表示スピード
    public float messageSpeed = 0.04f;
    //Delimiter
    private string[] delimiter = { "@page\r\n" };
    //Delimiter
    private string[] newLine = { "\r\n" };

    void Start()
    {
        currentTime = Time.time;
        scenario = getScenariosFromFile("Resources/Scenario.txt");
        string[] tmp = scenario[page].Split(newLine, StringSplitOptions.RemoveEmptyEntries);
        speaker.text = tmp[0];
        scenario[page] = scenario[page].Substring(tmp[0].Length+2);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            //Aボタンを押されるとメッセージウィンドウの表示切替
            enables = !enables;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            //Sボタンを押されるとページの最初から文字を表示
            reset();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (isEnd)
            {

                if (page < scenario.Length - 1)
                {
                    page++;
                    string[] tmp = scenario[page].Split(newLine, StringSplitOptions.RemoveEmptyEntries);
                    speaker.text = tmp[0];
                    scenario[page] = scenario[page].Substring(tmp[0].Length+2);
                    reset();
                }
            } else
            {
                end();
            }
        }
        waitSimbol.gameObject.SetActive(waitSimbolenabled);
        messageWindow.SetActive(enables);
        process();
    }

    /**
     * 文字を1文字ずつ表示する処理および, ページ移動待ちシンボルの点滅処理を行う.
     */
    void process()
    {
        if (index > scenario[page].Length)
        {
            isEnd = true;
            if (Time.time - currentTime > speed)
            {
                waitSimbolenabled = !waitSimbolenabled;
                waitSimbol.gameObject.SetActive(waitSimbolenabled);
                currentTime = Time.time;
            }
            return;
        }
        message.text = scenario[page].Substring(0, index);
        if (Time.time - currentTime > messageSpeed) { 
            index++;
            currentTime = Time.time;
        }
    }

    /**
     * ページ内の文字表示をリセットして最初から表示を始める.
     */
    void reset()
    {
        index = 0;
        waitSimbolenabled = false;
        isEnd = false;
    }

    /**
     * ページ内の全文字を表示させる.
     */
    void end()
    {        
        index = scenario[page].Length;
        message.text = scenario[page].Substring(0, index);
        isEnd = true;
    }

    /**
     * _filePath 読み込むシナリオファイルパス
     * @return 事前処理・ページ分割したシナリオ
     * 
     * 指定されたシナリオファイルを読み込み, 構文に従って, 事前処理・ページ分割して返す.
     */
    public string[] getScenariosFromFile(string _filePath)
    {
        FileInfo fi = new FileInfo(Application.dataPath + "/" + _filePath);
        string[] returnSt = {""};

        try
        {
            using (StreamReader sr = new StreamReader(fi.OpenRead(), Encoding.UTF8))
            {
                returnSt = sr.ReadToEnd().Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
            }
        }
        catch (Exception e)
        {
            print(e.Message);
            returnSt[0] = "READ ERROR: " + _filePath;
        }
        return returnSt;
    }
}