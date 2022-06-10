using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneController : MonoBehaviour
{
    [SerializeField]
    Text text;

    // Start is called before the first frame update
    void Start()
    {
        text.text = "update suc";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
