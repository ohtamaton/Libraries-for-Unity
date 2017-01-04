/**
 * MouseManager.cs
 * 
 * マウスからの入力を処理する.
 *
 * @author ys.ohta
 * @version 1.0
 * @date 2016/08/08
 */
using UnityEngine;
using System.Collections;

/**
 * MouseManager
 */
public class MouseManager : MonoBehaviour {
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

    //スライドの開始時のカーソル位置
	private Vector2 slideStartPosition;

    //スライド前のカーソル位置
	private Vector2 prevPosition;

    //スライド距離
	private Vector2 delta = Vector2.zero;

    //スライド中かどうかの判定フラグ
	private bool moved = false;

//===========================================================
// 関数宣言
//===========================================================

    //---------------------------------------------------
    // public
    //---------------------------------------------------

    /**
     * <summary>
     * マウスクリックされたかどうかを返す.
     * </summary>
     * @param
     * @return クリックされたかどうか
     **/
    public bool Clicked()
    {
        if (!moved && Input.GetButtonUp("Fire1"))
            return true;
        else
            return false;
    }

    /**
     * <summary>
     * マウススライド時のカーソルの移動量を返す.
     * </summary>
     * @param
     * @return delta スライドされた距離
     **/
    public Vector2 GetDeltaPosition()
    {
        return delta;
    }

    /**
     * <summary>
     * マウススライド中かどうかを返す.
     * </summary>
     * @param
     * @return moved マウススライド中かどうか
     **/
    public bool Moved()
    {
        return moved;
    }

    /**
     * <summary>
     * マウスの位置を返す.
     * </summary>
     * @param
     * @return Input.mousePosition マウスの位置
     **/
    public Vector2 GetCursorPosition()
    {
        return Input.mousePosition;
    }

    //---------------------------------------------------
    // private
    //---------------------------------------------------

    //None.

    //---------------------------------------------------
    // other
    //---------------------------------------------------

    /**
     * <summery>
     * マウススライドの判定を行う.
     * </summery>
     * @param
     * @return
     */
    void Update()
	{
        //スライド開始地点.
        if (Input.GetButtonDown("Fire1"))
			slideStartPosition = GetCursorPosition();
		
		//画面の１割以上移動させたらスライド開始と判断する.
		if (Input.GetButton("Fire1")) {
			if (Vector2.Distance(slideStartPosition,GetCursorPosition()) >= (Screen.width * 0.1f))
				moved = true;
		}
		
		//スライド操作が終了したか.
		if (!Input.GetButtonUp("Fire1") && !Input.GetButton("Fire1"))
			moved = false; // スライドは終わった.
		
		//移動量を求める.
		if (moved)
			delta = GetCursorPosition() - prevPosition;
		else
			delta = Vector2.zero;
		
		//カーソル位置を更新.
		prevPosition = GetCursorPosition();
	}
}
