using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MouseLook : MonoBehaviour
{
    public Slider mySlider;
    public float mouseSensitivity;
    public Transform playerBody;
    private float xRotation = 0f;
    public GameObject pausePanel;
    public GameObject tab;
    [SerializeField]
    private TextMeshProUGUI mouseSensitivityLabel;
    public static bool gamePaused;

    public void Unpause() {
        // Update boolean for game state (pause)
        gamePaused = false;

        // Lock cursor since player will not require it during gameplay
        Cursor.lockState = CursorLockMode.Locked;

        // Set pause/unpause screen UI to appropriate values to toggle them on/off
        pausePanel.SetActive(false);
        tab.SetActive(true);
        mouseSensitivity = mySlider.value;
        mouseSensitivityLabel.color = new Color(0,0,0);
        mouseSensitivityLabel.text = "pause";
        mySlider.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        Unpause();
    }


    // Update is called once per frame
    void Update()
    {
        // Pause screen when escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gamePaused = true;
            pausePanel.SetActive(true);
            mouseSensitivityLabel.color = new Color(255,255,255);
            mouseSensitivityLabel.text = "mouse sensitivity";
            mySlider.gameObject.SetActive(true);
            tab.SetActive(false);
        }

        // Only allow camera movement when game is unpaused
        if (gamePaused == false) {
            // Get X and Y mouse rotation values 
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity
                * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity
                * Time.deltaTime;
            xRotation -= mouseY;

            // Clamp mouse rotation to mimic human rotation
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            // Perform rotation on the player body
            transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            playerBody.Rotate(Vector3.up * mouseX);
        } else {
            Cursor.lockState = CursorLockMode.None;
        }

    }
}
