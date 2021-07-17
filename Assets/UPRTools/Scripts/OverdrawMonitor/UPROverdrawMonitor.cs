#if UNITY_2018_2_OR_NEWER
using System.Collections.Generic;
using System;
using UnityEngine;
using UPRProfiler.Common;

namespace UPRProfiler.OverdrawMonitor
{
    public class UPROverdrawMonitor : MonoBehaviour
    {
        private static UPROverdrawMonitor instance;

        public static bool Enabled = false;
        public static bool Cleaned = true;

        public static bool NotSupportedPlatform = false;

        public static bool NotSupportedFlagSent = false;

        private static string notSupportedGroupName = "overdrawNotSupported";
        
        public static UPROverdrawMonitor Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<UPROverdrawMonitor>();
                    if (instance == null)
                    {
                        if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBFloat) 
                            || !SystemInfo.SupportsTextureFormat(TextureFormat.RGBAFloat)
                            || !SystemInfo.supportsComputeShaders)
                        {
                            Debug.Log("Current platform dose not support upr overdraw monitor");
                            NotSupportedPlatform = true;
                        }

                        var go = new GameObject("UPROverdrawMonitor");
                        DontDestroyOnLoad(go);
                        go.hideFlags = HideFlags.HideAndDontSave;
                        instance = go.AddComponent<UPROverdrawMonitor>();
                        instance._monitorsGo = go;
                    }
                }

                return instance;
            }
        }

        private GameObject _monitorsGo;

        void Update()
        {
            if (!Enabled)
            {
                if (!Cleaned)
                {
                    Clean();
                    Cleaned = true;
                }
                return;
            }
            
            if (NotSupportedFlagSent)
            {
                return;
            }

            if (NotSupportedPlatform)
            {
                var toSent = new Dictionary<string, string>();
                toSent["supported"] = "-1";
                if(UPROpen.SendCustomizedData(UPRCameraOverdrawMonitor.customDataSubjectName, notSupportedGroupName,
                    "line", toSent))
                {
                    NotSupportedFlagSent = true;
                }
                
                Clean();
                return;
            }

            Camera[] activeCameras = Camera.allCameras;

            var monitors = GetAllMonitors();
            foreach (var monitor in monitors)
            {
                if (!Array.Exists(activeCameras, c => monitor.targetCamera == c))
                {
                    DestroyImmediate(monitor);
                }
                    
            }
                

            monitors = GetAllMonitors();
            foreach (Camera activeCamera in activeCameras)
            {
                if (!Array.Exists(monitors, m => m.targetCamera == activeCamera))
                {
                    var monitor = _monitorsGo.AddComponent<UPRCameraOverdrawMonitor>();
                    monitor.SetTargetCamera(activeCamera);
                }
            }
        }

        UPRCameraOverdrawMonitor[] GetAllMonitors()
        {
            return _monitorsGo.GetComponentsInChildren<UPRCameraOverdrawMonitor>(true);
        }

        void Clean()
        {
            var monitors = GetAllMonitors();
            foreach (var monitor in monitors)
                DestroyImmediate(monitor);
        }
    }
}
#endif