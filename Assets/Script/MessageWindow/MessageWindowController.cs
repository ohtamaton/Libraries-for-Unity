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

    public bool enables = true;
    public GameObject messageWindow;

    [SerializeField]
    private string[] scenario;
    private int page = 0;
    private int index = 0;
    private bool isEnd = false;

    private float currentTime;

    [SerializeField]
    private Text message;
    [SerializeField]
    private Text speaker;
    [SerializeField]
    private Text waitSimbol;
    private bool waitSimbolenabled = false;

    private string currentMessage;

    public float messageSpeed = 0.04f;
    public float speed = 0.15f;

    private string[] delimiter = { "@page\r\n" };

    private string[] newLine = { "\r\n" };

    void Start()
    {
        currentTime = Time.time;
        scenario = getScenariosFromFile("Resources/Scenario.txt");
        string[] tmp = scenario[page].Split(newLine, StringSplitOptions.RemoveEmptyEntries);
        print(tmp[0]);
        print(tmp[1]);
        speaker.text = tmp[0];
        scenario[page] = scenario[page].Substring(tmp[0].Length+2);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            enables = !enables;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
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

    void reset()
    {
        index = 0;
        waitSimbolenabled = false;
        isEnd = false;
    }

    void end()
    {        
        index = scenario[page].Length;
        message.text = scenario[page].Substring(0, index);
        isEnd = true;
    }

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