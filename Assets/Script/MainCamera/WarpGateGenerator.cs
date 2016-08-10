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
 *
 */
public class WarpGateGenerator : MonoBehaviour {

    //作成するワープドアのプレハブ]
    [SerializeField]
    private GameObject prefab;

    //生成したワープドアを保持する
    private GameObject door;

    //ワープドアを生成する場所のカメラからの距離
    public float distance = 10.0f;

    //@TODO
    [SerializeField]
    private GameObject rule;
    private DestCameraController destCamController;
    [SerializeField]
    private Camera destCamera;

    [SerializeField]
    private Camera webCamera;
    [SerializeField]
    private GameObject webView;
    private GyroCameraController gyroController;
    
    // Use this for initialization
    void Start () {
        destCamController = rule.GetComponent<DestCameraController>();
        gyroController = gameObject.GetComponent<GyroCameraController>();
	}
	
	// Update is called once per frame
	void Update () {

	}

    //ワープドアを生成し, その位置をPortalCameraControllerに設定する.
    public void generate()
    {
        //既に生成済みのワープドアがあれば削除
        if(door != null)
        {
            Destroy(door);
        }
        //カメラから正面のdistance距離の値を取得
        Vector3 pos = new Vector3(
            transform.position.x + transform.forward.x * distance, 
            0, 
            transform.position.z + transform.forward.z*distance
            );
        door = Instantiate(prefab, pos, transform.rotation) as GameObject;
        destCamController.Source = door.transform;
    }

    //生成したワープドアにMainCameraがぶつかった場合にワープ.
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("warpGate"))
        {
            Destroy(webView);
            webCamera.enabled = false;
            transform.position = destCamera.transform.position;
            transform.rotation = destCamera.transform.rotation;
            gyroController.enabled = false;
        }
    }
}
