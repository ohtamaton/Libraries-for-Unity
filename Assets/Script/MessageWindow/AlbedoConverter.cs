using UnityEngine;
using System.Collections;

public class AlbedoConverter : MonoBehaviour
{

    [SerializeField]
    private string before_hiragana = "あいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもやゆよらりるれろわをんがぎぐげござじずぜぞだぢづでどばびぶべぼぱぴぷぺぽ";
    [SerializeField]
    private string before_katakana = "アイウエオカキクケコサシスセソタチツテトナニヌネノハヒフヘホマミムメモヤユヨラリルレロワヲンガギグゲゴザジズゼゾダヂヅデドバビブベボパピプペポ";
    [SerializeField]
    private string before_komoji = "ぁぃぅぇぉゃゅょっ";
    [SerializeField]
    private string after = "ワミフネトアチルテヨラキヌヘホサヒユセソハシスメオマリクケロヤイツレコタヲモナニウエノカムンダジヅデゾバギブゲボガビグベゴザヂズゼドプペパポピ";
    [SerializeField]
    private string after_komoji = "ァィゥェォャュョッ";
    [SerializeField]
    private bool enabled = false;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            enabled = !enabled;
        }
    }

    public string convert(string message)
    {
        char[] tmp = message.ToCharArray();
        for (int i = 0; i < message.Length; i++)
        {
            int index = before_hiragana.IndexOf(message[i]);
            if (index < 0)
            {
                index = before_katakana.IndexOf(message[i]);
            }
            if (index >= 0)
            {
                tmp[i] = after[index];
            }
            else
            {
                index = before_komoji.IndexOf(message[i]);
                if (index >= 0)
                {
                    tmp[i] = after_komoji[index];
                }

            }
        }

        message = new string(tmp);
        return message;
    }
}
