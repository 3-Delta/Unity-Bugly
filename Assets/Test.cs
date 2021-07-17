using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UI;

public class Test : MonoBehaviour {
    public Button button;
    public Button btnCrash;
    public Text text;

    private void Start() {

        // bugly初始化工作都在BuglyInit工作

        text.text = SystemInfo.deviceModel;
        button.onClick.AddListener(OnClicked);
        btnCrash.onClick.AddListener(OnBtnCrashClicked);
    }

    private void OnClicked() {
        Debug.LogError("OnClicked");
    }
    private void OnBtnCrashClicked() {
        Debug.LogError("OnBtnCrashClicked");
        Utils.ForceCrash(ForcedCrashCategory.AccessViolation);
    }
}
