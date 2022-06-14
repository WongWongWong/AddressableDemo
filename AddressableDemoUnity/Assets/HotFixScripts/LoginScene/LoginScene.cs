using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var text = GameObject.Find("Text");
        if (text)
        {
            text.GetComponent<Text>().text = "LoginScene";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
