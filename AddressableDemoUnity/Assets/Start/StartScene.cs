using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using zFramework.Hotfix.Toolkit;

public class StartScene : MonoBehaviour
{
    [SerializeField, Tooltip("代码执行脚本")]
    HotfixLoader hotfixLoader;

    void Start()
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
        StartCoroutine(LoadAssemblyAsync());
    }

    /// <summary>
    /// 解析代码
    /// </summary>
    /// <returns></returns>
    public IEnumerator LoadAssemblyAsync()
    {
        //下载脚本
        AsyncOperationHandle handler = Addressables.DownloadDependenciesAsync("HotFixScripts");
        while (!handler.IsDone)
        {
            yield return null;
        }
        if (handler.Status == AsyncOperationStatus.Succeeded)
        {
            hotfixLoader.RunScripts(ChangeHRMScene);
        }
    }

    /// <summary>
    /// 切换到热更场景
    /// </summary>
    void ChangeHRMScene()
    {
        Addressables.LoadSceneAsync("HRMScene");
    }
}
