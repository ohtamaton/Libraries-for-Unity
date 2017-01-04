/**
 * WebCamScript.cs
 * 
 * WebCameraの映像を写したいオブジェクトににAttachするScript.
 *
 * @author ys.ohta
 * @version 1.0
 * @date 2016/08/08
 */
using UnityEngine;
using System.Collections;

/**
 * WebCamScript
 */
public class WebCamScript : MonoBehaviour {
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
    
    //WebCamTextureの横幅
    [SerializeField] private int Width = 1280;

    //WebCamTextureの縦幅
    [SerializeField] private int Height = 720;

    //WebCameraのFPS値
    [SerializeField] private int FPS = 30;

//===========================================================
// 関数宣言
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
     * <summary>
     * WebCameraがあるデバイスの場合はその映像を投影するWebCamTextureを生成
     * </summary>
     * @param
     * @return
     **/
    void Start()
    {
        Vector3 euler = transform.localRotation.eulerAngles;

        //Androidの縦向きだと-90
        transform.localRotation = Quaternion.Euler(euler.x, euler.y, euler.z);

        WebCamDevice[] devices = WebCamTexture.devices;

        //WebCameraが1個以上見つかった場合
        if (devices.Length > 0)
        {
            //WebCamTextureを生成
            WebCamTexture webcamTexture = new WebCamTexture(Width, Height, FPS);
            
            //RendererのtextureにWebCamTextureを貼り付ける.
            GetComponent<Renderer>().material.mainTexture = webcamTexture;

            //WebCamTextureのカメラ表示開始
            webcamTexture.Play();
        }
        else
        {
            Debug.Log("Web Camera is NOT found.");
            return;
        }
    }
}
