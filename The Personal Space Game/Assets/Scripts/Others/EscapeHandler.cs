using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EscapeHandler : MonoBehaviour
{
    public Button backBTN;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            backBTN.onClick.Invoke();
    }
}
