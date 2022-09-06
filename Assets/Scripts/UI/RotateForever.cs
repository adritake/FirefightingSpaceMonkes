using UnityEngine;

namespace FFSM
{
    public class RotateForever : MonoBehaviour
    {
        public float RotationSpeed;

        void Update()
        {
            Rotate();
        }

        private void Rotate()
        {
            transform.Rotate(Vector3.forward, RotationSpeed * Time.deltaTime);
        }
    }
}
