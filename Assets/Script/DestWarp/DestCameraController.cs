/**
 * DestCameraController.cs
 * 
 * DestCameraを制御するためのクラス
 *
 * @author ys.ohta
 * @version 1.0
 * @date 2016/08/08
 */
using UnityEngine;
using System.Collections;

/**
 * DestCameraController
 */
public class DestCameraController : MonoBehaviour {
//===========================================================
// 変数宣言
//===========================================================

    //---------------------------------------------------
    // public
    //---------------------------------------------------

    //移動前のワープゲートのtransform
    public Transform Source = null;
    //移動先のワープゲートのtransform
    public Transform Destination = null;

    //---------------------------------------------------
    // private
    //---------------------------------------------------

    //MainCamera
    [SerializeField] private Camera MainCamera = null;
    //移動先のCamera
    [SerializeField] private Camera destCamera = null;

    //===========================================================
    // 関数定義
    //===========================================================

    //---------------------------------------------------
    // public
    //---------------------------------------------------

    //None.

    //---------------------------------------------------
    // private
    //---------------------------------------------------

    //None.

    //---------------------------------------------------
    // other
    //---------------------------------------------------

    /**
     * <summery>
     * DestCameraをMainCameraの動きに合わせて調整する.
     * 毎フレーム実行される.
     * </summery>
     * @param
     * @return
     */
    void Update () {

        //ワールド->ローカルの変換後取得(Position, Rotation)
        Vector3 cameraInSourceSpace = Source.InverseTransformPoint(MainCamera.transform.position);
        Quaternion cameraInSourceSpaceRot = Quaternion.Inverse(Source.rotation) * MainCamera.transform.rotation;

        //ローカル->ワールドの変換(Position, Rotation)
        destCamera.transform.position = Destination.TransformPoint(cameraInSourceSpace);
        destCamera.transform.rotation = Destination.rotation * cameraInSourceSpaceRot;

        //視野角を合わせる
        destCamera.fieldOfView = MainCamera.fieldOfView;

        //アスペクト比を合わせる
        destCamera.aspect = MainCamera.aspect;

    }
}
