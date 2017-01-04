/**
 * KeyBoardCameraController.cs
 * 
 * MainCameraにアタッチするScript.
 * キーボードの入力にしたがってカメラの位置を変更する.
 * 
 * @author ys.ohta
 * @version 1.0
 * @date 2016/08/08
 */
using UnityEngine;
using System.Collections;

/**
 * KeyBoardCameraController
 */
public class KeyBoardCameraController : MonoBehaviour {
//===========================================================
// 変数宣言
//===========================================================

    //---------------------------------------------------
    // public
    //---------------------------------------------------

    //None.

    //---------------------------------------------------
    // private
    //---------------------------------------------------

    //カメラの移動速度
    [SerializeField] private float MoveSpeed = 5.0f;

    //バックボタンが押されているかどうか
    private bool isBack = false;

    //フォワードボタンが押されているかどうか
    private bool isForward = false;

//===========================================================
// 関数宣言
//===========================================================

    //---------------------------------------------------
    // public
    //---------------------------------------------------

    //None. 

    //---------------------------------------------------
    // public
    //---------------------------------------------------

    //None. 

    //---------------------------------------------------
    // other
    //---------------------------------------------------

    /**
     * <summery>
     * キーボード/ボタンの入力にしたがってカメラの位置を変更する.
     * </summery>
     * @param
     * @return
     */
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

    /**
     * <summery>
     * フォワードボタンが押されているかどうかを返す
     * </summery>
     * @param
     * @return isForward フォワードボタンが押されているかどうか
     */
    public void setForward(bool b)
    {
        isForward = b;
    }

    /**
     * <summery>
     * バックボタンが押されているかどうかを返す
     * </summery>
     * @param
     * @return isBack バックボタンが押されているかどうか
     */
    public void setBack(bool b)
    {
        isBack = b;
    }
}
