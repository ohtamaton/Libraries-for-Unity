/**
 * WarpGateGenerator.cs
 * 
 * MainCameraにAttachするScript.
 * 正面のdistanceの距離にワープ用のドアを生成する. 
 *
 * @author ys.ohta
 * @version 1.0
 * @date 2016/08/08
 */
using UnityEngine;
using System.Collections;

/**
 * WarpGateGenerator
 */
public class WarpGateGenerator : MonoBehaviour {
//===========================================================
// 変数宣言
//===========================================================

    //---------------------------------------------------
    // public
    //---------------------------------------------------

    //ワープドアを生成する場所のカメラからの距離
    public float distance = 10.0f;

    //---------------------------------------------------
    // private
    //---------------------------------------------------

    //作成するワープドアのプレハブ
    [SerializeField]
    private GameObject prefab;

    //生成したワープドア
    private GameObject door;

    //ワープ先のカメラのコントローラ
    [SerializeField] private DestCameraController destCamController;

    //ワープ先のカメラ
    [SerializeField] private Camera destCamera;

    //ウェブカメラ
    [SerializeField] private Camera webCamera;

    //ウェブカメラ投影用のオブジェクト
    [SerializeField] private GameObject webView;

    //ジャイロカメラコントローラ
    [SerializeField] private GyroCameraController gyroController;

//===========================================================
// 関数宣言
//===========================================================

    //---------------------------------------------------
    // public
    //---------------------------------------------------

    /**
     * <summary>
     * ワープドアを生成し, その位置をDestCameraControllerに設定する.
     * </summary>
     * @param
     * @return
     **/
    public void generate()
    {
        //既に生成済みのワープドアがあれば削除
        if(door != null)
        {
            Destroy(door);
        }

        //ドアの生成位置を計算
        Vector3 pos = new Vector3(
            transform.position.x + transform.forward.x * distance, 
            0, 
            transform.position.z + transform.forward.z*distance
            );

        //ドアを生成
        door = Instantiate(prefab, pos, transform.rotation) as GameObject;

        //DestCameraControllerにワープドアの位置を設定
        destCamController.Source = door.transform;
    }

    //---------------------------------------------------
    // public
    //---------------------------------------------------

    //None. 

    //---------------------------------------------------
    // other
    //---------------------------------------------------

    /**
     * <summary>
     * 生成したワープドアにMainCameraがぶつかった場合にワープ.
     * </summary>
     * @param other
     * @return
     **/
    void OnTriggerEnter(Collider other)
    {
        //MainCameraがワープゲートに衝突した場合
        if (other.gameObject.layer == LayerMask.NameToLayer("warpGate"))
        {
            //Webカメラ用のオブジェクトを削除
            Destroy(webView);
            webCamera.enabled = false;

            //MainCameraの位置をワープ先に設定
            transform.position = destCamera.transform.position;
            transform.rotation = destCamera.transform.rotation;

            //ジャイロセンサをdisable
            gyroController.enabled = false;
        }
    }
}
