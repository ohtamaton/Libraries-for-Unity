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
 * WebCamScript.cs
 */
public class WebCamScript : MonoBehaviour {

    //WebCamTextureの横幅
    [SerializeField]
    private int Width = 1280;
    [SerializeField]
    //WebCamTextureの縦幅
    private int Height = 720;
    //WebCameraのFPS値
    [SerializeField]
    private int FPS = 30;

    void Start()
    {
        Vector3 euler = transform.localRotation.eulerAngles;

        //Androidの縦向きだと-90
        transform.localRotation = Quaternion.Euler(euler.x, euler.y, euler.z);// - 90);

        WebCamDevice[] devices = WebCamTexture.devices;

        //WebCameraが見つかった場合
        if (devices.Length > 0)
        {
            WebCamTexture webcamTexture = new WebCamTexture(Width, Height, FPS);
            //RendererのtextureにWebCamTextureを貼り付ける.
            GetComponent<Renderer>().material.mainTexture = webcamTexture;
            webcamTexture.Play();
        }
        else
        {
            Debug.Log("Web Camera is NOT found.");
            return;
        }
    }
}
