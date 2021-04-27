using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour {
    public Button button;
    public Text text;

    private void Start() {

        // bugly初始化工作都在BuglyInit工作

        text.text = SystemInfo.deviceModel;
        button.onClick.AddListener(OnClicked);
    }

    private void OnClicked() {
        Debug.LogError("OnClicked");
    }
}
