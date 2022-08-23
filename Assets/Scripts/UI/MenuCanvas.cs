using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuCanvas : MonoBehaviour
{
    public Button createRoomButton;

    private void Start()
    {
        createRoomButton.onClick.AddListener(() => NetworkManager.Instance.CreateNewRoom());
    }
}
