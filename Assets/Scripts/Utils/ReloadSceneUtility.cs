using FFSM.GameManagers;
using System.Collections;
using UnityEngine;

namespace FFSM.Utils
{
    public class ReloadSceneUtility : MonoBehaviour
    {
        public float ReloadTime;

        void Start()
        {
            StartCoroutine(LoadCurrentLevelCoroutine());
        }

        private IEnumerator LoadCurrentLevelCoroutine()
        {
            yield return new WaitForSeconds(ReloadTime);
            PunSceneManager.Instance.LoadCurrentLevel();
        }
    }
}
