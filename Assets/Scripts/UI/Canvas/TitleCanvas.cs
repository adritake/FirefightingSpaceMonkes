using UnityEngine;
using TMPro;

namespace FFSM
{
    public class TitleCanvas : MonoBehaviour
    {
        [Tooltip("Input field for player name")]
        [SerializeField] private TMP_InputField _inputNameField;

        #region Monobehaviour
        private void Awake()
        {
            _inputNameField = GetComponentInChildren<TMP_InputField>();
        }

        #endregion

        #region Public Methods
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

        #endregion

    }
}
