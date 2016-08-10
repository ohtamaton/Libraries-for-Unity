
/**
 * MouseCameraController.cs
 * 
 * MainCameraにAttachするScript.
 * マウスのドラッグでカメラの角度を変更する.
 *
 * @author ys.ohta
 * @version 1.0
 * @date 2016/08/08
 */
using UnityEngine;
using System.Collections;

/**
 * MouseCameraController
 * 
 */
public class MouseCameraController : MonoBehaviour {

    //画面の横幅分カーソルを移動させたときの回転幅
    [SerializeField]
    private float rotAngle = 180.0f;
    //画面の縦幅分カーソルを移動させたときの最大幅
    [SerializeField]
    private float verAngleRange = 60.0f;

    //画面の縦幅分カーソルを移動させたときの回転幅
    [SerializeField]
    private float horizontalAngle = 250.0f;
    [SerializeField]
    private float verticalAngle = 10.0f;

    [SerializeField]
    private GameObject inputManager;
    private InputManager inputMgr;

    void Start()
	{
        inputMgr = inputManager.GetComponent<InputManager>();
	}

	// Update is called once per frame    
	void LateUpdate () {
		// ドラッグ入力でカメラのアングルを更新する.
		if (inputMgr.Moved()) {
			float anglePerPixel = rotAngle / (float)Screen.width;
			Vector2 delta = inputMgr.GetDeltaPosition();
			horizontalAngle += delta.x * anglePerPixel;
			horizontalAngle = Mathf.Repeat(horizontalAngle,360.0f);
			verticalAngle -= delta.y * anglePerPixel;
			verticalAngle = Mathf.Clamp(verticalAngle,-verAngleRange,verAngleRange);
            transform.rotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0);
        }		
	}
}
