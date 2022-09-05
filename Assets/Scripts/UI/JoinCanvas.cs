using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinCanvas : MonoBehaviour
{
    public GameObject menuCanvas;
    public GameObject roomCanvas;

    public void MenuButtonClicked()
    {
        //Show Menu
        gameObject.SetActive(false);
    }

    public void ShowRoomCanvas()
    {
        roomCanvas.SetActive(true);
        gameObject.SetActive(false);
    }

}
