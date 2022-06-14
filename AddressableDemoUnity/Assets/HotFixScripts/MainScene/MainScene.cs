using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class MainScene : MonoBehaviour
{
    [SerializeField]
    Text text;

    // Start is called before the first frame update
    void Start()
    {
        text.text = "更新个屁？？？？？？？？焯？";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickToGameScene()
    {
        var handler = Addressables.LoadSceneAsync("Assets/HoxFixAssets/Scenes/GameScene.unity");
    }
}
