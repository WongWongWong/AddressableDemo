using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using zFramework.Hotfix.Toolkit;
using static zFramework.Hotfix.Toolkit.HotfixLoader;

namespace HRM
{
    public class HRMController : MonoBehaviour
    {
        [SerializeField, Tooltip("代码执行脚本")]
        HotfixLoader hotfixLoader;

        /// <summary>
        /// 更新列表
        /// </summary>
        List<string> _updateCatalogs;

        private void Start()
        {
            InitAddressbles();
        }

        /// <summary>
        /// 初始化Addressbles
        /// </summary>
        void InitAddressbles()
        {
            AsyncOperationHandle<IResourceLocator> init = Addressables.InitializeAsync();
            init.Completed += InitAddressblesComplete;
        }

        /// <summary>
        /// 初始化addressbles完成
        /// </summary>
        /// <param name="obj"></param>
        void InitAddressblesComplete(AsyncOperationHandle<IResourceLocator> obj)
        {
            //获取更新列表
            AsyncOperationHandle<List<string>> updateList = Addressables.CheckForCatalogUpdates();
            updateList.Completed += CheckCatalogUpdatesComplete;
        }

        /// <summary>
        /// 检测更新
        /// </summary>
        /// <param name="obj"></param>
        void CheckCatalogUpdatesComplete(AsyncOperationHandle<List<string>> obj)
        {
            //执行更新
            _updateCatalogs = obj.Result;
            if (_updateCatalogs.Count > 0)
            {
                //需要更新,获取下载大小
                GetDownLoadSize();
            }
            else
            {
                //无需更新
                RunScripts();
            }
        }

        /// <summary>
        /// 获得下载大小
        /// </summary>
        /// <param name="catalogs"></param>
        void GetDownLoadSize()
        {
            var handler = Addressables.GetDownloadSizeAsync(_updateCatalogs);
            handler.Completed += GetDownLoadSizeComplete;
        }

        /// <summary>
        /// 获得下载大小完成
        /// </summary>
        /// <param name="completeHandler"></param>
        void GetDownLoadSizeComplete(AsyncOperationHandle<long> completeHandler)
        {
            //输出大小
            Debug.LogFormat("[*] Hotfix Size:" + completeHandler.Result);
            //直接更新
            StartCoroutine(CheckUpdateCatalogs(_updateCatalogs));
        }

        /// <summary>
        /// 更新列表
        /// </summary>
        /// <param name="catalogs"></param>
        /// <returns></returns>
        IEnumerator CheckUpdateCatalogs(List<string> catalogs)
        {
            var handler = Addressables.UpdateCatalogs(catalogs);
            handler.Completed += UpdateCatalogsComplete;

            while (!handler.IsDone)
            {
                //更新进度
                var status = handler.GetDownloadStatus();
                float precent = status.Percent;
                Debug.LogFormat("[*] download Percent:{0}", precent);
                yield return null;
            }
        }

        /// <summary>
        /// 检测热更完成
        /// </summary>
        void UpdateCatalogsComplete(AsyncOperationHandle<List<IResourceLocator>> completeHandler)
        {
            RunScripts();
        }

        /// <summary>
        /// 替换最新代码
        /// </summary>
        void RunScripts()
        {
            hotfixLoader.RunScripts(LoadScene);
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        void LoadScene()
        {
            Addressables.LoadSceneAsync("Assets/HoxFixAssets/Scenes/MainScene.unity");
        }
    }
}