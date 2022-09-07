using UnityEngine;

namespace FFSM.GameManagers
{
    public class CameraManager : MonoBehaviour
    {
        public Transform Target;
        public Transform CameraOrigin;
        public Vector2 MovementPlaneSize;

        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void LateUpdate()
        {
            FollowTarget();
        }

        private void FollowTarget()
        {
            if (Target)
            {
                Vector2 newCameraPosition = Target.position;

                float maxXLimit = CameraOrigin.position.x + MovementPlaneSize.x / 2;
                float minXLimit = CameraOrigin.position.x - MovementPlaneSize.x / 2;
                float maxYLimit = CameraOrigin.position.y + MovementPlaneSize.y / 2;
                float minYLimit = CameraOrigin.position.y - MovementPlaneSize.y / 2;

                if (Target.position.x > maxXLimit)
                {
                    newCameraPosition.x = maxXLimit;
                }
                else if (Target.position.x < minXLimit)
                {
                    newCameraPosition.x = minXLimit;
                }

                if (Target.position.y > maxYLimit)
                {
                    newCameraPosition.y = maxYLimit;
                }
                else if (Target.position.y < minYLimit)
                {
                    newCameraPosition.y = minYLimit;
                }

                _mainCamera.transform.position = new Vector3(newCameraPosition.x, newCameraPosition.y, _mainCamera.transform.position.z);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(CameraOrigin.position, MovementPlaneSize);
        }
    }
}
