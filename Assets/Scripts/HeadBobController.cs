using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobController : MonoBehaviour
{
    [SerializeField] private bool effectEnabled = true;

    [SerializeField, Range(0, 0.01f)] private float amplitude = 0.001f;
    [SerializeField, Range(0, 30f)] private float frequency = 10.0f;

    [SerializeField] private Transform effectCamera = null;
    [SerializeField] private Transform cameraHolder = null;

    private float toggleSpeed = 3.0f;
    private Vector3 startPos;
    private CharacterController characterController;


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        startPos = effectCamera.localPosition; 
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!effectEnabled)
        {
            return;
        }
        CheckMotion();
        ResetPosition();
        effectCamera.LookAt(FocusTarget());
    }

    private void PlayMotion(Vector3 motion)
    {
        effectCamera.localPosition += motion;
    }
    private void CheckMotion()
    {
        float speed = new Vector3(characterController.velocity.x, 0, characterController.velocity.z).magnitude;

        if (speed < toggleSpeed)
        {
            return;
        }
        if (!characterController.isGrounded)
        {
            return;
        }
        PlayMotion(FootStepMotion());
    }

    private void ResetPosition()
    {
        if (effectCamera.localPosition == startPos)
        {
            return;
        }
        effectCamera.localPosition = Vector3.Lerp(effectCamera.localPosition, startPos, 1 * Time.deltaTime);
    }
    private Vector3 FootStepMotion()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * frequency) * amplitude;
        pos.x += Mathf.Cos(Time.time * frequency / 2) * amplitude * 2;
        return pos;
    }
    private Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + cameraHolder.localPosition.y, transform.position.z);
        pos += cameraHolder.forward * 15.0f;
        return pos;
    }
}
