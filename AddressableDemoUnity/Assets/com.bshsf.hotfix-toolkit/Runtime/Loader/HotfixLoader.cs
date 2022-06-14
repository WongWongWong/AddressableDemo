using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace zFramework.Hotfix.Toolkit
{
    [DisallowMultipleComponent]
    public class HotfixLoader : MonoBehaviour
    {
        public AssetReferenceHotfixAssemblis hotfixAssemblies;

        public void RunScripts(Action complete)
        {
            StartCoroutine(LoadScripts(complete));
        }

        IEnumerator LoadScripts(Action complete)
        {
            var handler = hotfixAssemblies.LoadAssetAsync();
            yield return handler;
            if (handler.Status == AsyncOperationStatus.Succeeded)
            {
                var so = hotfixAssemblies.Asset as HotfixAssembliesData;
                yield return so.LoadAssemblyAsync();
            }
            hotfixAssemblies.ReleaseAsset();

            complete?.Invoke();
        }



#if UNITY_EDITOR
        private void Reset()
        {
            if (hotfixAssemblies == null)
            {
                if (UnityEditor.AssetDatabase.TryGetGUIDAndLocalFileIdentifier(HotfixAssembliesData.Instance, out string guid, out long id))
                {
                    hotfixAssemblies = new AssetReferenceHotfixAssemblis(guid);
                }
            }
            else if (!hotfixAssemblies.editorAsset)
            {
                hotfixAssemblies.SetEditorAsset(HotfixAssembliesData.Instance);
            }
        }
#endif

        [Serializable]
        public class AssetReferenceHotfixAssemblis : AssetReferenceT<HotfixAssembliesData>
        {
            public AssetReferenceHotfixAssemblis(string guid) : base(guid) { }
        }
    }
}