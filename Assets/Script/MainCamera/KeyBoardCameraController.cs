/**
 * KeyBoardCameraController.cs
 * 
 * MainCameraにアタッチするScript.
 * キーボードの入力にしたがってカメラの位置を変更する.
 * 
 * @author ys.ohta
 * @version 1.0
 * @date 2016/08/XX
 */

using UnityEngine;
using System.Collections;

/**
 * KeyBoardCameraController
 * 
 */
public class KeyBoardCameraController : MonoBehaviour {

    [SerializeField]
    private float MoveSpeed = 5.0f;
    private bool isBack = false;
    private bool isForward = false;

    void Update () {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            transform.position -= transform.forward;
        }

        if (isForward)
        {
            transform.position += transform.forward;
        }

        if (isBack)
        {
            transform.position -= transform.forward;
        }
    }

    public void setForward(bool b)
    {
        isForward = b;
    }

    public void setBack(bool b)
    {
        isBack = b;
    }
}
