using FFSM.GameManagers;
using Photon.Pun;
using UnityEngine;

namespace FFSM.GameElements
{
    public class SpaceshipController : MonoBehaviourPunCallbacks
    {
        [Header("Spaceship parameters")]
        public float Gravity = 10f;
        public float VerticalForce = 10f;
        public float HorizontalForce = 8f;
        public float RotationSpeed = 50f;
        public float MaxRotationAngle = 50f;
        public float DestroyTime;

        [Header("Fire")]
        public float ExtinguishRadius = 0.2f;
        public float ExtinguishDistance = 1;
        public LayerMask FireLayer;

        [Header("Componets")]
        public Thruster LeftThruster;
        public Thruster RightThruster;
        public Transform LandingPosition;
        public GameObject LeftMonke;
        public GameObject LeftMonkeOutline;
        public GameObject RightMonke;
        public GameObject RightMonkeOutline;

        [Header("Landing")]
        public LayerMask LandingPlatformLayerMask;
        public float MaxAllowedLandingSpeed = 5f;
        public float MaxAllowedLandingAngle = 35f;

        [Header("Obstacles")]
        public LayerMask ObstaclesLayerMask;

        [Header("VFX")]
        public GameObject ShipExplosion;
        public SpriteRenderer[] VisibleSprites;

        [Header("Sounds")]
        public AudioClip explosionSound;
        public AudioClip landingSound;

        private Vector3 _currentSpeed;
        private bool _leftButtonIsPressed;
        private bool _rightButtonIsPressed;
        private bool _canMove;
        private bool _canStartMovement;

        #region MonoBehaviour
        void Awake()
        {
            _canStartMovement = true;
            OutlinePlayerMonke();
        }

        void Update()
        {
            ReadInput();
            UpdateThrustersAnimation();

            if (PhotonNetwork.IsMasterClient)
            {
                AppplyGravity();
                ApplyRotation();
                ApplyThrustersForce();
                MoveSpaceship();
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                CheckLandingPlatform(collision);
                CheckObstacles(collision);
            }
        }

        void FixedUpdate()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                ExtinguishFire();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(LeftThruster.transform.position + LeftThruster.transform.up * ExtinguishDistance, ExtinguishRadius);
            Gizmos.DrawWireSphere(RightThruster.transform.position + RightThruster.transform.up * ExtinguishDistance, ExtinguishRadius);
        }
        #endregion

        #region Private methods
        private void ReadInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (photonView.IsMine)
                {
                    _leftButtonIsPressed = true;
                    photonView.RPC(nameof(RPC_EnableLeftThruster), RpcTarget.Others, true);
                }
                else
                {
                    _rightButtonIsPressed = true;
                    photonView.RPC(nameof(RPC_EnableRightThruster), RpcTarget.MasterClient, true);
                }
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (photonView.IsMine)
                {
                    _leftButtonIsPressed = false;
                    photonView.RPC(nameof(RPC_EnableLeftThruster), RpcTarget.Others, false);
                }
                else
                {
                    _rightButtonIsPressed = false;
                    photonView.RPC(nameof(RPC_EnableRightThruster), RpcTarget.MasterClient, false);
                }
            }


            if (_canStartMovement && (_rightButtonIsPressed || _leftButtonIsPressed))
            {
                _canStartMovement = false;
                _canMove = true;
            }
        }

        private void UpdateThrustersAnimation()
        {
            if (_canMove)
            {
                LeftThruster.EnableThruster(_leftButtonIsPressed);
                RightThruster.EnableThruster(_rightButtonIsPressed);
            }
        }

        private void AppplyGravity()
        {
            if (_canMove)
            {
                _currentSpeed.y -= Gravity * Time.deltaTime;
            }
        }

        private void ApplyRotation()
        {
            if (_canMove)
            {
                float rotationSpeed = 0;

                if (_leftButtonIsPressed)
                {
                    rotationSpeed -= RotationSpeed;
                }

                if (_rightButtonIsPressed)
                {
                    rotationSpeed += RotationSpeed;
                }

                transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

                float shipAngle = transform.eulerAngles.z > 180 ? -(360 - transform.eulerAngles.z) : transform.eulerAngles.z;

                transform.rotation = Quaternion.Euler(
                    transform.eulerAngles.x,
                    transform.eulerAngles.y,
                    Mathf.Clamp(shipAngle, -MaxRotationAngle, MaxRotationAngle));
            }
        }

        private void ApplyThrustersForce()
        {
            if (_leftButtonIsPressed)
            {
                _currentSpeed.x += HorizontalForce * Time.deltaTime;
                _currentSpeed.y += VerticalForce / 2 * Time.deltaTime;
            }

            if (_rightButtonIsPressed)
            {
                _currentSpeed.x -= HorizontalForce * Time.deltaTime;
                _currentSpeed.y += VerticalForce / 2 * Time.deltaTime;
            }
        }

        private void MoveSpaceship()
        {
            if (_canMove)
            {
                transform.position += _currentSpeed * Time.deltaTime;
            }
        }

        private void CheckLandingPlatform(Collision2D collision)
        {
            if (LayerIsInLayerMask(collision.gameObject.layer, LandingPlatformLayerMask))
            {
                if (_currentSpeed.magnitude > MaxAllowedLandingSpeed
                    || GetSpaceshipAngle() > MaxAllowedLandingAngle)
                {
                    Debug.Log("Level failed, Speed = " + _currentSpeed.magnitude + " Angle = " + GetSpaceshipAngle());
                    LevelManager.Instance.FailLevel();
                    photonView.RPC(nameof(RPC_DestroyShip), RpcTarget.AllViaServer);
                }
                else if (LevelManager.Instance.AllFireExtinguished())
                {
                    LevelManager.Instance.CompleteLevel();
                    AudioManager.Instance.PlaySound(landingSound);
                    SnapSpaceshipToGround(collision.GetContact(0).point);
                }
                else
                {
                    Debug.Log("Not all fires are extinguished");
                    LevelManager.Instance.WarningFires();
                    AudioManager.Instance.BuzzSound();
                    SnapSpaceshipToGround(collision.GetContact(0).point);
                    _canStartMovement = true;
                }
            }
        }

        private void SnapSpaceshipToGround(Vector2 contactPoint)
        {
            _canMove = false;
            transform.rotation = Quaternion.identity;
            Vector3 displacement = LandingPosition.position - transform.position;
            transform.position = new Vector3(contactPoint.x, contactPoint.y, 0) - displacement;
            _currentSpeed = Vector3.zero;
        }

        private void CheckObstacles(Collision2D collision)
        {
            if (LayerIsInLayerMask(collision.gameObject.layer, ObstaclesLayerMask))
            {
                LevelManager.Instance.FailLevel();
                photonView.RPC(nameof(RPC_DestroyShip), RpcTarget.AllViaServer);
            }
        }

        private void ExtinguishFire()
        {
            if (_leftButtonIsPressed)
            {
                var fire = DetectFire(LeftThruster.transform.position, LeftThruster.transform.up);
                if (fire != null)
                {
                    fire.Extinguish();
                }
            }

            if (_rightButtonIsPressed)
            {
                var fire = DetectFire(RightThruster.transform.position, RightThruster.transform.up);
                if (fire != null)
                {
                    fire.Extinguish();
                }
            }
        }

        private Fire DetectFire(Vector2 origin, Vector2 direction)
        {
            var fire = Physics2D.CircleCast(origin, ExtinguishRadius, direction, ExtinguishDistance, FireLayer);

            return fire.collider?.gameObject.GetComponent<Fire>();
        }

        private void OutlinePlayerMonke()
        {
            if (photonView.IsMine)
            {
                LeftMonke.SetActive(false);
                LeftMonkeOutline.SetActive(true);
                RightMonke.SetActive(true);
                RightMonkeOutline.SetActive(false);
            }
            else
            {
                LeftMonke.SetActive(true);
                LeftMonkeOutline.SetActive(false);
                RightMonke.SetActive(false);
                RightMonkeOutline.SetActive(true);
            }
        }
        #endregion

        #region RPC
        [PunRPC]
        private void RPC_EnableLeftThruster(bool enable)
        {
            _leftButtonIsPressed = enable;
        }

        [PunRPC]
        private void RPC_EnableRightThruster(bool enable)
        {
            _rightButtonIsPressed = enable;
        }

        [PunRPC]
        private void RPC_DestroyShip()
        {
            foreach (var sprite in VisibleSprites)
            {
                sprite.enabled = false;
            }
            ShipExplosion.SetActive(true);
            AudioManager.Instance.PlaySound(explosionSound);
            _canMove = false;
            Destroy(gameObject, DestroyTime);
        }
        #endregion

        #region Utils

        private bool LayerIsInLayerMask(int layer, LayerMask layerMask)
        {
            return layerMask == (layerMask | (1 << layer));
        }

        private float GetSpaceshipAngle()
        {
            return Mathf.Abs(Vector3.Angle(Vector3.up, transform.up));
        }
        #endregion
    }
}
