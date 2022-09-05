using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinCanvas : MonoBehaviour
{
    public GameObject menuCanvas;
    public GameObject roomCanvas;
    public GameObject monkes;

    public void MenuButtonClicked()
    {
        //Show Menu
        gameObject.SetActive(false);
        menuCanvas.SetActive(true);
        monkes.SetActive(true);
    }

    public void ShowRoomCanvas()
    {
        roomCanvas.SetActive(true);
        gameObject.SetActive(false);
    }

}
