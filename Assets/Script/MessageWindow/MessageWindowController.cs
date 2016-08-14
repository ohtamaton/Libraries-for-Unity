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
    //Script Parser
    [SerializeField]
    MessageScriptParser parser;
    //Scenario Script File Path
    public string filepath = "Resources/Scenario.txt";
    //メッセージウィンドウの表示・非表示
    private bool enables = false;
    //メッセージウィンドウオブジェクト
    public Image messageWindow;

    //現在のページのメッセージウィンドウに表示するテキストシナリオ
    public string scenario;
    //シナリオのページ数
    private int page = 0;
    //ページ内の表示されている文字数
    private int index = 0;
    //ページ内のすべての文字が表示されたかどうか
    private bool isEnd = false;
    //現在の時刻
    private float currentTime;
    //表示中のメッセージ
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
        parser.messageWindow = this;

        //TODO 実際は下記の処理はStartScenarioのみで行うべき.
        parser.openScript(filepath);
        StartCoroutine(parser.Parse());
    }

    void StartScenario(string _filepath)
    {
        filepath = _filepath;
        parser.openScript(filepath);
        StartCoroutine(parser.Parse());
    }

    //TODO name process
    public void SetSpeaker(string s)
    {
        speaker.text = s;
    }

    public void SetWindowEnable(bool b)
    {
        enables = b;
    }

    public void SetWaitSimbolEnable(bool b)
    {
        waitSimbolenabled = b;
    }

    // Update is called once per frame
    void Update()
    {
        switch (parser.state)
        {
            case MessageScriptParser.State.ST_MESSAGE:

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
                            parser.state = MessageScriptParser.State.ST_OTHERS;
                            //reset();
                        }
                    }
                    else
                    {
                        end();
                    }
                }
                
                process();
                break;
            case MessageScriptParser.State.ST_END:
                break;

            default:
                break;
        }
        waitSimbol.gameObject.SetActive(waitSimbolenabled);
        messageWindow.gameObject.SetActive(enables);
    }

    /**
     * 文字を1文字ずつ表示する処理および, ページ移動待ちシンボルの点滅処理を行う.
     */
    void process()
    {
        if (index > scenario.Length)
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
        message.text = scenario.Substring(0, index);
        if (Time.time - currentTime > messageSpeed) { 
            index++;
            currentTime = Time.time;
        }
    }

    /**
     * ページ内の文字表示をリセットして最初から表示を始める.
     */
    public void reset()
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
        index = scenario.Length;
        message.text = scenario.Substring(0, index);
        isEnd = true;
    }
}