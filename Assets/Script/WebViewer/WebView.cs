using UnityEngine;

public class WebView : MonoBehaviour
{

    string url = "https://google.co.jp/";

    void Start()
    {
#if !UNITY_EDITOR
        WebViewObject webViewObject = this.transform.gameObject.AddComponent<WebViewObject>();
        webViewObject.Init();
        webViewObject.LoadURL(url);
        webViewObject.SetVisibility(true);
        webViewObject.SetMargins(0,0,0,200);
#endif
    }
}