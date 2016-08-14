using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StringTest : MonoBehaviour {

	// Use this for initialization
	void Start () {

        string spaceStr = "             ";
        Debug.LogFormat("spaceStr.trim before:{0};", spaceStr);

        //先頭および末尾に空白文字があれば削除
        spaceStr = spaceStr.Trim();
        Debug.LogFormat("spaceStr.trim after:{0};", spaceStr);

        /**
         * 関数処理の実験
         */
        string func = "  @f_fu_nc(\"aaa\", \" aaa a\", \"aa, bb\", 0.01, \"()\", 'a'  )     ";
        func = func.Trim();
        Debug.LogFormat("func after trim:{0};", func);

        if(!func.StartsWith("@"))
        {
            throw new System.Exception("Validation Error.");
        }

        if(func.StartsWith("@f_")) //関数処理
        {
            //関数名の取得
            string funcName = getFunctionName(func);            
            string paramPart = func.Substring(func.IndexOf("(")+1).Trim();
            string[] parameters = getParameters(paramPart);

            Debug.LogFormat("parameters length: {0}",parameters.Length);

            //関数ごとに処理分岐. 関数を追加する場合はここに追加.
            switch(funcName)
            {
                case "message":
                    break;
                case "message_start":
                    break;
                case "name":
                    break;
                case "showWindow":
                    break;
                case "closeWindow":
                    break;
                case "wait":
                default:
                    break;
            }
        }
    }

    private string getFunctionName(string s)
    {
        //関数名の取得
        if (s.Equals(""))
        {
            return null;
        }

        if (s.IndexOf("(") < 0)
        {
            throw new System.Exception("Validation Error.");
        }

        if (!s.EndsWith(")"))
        {
            throw new System.Exception("Validation Error.");
        }

        string funcName = s.Substring(0, s.IndexOf("(")).Substring("@f_".Length);
        Debug.LogFormat("funcName:{0};", funcName);

        if (!isFunctionName(funcName))
        {
            throw new System.Exception("Validation Error. func name is invalid.");
        }

        return funcName;
    }

    private string[] getParameters(string paramPart)
    {
        //object[] parameters = new object[10];
        List<string> parameters = new List<string>();
        Debug.LogFormat("paramPart:{0};", paramPart);

        while (!paramPart[0].Equals(")"))
        {
            if (paramPart.StartsWith("\""))
            {
                parameters.Add(paramPart.Substring(1, paramPart.IndexOf("\"", 1) - 1));
                paramPart = paramPart.Substring(paramPart.IndexOf("\"", 1) + 1).TrimStart().TrimStart(',').TrimStart();
                Debug.LogFormat("parameter:{0};", parameters[parameters.Count-1]);
                Debug.LogFormat("paramPart:{0};", paramPart);

            }
            else if (paramPart.StartsWith("'"))
            {
                parameters.Add(paramPart.Substring(1, paramPart.IndexOf("'", 1) - 1));
                paramPart = paramPart.Substring(paramPart.IndexOf("'", 1) + 1).TrimStart().TrimStart(',').TrimStart();
                Debug.LogFormat("parameter:{0};", parameters[parameters.Count - 1]);
                Debug.LogFormat("paramPart:{0};", paramPart);
            }
            else if (char.IsDigit(paramPart, 0))
            {
                string parameter = paramPart.Substring(0, paramPart.IndexOf(",")).Trim();
                int intResult;
                float floatResult;
                if (!int.TryParse(parameter, out intResult) && !float.TryParse(parameter, out floatResult))
                {
                    throw new System.Exception("Validation Error.");
                }
                parameters.Add(parameter);
                paramPart = paramPart.Substring(paramPart.IndexOf(",") + 1).TrimStart();
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

        for (int i=1; i < s.Length-1; i++)
        {
            if (!(char.IsLetterOrDigit(s, i) || s[i].Equals('_')))
            {                
                return false;
            }
        }
        if (!char.IsLetterOrDigit(s, s.Length-1)) {
            return false;
        }
        return true;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
