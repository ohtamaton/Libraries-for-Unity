/**
 * MessageScriptParser.cs
 *
 * @author ys.ohta
 * @version 1.0
 * @date 2016/08/XX
 */

using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Text;

/**
 * MessageScriptParser
 * 
 */
public class MessageScriptParser : MonoBehaviour {

    private string script;

    public State state = State.ST_OTHERS;

    public enum State
    {
        ST_MESSAGE, ST_OTHERS, ST_END
    }

    //メッセージウィンドウ処理用のController
    public MessageWindowController messageWindow;

    //選択ウィンドウ処理用のController
    //SelectWindowController selectWindow;

    // Use this for initialization
    void Awake () {
	
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

    }

    void MessageStart(string[] parameters)
    {

        parameters[0] = parameters[0].Substring(parameters[0].IndexOf("\"") + 1);
        parameters[0] = parameters[0].Substring(0, parameters[0].IndexOf("\""));
        messageWindow.SetSpeaker(parameters[0]);
        //TODO setscenario
        messageWindow.scenario = parameters[1];

        parameters[2] = parameters[2].Replace(" ", string.Empty);

        float res;
        if (!float.TryParse(parameters[2], out res))
        {
            throw new Exception("script validation error. please check your script.");
        }

        messageWindow.messageSpeed = res;

    }

    void Message(string[] parameters)
    {
        
        parameters[0] = parameters[0].Substring(parameters[0].IndexOf("\"")+1);
        parameters[0] = parameters[0].Substring(0, parameters[0].IndexOf("\""));
        messageWindow.SetSpeaker(parameters[0]);
        //TODO setscenario
        parameters[1] = parameters[1].Substring(parameters[1].IndexOf("\"") + 1);
        parameters[1] = parameters[1].Substring(0, parameters[1].IndexOf("\""));
        messageWindow.scenario = parameters[1];

        parameters[2] = parameters[2].Replace(" ", string.Empty);

        float res;
        if (!float.TryParse(parameters[2], out res))
        {
            throw new Exception("script validation error. please check your script.");
        }

        messageWindow.messageSpeed = res;

    }

    void Name(string[] parameters)
    {
        parameters[0] = parameters[0].Substring(parameters[0].IndexOf("\"") + 1);
        parameters[0] = parameters[0].Substring(0, parameters[0].IndexOf("\""));
        messageWindow.SetSpeaker(parameters[0]);
        //TODO name更新処理をwindow側で
    }

    void Select(string[] parameters)
    {
        //TODO implementation
        throw new Exception("Sorry, this function is Not implemented yet.");
    }

    /**
     * scriptの内容を1行ずつparseして処理を行う.
     */
    public IEnumerator Parse()
    {
        string[] newLine = { ("\r\n") };
        string[] delimiter = { "(", ")", ","};
        string[] lines = script.Split(newLine, StringSplitOptions.RemoveEmptyEntries);
        int i = 0;

        while ((i < lines.Length)||(state == State.ST_MESSAGE))
        {
            if (state == State.ST_MESSAGE) //Parse処理しない状態の場合はそのまま抜ける.
            {
                yield return null;
                continue;
            }
            if (lines[i].StartsWith("@message_start"))
            {
                messageWindow.reset();
                //パラメータ取得                
                string[] tmpParams = lines[i].Substring("@message_start".Length).Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                
                if (tmpParams.Length != 2)
                {
                    throw new Exception("script validation error. please check your script at @message_start.");
                }
                string[] parameters = { tmpParams[0], "", tmpParams[1] };
                i++;
                while(!lines[i].StartsWith("@message_end"))
                {
                    parameters[1] += lines[i] + "\r\n";
                    i++;
                    if(i >= lines.Length)
                    {
                        throw new Exception("script validation error. cannot find @message_end");
                    }
                }
                MessageStart(parameters);
                i++;
                state = State.ST_MESSAGE;
                yield return null;
            }
            else if (lines[i].StartsWith("@message"))
            {
                messageWindow.reset();
                //パラメータ取得
                string[] parameters = lines[i].Substring("@message".Length).Split(delimiter, StringSplitOptions.RemoveEmptyEntries);           
                if (parameters.Length != 3)
                {
                    throw new Exception("script validation error. please check your script at @message.");
                }
                Message(parameters);
                i++;
                state = State.ST_MESSAGE;
                yield return null;
            }
            else if (lines[i].StartsWith("@name"))
            {
                string[] parameters = lines[i].Substring("@name".Length).Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

                if (parameters.Length != 1)
                {
                    throw new Exception("script validation error. please check your script at @name.");
                }

                Name(parameters);
                i++;
                state = State.ST_OTHERS;
                yield return null;
            }
            else if (lines[i].StartsWith("@select"))
            {
                string[] parameters = lines[i].Substring("@select".Length).Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

                if (parameters.Length <= 1)
                {
                    throw new Exception("script validation error. please check your script at @select.");
                }
                Select(parameters);
                i++;
                state = State.ST_OTHERS;
                yield return null;
            }
            else if (lines[i].StartsWith("@wait"))
            {
                string[] parameters = lines[i].Substring("@wait".Length).Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                if (parameters.Length != 1)
                {
                    throw new Exception("script validation error. please check your script at @wait.");
                }
                int res;
                if(!int.TryParse(parameters[0], out res))
                {
                    throw new Exception("script validation error. please check your script at @wait.");
                }
                i++;
                state = State.ST_OTHERS;
                yield return new WaitForSeconds(res);
            }
            else if (lines[i].Equals("@showWindow"))
            {
                messageWindow.SetWindowEnable(true);
                i++;
                state = State.ST_OTHERS;
                yield return null;
            }
            else if (lines[i].Equals("@closeWindow"))
            {
                messageWindow.SetWindowEnable(false);
                i++;
                state = State.ST_OTHERS;
                yield return null;
            }
            else
            {
                string err = "line:" + lines[i] + "script validation error. please check your script.";
                throw new Exception(err);
            }         
        }
        Debug.Log("ST_END");
        state = State.ST_END;
        yield return null;
    }

    /**
     * 処理するシナリオファイルを開き, 内容をscriptに設定.
     *
     */
    public void openScript(string _filePath)
    {

        FileInfo fi = null;
        #if UNITY_EDITOR
        fi = new FileInfo(Application.dataPath + "/" + _filePath);
        #elif UNITY_ANDROID
        fi = new FileInfo(Application.streamingAssetsPath + "/" + _filePath);        
        #endif
        
        string returnSt;

        try
        {
            using (StreamReader sr = new StreamReader(fi.OpenRead(), Encoding.UTF8))
            {
                returnSt = sr.ReadToEnd();
            }
        }
        catch (Exception e)
        {
            print(e.Message);
            returnSt = "READ ERROR: " + _filePath;
        }
        script = returnSt;
    }
}
