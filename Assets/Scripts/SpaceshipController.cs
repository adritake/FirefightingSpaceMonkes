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
    private bool _levelStarted;

    #region MonoBehaviour
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

        if (!_levelStarted && (_rightButtonIsPressed || _leftButtonIsPressed))
        {
            _levelStarted = true;
            _canMove = true;
        }
    }

    private void UpdateThrustersAnimation()
    {
        LeftThruster.SetActive(_leftButtonIsPressed);
        RightThruster.SetActive(_rightButtonIsPressed);
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
            else
            {
                Debug.Log("Level completed!");
                _canMove = false;
                transform.rotation = Quaternion.identity;
                Vector2 contactPoint = collision.GetContact(0).point;
                transform.position = new Vector3(contactPoint.x, contactPoint.y, 0) - LandingPosition.localPosition;
            }
        }
    }

    private void CheckObstacles(Collision2D collision)
    {
        if (LayerIsInLayerMask(collision.gameObject.layer, ObstaclesLayerMask))
        {
            Debug.Log("Level failed, you hit an obstacle");
            Destroy(gameObject);
        }
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
