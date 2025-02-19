using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunshot : MonoBehaviour
{
    public AudioSource mySounds;
    public AudioClip gunshot;

    // For recoil
    public Camera playerCamera; // Reference to the player's camera
    public float recoilAmount = 0.1f; // Amount of recoil movement
    public float recoilRotationAmount = 2f; // Amount of rotation for recoil
    public float recoilResetSpeed = 5f; // Speed at which the camera returns to normal
    public float recoilTime = 1.0f; // Time that recoil lasts

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 recoilPosition;
    private float recoilTimeLeft = 1.0f;

    void Start()
    {
        initialPosition = playerCamera.transform.localPosition;
        initialRotation = playerCamera.transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Cursor.lockState == CursorLockMode.Locked)
        {
            mySounds.PlayOneShot(gunshot);

            // Apply recoil if any recoil time is left
            if (recoilTimeLeft > 0)
            {
                // Decrease recoil time
                recoilTimeLeft -= Time.deltaTime;

                // Apply recoil movement (position and rotation)
                playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, initialPosition + recoilPosition, Time.deltaTime * recoilResetSpeed);
                playerCamera.transform.localRotation = Quaternion.Lerp(playerCamera.transform.localRotation, initialRotation, Time.deltaTime * recoilResetSpeed);
            }
        }
    }

    public void ApplyRecoil()
    {
        // Apply recoil when the player fires
        recoilTimeLeft = recoilTime;

        // Recoil position (backward) and rotation (upward)
        recoilPosition = new Vector3(Random.Range(-recoilAmount, recoilAmount), Random.Range(-recoilRotationAmount, recoilRotationAmount), 0);
    }
}
