/**
 * GyroCameraController.cs
 * 
 * MainCameraにAttachするScript.
 * Gyroセンサに合わせてカメラの向きをリアルタイムに変更する.
 *
 * @author ys.ohta
 * @version 1.0
 * @date 2016/08/08
 */
using UnityEngine;
using System.Collections;

/**
 * GyroCameraController
 */
public class GyroCameraController : MonoBehaviour
{
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

    //ゲーム開始時のジャイロ値
    private Quaternion start_gyro;
    //リアルタイムのジャイロ値
    private Quaternion gyro;

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
     * カメラの向きの初期化を行う.
     * </summery>
     * @param
     * @return
    **/
    void Start()
    {
        //Gyroセンサの初期設定およびカメラの向きの初期化を行う.
        start_gyro = Input.gyro.attitude;
        Input.gyro.enabled = true;
        if (Input.gyro.enabled)
        {
            this.transform.localRotation = Quaternion.Euler(0, -start_gyro.y, 0);
        }
    }

    /**
     * <summary>
     * ジャイロ値にあわせたカメラの角度の更新を行う.
     * </summary>
     * @param
     * @return
     **/
    void Update() 
    {
        Input.gyro.enabled = true;
        if (Input.gyro.enabled)
        {
            //Gyroセンサの状態に合わせてカメラの向きを更新する.
            gyro = Input.gyro.attitude;
            gyro = Quaternion.Euler(90, 0, 0) * (new Quaternion(-gyro.x, -gyro.y, gyro.z, gyro.w));
            this.transform.localRotation = gyro;
        }
    }
}
