using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace HRM
{
    public class HRMController : MonoBehaviour
    {
        List<string> _updateList;
        int _sucNum;

        private void Awake()
        {
            //Addressables.LoadSceneAsync
            //Addressables.UnloadSceneAsync
            //Addressables.GetDownloadSizeAsync()
            //Addressables.DownloadDependenciesAsync
        }

        private void Start()
        {
            //var handler = Addressables.CheckForCatalogUpdates();
        }

        public void OnClickUpdate()
        {
            _sucNum = 0;
            StartCoroutine(CheckUpdateList());
            //AsyncOperationHandle handler = Addressables.LoadAssetAsync<AudioClip>("Assets/Audio/fail.mp3");
            //handler.Completed += LoadAssetComplete;

            //var handler = Addressables.InitializeAsync();
        }

        void InitializeAsyncComplete(AsyncOperationHandle obj)
        {

        }

        public void OnClickClearCache()
        {
            Addressables.ClearDependencyCacheAsync("MainScene");
        }

        IEnumerator CheckUpdateList()
        {
            AsyncOperationHandle handler = Addressables.CheckForCatalogUpdates();
            handler.Completed += CheckUpdateListComplete;

            while (!handler.IsDone)
            {
                var status = handler.GetDownloadStatus();
                Debug.Log("[*] loadPrecent:" + status.Percent);
                yield return null;
            }
        }

        void CheckUpdateListComplete(AsyncOperationHandle obj)
        {
            _updateList = obj.Result as List<string>;
            if (_updateList.Count > 0)
            {
                foreach (var key in _updateList)
                {
                    var handler = Addressables.DownloadDependenciesAsync(key, true);
                    handler.Completed += OnUpdateOnceComplete;
                }
            }
            else
            {
                OnTotalUpdateComplete();
            }
        }

        void OnUpdateOnceComplete(AsyncOperationHandle obj)
        {
            _sucNum++;
            if (_sucNum == _updateList.Count)
            {
                OnTotalUpdateComplete();
            }
        }

        void OnTotalUpdateComplete()
        {
            Debug.Log("[*] OnTotalUpdateComplete");
            Addressables.LoadSceneAsync("MainScene");
        }

        void LoadAssetComplete(AsyncOperationHandle operationHandle)
        {
            //var audio = operationHandle.Result as AudioClip;
            var gameObject = new GameObject("ScriptText");
            gameObject.AddComponent<LoadScripts>();
        }
    }
}