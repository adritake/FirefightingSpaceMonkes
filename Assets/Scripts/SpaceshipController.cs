using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    [Header("Spaceship parameters")]
    public float Gravity = 10f;
    public float VerticalForce = 10f;
    public float HorizontalForce = 8f;
    public float RotationSpeed = 50f;
    public float MaxRotationAngle = 50f;

    [Header("Fire")]
    public float ExtinguishRadius = 0.2f;
    public float ExtinguishDistance = 1;
    public LayerMask FireLayer;

    [Header("Componets")]
    public GameObject LeftThruster;
    public GameObject RightThruster;
    public Transform LandingPosition;

    [Header("Landing")]
    public LayerMask LandingPlatformLayerMask;
    public float MaxAllowedLandingSpeed = 5f;
    public float MaxAllowedLandingAngle = 35f;

    [Header("Obstacles")]
    public LayerMask ObstaclesLayerMask;

    private Vector3 _currentSpeed;
    private bool _leftButtonIsPressed;
    private bool _rightButtonIsPressed;
    private bool _canMove;
    private bool _canStartMovement;

    #region MonoBehaviour
    void Awake()
    {
        _canStartMovement = true;
    }

    void Update()
    {
        ReadInputs();
        UpdateThrustersAnimation();
        AppplyGravity();
        ApplyRotation();
        ApplyThrustersForce();
        MoveSpaceship();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        CheckLandingPlatform(collision);
        CheckObstacles(collision);
    }

    void FixedUpdate()
    {
        ExtinguishFire();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (_leftButtonIsPressed)
        {
            Gizmos.DrawWireSphere(LeftThruster.transform.position + LeftThruster.transform.up * ExtinguishDistance, ExtinguishRadius);
        }

        if (_rightButtonIsPressed)
        {
            Gizmos.DrawWireSphere(RightThruster.transform.position + RightThruster.transform.up * ExtinguishDistance, ExtinguishRadius);
        }
    }
    #endregion

    #region Private methods
    private void ReadInputs()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _leftButtonIsPressed = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            _leftButtonIsPressed = false;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _rightButtonIsPressed = true;
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            _rightButtonIsPressed = false;
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
            LeftThruster.SetActive(_leftButtonIsPressed);
            RightThruster.SetActive(_rightButtonIsPressed);
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
                Destroy(gameObject);
            }
            else if (LevelManager.Instance.AllFireExtinguished())
            {
                Debug.Log("Level completed!");
                SnapSpaceshipToGround(collision.GetContact(0).point);
            }
            else
            {
                Debug.Log("Not all fires are extinguished");
                SnapSpaceshipToGround(collision.GetContact(0).point);
                _canStartMovement = true;
            }
        }
    }

    private void SnapSpaceshipToGround(Vector2 contactPoint)
    {
        _canMove = false;
        transform.rotation = Quaternion.identity;
        transform.position = new Vector3(contactPoint.x, contactPoint.y, 0) - LandingPosition.localPosition;
        _currentSpeed = Vector3.zero;
    }

    private void CheckObstacles(Collision2D collision)
    {
        if (LayerIsInLayerMask(collision.gameObject.layer, ObstaclesLayerMask))
        {
            Debug.Log("Level failed, you hit an obstacle");
            Destroy(gameObject);
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
