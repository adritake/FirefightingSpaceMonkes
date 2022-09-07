using DG.Tweening;
using TMPro;
using UnityEngine;

namespace FFSM.UI
{
    public class TutorialText : MonoBehaviour
    {
        public float BlinkTime = 0.5f;

        private TextMeshProUGUI _text;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            _text
                .DOColor(Color.clear, BlinkTime)
                .SetEase(Ease.InQuad)
                .SetLoops(-1, LoopType.Yoyo);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
