/**
 * MessageScriptParser.cs
 *
 * @author ys.ohta
 * @version 1.0
 * @date 2016/08/15
 */

using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Text;
using System.Collections.Generic;

/**
 * MessageScriptParser
 */
public class MessageScriptParser : MonoBehaviour {
//===========================================================
// 変数宣言
//===========================================================
    //---------------------------------------------------
    // public
    //---------------------------------------------------
    
    public State state = State.ST_OTHERS;

    public enum State
    {
        ST_MESSAGE, ST_OTHERS, ST_END
    }

    //メッセージウィンドウ処理用のController
    public MessageWindowController messageWindow;

    //---------------------------------------------------
    // private
    //---------------------------------------------------

    //Script
    private string script;

    //選択ウィンドウ処理用のController
    //For the future upgrade
    //TODO SelectWindowController selectWindow;

//===========================================================
// 関数宣言
//===========================================================
    //---------------------------------------------------
    // public
    //---------------------------------------------------

    /**
     * ScriptPserser
     * @v_var = 1なども処理できるように
     * if (条件式) else if elseなども処理できるように
     * while(条件式) do doneなども処理できるように
     * for()
     * 条件式 ==, !=, <, >, >=, <=, &&, 
     */
    public IEnumerator Parse()
    {

#if UNITY_EDITOR
        string[] delimiter = { ("\r\n") };
        string newLine = "\r\n";
#elif UNITY_STANDALONE_WIN
        string[] delimiter = { ("\r\n") };
        string newLine = "\r\n";
#elif UNITY_STANDALONE_LINUX
        string[] delimiter = { ("\n") };
        string newLine = "\n";
#endif

        //Scriptを改行区切りで分ける.
        string[] lines = script.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
        int i = 0;

        while ((i < lines.Length) || (state == State.ST_MESSAGE)) //Scriptの最終行まで処理する.
        {


            //@を期待するモードかどうかで場合分け
            if (state == State.ST_MESSAGE)
            {
                yield return null;
                continue;
            }

            //先頭および末尾に空白文字があれば削除
            lines[i] = lines[i].Trim();

            //空白文字をなくしたあと何も残らなければ
            if (lines[i].Equals(""))
            {
                i++;
                yield return null;
            }

            if (!lines[i].StartsWith("@"))
            {
                Debug.LogFormat("lines[{0}]:{1}", i, lines[i]);
                throw new System.Exception("Validation Error.");
            }

            if (lines[i].StartsWith("@f_")) //関数処理
            {
                //関数名の取得
                string funcName = getFunctionName(lines[i]);
                string paramPart = lines[i].Substring(lines[i].IndexOf("(") + 1).Trim();
                string[] parameters = getParameters(paramPart);

                Debug.LogFormat("paramPart: {0}", paramPart);

                //関数ごとに処理分岐. 関数を追加する場合はここに追加.
                switch (funcName)
                {
                    case "message":
                        Message(parameters);
                        state = State.ST_MESSAGE;
                        yield return null;
                        break;
                    case "message_start":
                        string message = "";
                        i++;
                        while (!lines[i].StartsWith("@f_message_end"))
                        {
                            message += lines[i] + newLine;
                            i++;
                            if (i >= lines.Length)
                            {
                                throw new Exception("script validation error. cannot find @message_end");
                            }
                        }
                        Debug.LogFormat("1:{0}", parameters.Length);
                        string[] tmpParams = { parameters[0], message, parameters[1] };
                        Message(tmpParams);
                        state = State.ST_MESSAGE;
                        yield return null;
                        break;
                    case "name":
                        Name(parameters);
                        state = State.ST_OTHERS;
                        yield return null;
                        break;
                    case "showWindow":
                        messageWindow.SetWindowEnable(true);
                        state = State.ST_OTHERS;
                        yield return null;
                        break;
                    case "closeWindow":
                        messageWindow.SetWindowEnable(false);
                        state = State.ST_OTHERS;
                        yield return null;
                        break;
                    case "wait":
                        messageWindow.SetWaitSimbolEnable(false);
                        if (parameters.Length != 1)
                        {
                            throw new Exception("script validation error. please check your script at @wait.");
                        }
                        int res;
                        if (!int.TryParse(parameters[0], out res))
                        {
                            throw new Exception("script validation error. please check your script at @wait.");
                        }
                        state = State.ST_OTHERS;
                        yield return new WaitForSeconds(res);
                        break;
                    case "select":
                        Select(parameters);
                        state = State.ST_OTHERS;
                        yield return null;
                        break;
                    case "if":
                        /**
                         * @f_if ($select == 0)
                         * @f_else if($select ==1)
                         * @f_else
                         * 
                         * 
                         * @f_if ($rand == 30)
                         * @f_else if ($rand == 60)
                         * @f_else
                         * 
                         * 
                         * @f_if ($switch[10] == true)
                         * 
                         *
                         * @f_if($switch[10] != true)
                         * 
                         */
                        //選択結果による分岐
                        //乱数による分岐
                        //フラグによる分岐(C#からフラグ情報の取得が必要? フラグ番号のon/offなど)
                        yield return null;
                        break;
                    case "jump":
                        /**
                         * @jump(10)
                         */
                        //あるスクリプトの行にジャンプする
                        yield return null;
                        break;
                    default:
                        throw new Exception("script validation error. please check your script at @wait.");
                }
            }
            else if (lines[i].StartsWith("@v_")) //変数処理
            {
                //TODO
                throw new Exception("@v_ is not supported yet.");
            }
            i++;
        }
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

    //---------------------------------------------------
    // private
    //---------------------------------------------------

    private void Message(string[] parameters)
    {
        messageWindow.reset();
        if (parameters.Length != 3)
        {
            throw new Exception("script validation error. please check your script at @message.");
        }
        messageWindow.SetSpeaker(parameters[0]);
        messageWindow.scenario = (parameters[1]);
        float res;
        if (!float.TryParse(parameters[2], out res))
        {
            throw new Exception("script validation error. please check your script.");
        }
        messageWindow.messageSpeed = res;
    }

    private void Name(string[] parameters)
    {
        if (parameters.Length != 1)
        {
            throw new Exception("script validation error. please check your script at @name.");
        }
        messageWindow.SetSpeaker(parameters[0]);
    }

    private void Select(string[] parameters)
    {
        //TODO implementation
        if (parameters.Length <= 1)
        {
            throw new Exception("script validation error. please check your script at @select.");
        }
        throw new Exception("Sorry, this function is Not implemented yet.");
    }

    private string getFunctionName(string s)
    {
        //関数名の取得
        if (s.Equals(""))
        {
            //
            return null;
        }

        string funcName = "";

        if (s.IndexOf("(") < 0)
        {
            funcName = s.Substring("@f_".Length);
            Debug.Log(s);
            return funcName;
        }

        if (!s.EndsWith(")"))
        {
            throw new System.Exception("Validation Error.");
        }

        funcName = s.Substring(0, s.IndexOf("(")).Substring("@f_".Length);
        Debug.LogFormat("funcName:{0};", funcName);

        if (!isFunctionName(funcName))
        {
            throw new System.Exception("Validation Error. func name is invalid.");
        }

        return funcName;
    }

    private string[] getParameters(string paramPart)
    {
        List<string> parameters = new List<string>();
        Debug.LogFormat("getParameters start paramPart:{0};", paramPart);

        while (paramPart.Length != 0 && !paramPart[0].Equals(")"))
        {
            if (paramPart.StartsWith("\""))
            {
                parameters.Add(paramPart.Substring(1, paramPart.IndexOf("\"", 1) - 1));
                paramPart = paramPart.Substring(paramPart.IndexOf("\"", 1) + 1).TrimStart().TrimStart(',').TrimStart();
                Debug.Log("\"");
                Debug.LogFormat("parameter:{0};", parameters[parameters.Count - 1]);
                Debug.LogFormat("paramPart:{0};", paramPart);

            }
            else if (paramPart.StartsWith("'"))
            {
                parameters.Add(paramPart.Substring(1, paramPart.IndexOf("'", 1) - 1));
                paramPart = paramPart.Substring(paramPart.IndexOf("'", 1) + 1).TrimStart().TrimStart(',').TrimStart();
                Debug.Log("'");
                Debug.LogFormat("parameter:{0};", parameters[parameters.Count - 1]);
                Debug.LogFormat("paramPart:{0};", paramPart);
            }
            else if (char.IsDigit(paramPart, 0))
            {
                int lastIndex = paramPart.IndexOf(",");
                if (lastIndex <0)
                {
                    lastIndex = paramPart.IndexOf(")");
                }
                string parameter = paramPart.Substring(0, lastIndex).Trim();
                int intResult;
                float floatResult;
                if (!int.TryParse(parameter, out intResult) && !float.TryParse(parameter, out floatResult))
                {
                    throw new System.Exception("Validation Error.");
                }
                parameters.Add(parameter);
                paramPart = paramPart.Substring(lastIndex + 1).TrimStart();
                Debug.Log("digit");
                Debug.LogFormat("parameter:{0};", parameters[parameters.Count - 1]);
                Debug.LogFormat("paramPart:{0};", paramPart);
            }
            else
            {
                //TODO 今は, 数字と文字列のみで, 変数には未対応.
                break;
            }
        }
        Debug.Log("parameter analysis end.");
        return parameters.ToArray();
    }

    //function nameのフォーマットに従っているか判定する.
    //{alphabet}{alphabet | number | "_"}*
    private bool isFunctionName(string s)
    {
        if (!char.IsLetter(s, 0))
        {
            return false;
        }

        for (int i = 1; i < s.Length - 1; i++)
        {
            if (!(char.IsLetterOrDigit(s, i) || s[i].Equals('_')))
            {
                return false;
            }
        }
        if (!char.IsLetterOrDigit(s, s.Length - 1))
        {
            return false;
        }
        return true;
    }
}
