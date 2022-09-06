using UnityEngine;
using TMPro;

public class TitleCanvas : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputNameField;

    private void Awake()
    {
        _inputNameField = GetComponentInChildren<TMP_InputField>();
    }

    public void StartButtonClicked()
    {
        if (!string.IsNullOrEmpty(_inputNameField.text))
        {
            NetworkManager.Instance.SetPlayerName(_inputNameField.text);
            NetworkManager.Instance.Login();
        }
        else
        {
            Debug.LogError("Monke name empty");
        }
    }
}
