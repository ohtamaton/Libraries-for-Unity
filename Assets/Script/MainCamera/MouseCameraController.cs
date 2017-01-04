
/**
 * MouseCameraController.cs
 * 
 * MainCameraにAttachするScript.
 * マウスのスライドでカメラの角度を変更する.
 *
 * @author ys.ohta
 * @version 1.0
 * @date 2016/08/08
 */
using UnityEngine;
using System.Collections;

/**
 * MouseCameraController
 */
public class MouseCameraController : MonoBehaviour {
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

    //画面の横幅分スライドさせたときの回転幅
    [SerializeField] private float rotAngle = 180.0f;
    
    //垂直方向にスライドさせたときの最大回転幅
    [SerializeField] private float verAngleRange = 60.0f;

    //回転後の垂直方向の角度
    [SerializeField] private float horizontalAngle = 250.0f;
    
    //回転後の水平方向の角度
    [SerializeField] private float verticalAngle = 10.0f;

    //マウス処理用のゲームオブジェクト
    [SerializeField] private MouseManager mouseMgr;

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
     * <summary>
     * マウスのスライドでカメラの角度を変更する.
     * </summary>
     * @param
     * @return 
     **/
    void LateUpdate () {
		//マウススライドでカメラのアングルを更新する.
		if (mouseMgr.Moved()) {
			float anglePerPixel = rotAngle / (float)Screen.width;
			Vector2 delta = mouseMgr.GetDeltaPosition();

            //水平角度計算
            horizontalAngle += delta.x * anglePerPixel;
			horizontalAngle = Mathf.Repeat(horizontalAngle,360.0f);

            //垂直角度計算
            verticalAngle -= delta.y * anglePerPixel;
			verticalAngle = Mathf.Clamp(verticalAngle,-verAngleRange,verAngleRange);

            //スライド後の角度を設定
            transform.rotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0);
        }		
	}
}
