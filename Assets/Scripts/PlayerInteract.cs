using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteract : MonoBehaviour
{
    public Camera cam;
    private int[] pointsBasedOnAccuracy = {15, 10, 5, 3, 1};
    private double[] distanceBoundaries = { 0.23, 0.31, 0.4, 0.5, 1.0};

    [SerializeField]
    private float distance = 3f;

    [SerializeField]
    private LayerMask mask;

    private PlayerUI playerUI;

    [SerializeField]
    private GameObject spawnObject;

    //public GameObject target;
    public List<GameObject> allTargets;

    public TextMeshProUGUI pointsText;
    public int numOfPoints;

    public TextMeshProUGUI accuracyPercent;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 initPosition = new Vector3(-4, 12, 9);
        playerUI = GetComponent<PlayerUI>();
        playerUI.UpdateText(string.Empty);
        allTargets.Add(Instantiate(spawnObject, initPosition, Quaternion.identity));
    }

    private IEnumerator WaitSeconds(float waitTime)
    {
        Debug.Log("Coroutine started");
        yield return new WaitForSeconds(waitTime);
        Debug.Log("Coroutine ended");
        playerUI.UpdateText(string.Empty);
    }

    Vector3 generatePosition(int randIdx)
    {
        // Generate random position on wall 1 
        Vector3 position = new Vector3(Random.Range(-4, 8), Random.Range(8, 10), 9);

        // Generate random position on wall 2 (left wall)
        Vector3 position2 = new Vector3(-9, Random.Range(8, 10), Random.Range(-9, 7.5f));

        // Generate random position on wall 3 (right wall)
        Vector3 position3 = new Vector3(9.38f, Random.Range(8, 10), Random.Range(-9, 7.5f));

        Vector3[] positions = { position, position2, position3 };

        return positions[randIdx];
    }

    // Update is called once per frame
    void Update()
    {
        if (!MouseLook.gamePaused) {
            // Initialize ray position relative to camera position
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            Debug.DrawRay(ray.origin, ray.direction * distance);

            // Stores collision info
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, distance, mask))
            {
                // Ray collides with an object of the class "Interactable"
                // (i.e. the target)
                if (hitInfo.collider.GetComponent<Interactable>() != null)
                {
                    Interactable interactable = hitInfo.collider.GetComponent
                        <Interactable>();

                    // Player clicks (shoots) the target
                    if (Input.GetMouseButtonDown(0))
                    {
                        GameObject hitObject = hitInfo.collider.gameObject;
                        Vector3 objectCenter = hitObject.transform.position; // Center of the GameObject
                        Vector3 hitPoint = hitInfo.point;

                        // Calculate the distance from the center
                        float distanceFromCenter = Vector3.Distance(objectCenter, hitPoint);

                        int pointsToUpdate = 0;

                        if (distanceFromCenter < distanceBoundaries[0]) {
                            pointsToUpdate = pointsBasedOnAccuracy[0];
                            accuracyPercent.text = "Excellent";
                            accuracyPercent.color = Color.green;
                        } 
                        else if (distanceFromCenter < distanceBoundaries[1])
                        {
                            pointsToUpdate = pointsBasedOnAccuracy[1];
                            accuracyPercent.text = "Good";
                            accuracyPercent.color = Color.blue;
                        }
                        else if (distanceFromCenter < distanceBoundaries[2])
                        {
                            pointsToUpdate = pointsBasedOnAccuracy[2];
                            accuracyPercent.text = "Satisfactory";
                            accuracyPercent.color = Color.yellow;
                        }
                        else if (distanceFromCenter < distanceBoundaries[3])
                        {
                            pointsToUpdate = pointsBasedOnAccuracy[3];
                            accuracyPercent.text = "OK";
                            accuracyPercent.color = new Color(237, 123, 52);
                        }
                        else if (distanceFromCenter < distanceBoundaries[4])
                        {
                            pointsToUpdate = pointsBasedOnAccuracy[4];
                            accuracyPercent.text = "Meh";
                            accuracyPercent.color = Color.red;
                        }

                        // Generating new position of the target:
                        int randIdx = Random.Range(0, 3);
                        Vector3 chosenPosition = generatePosition(randIdx);

                        // Display "Target hit" text and increment number of points
                        playerUI.UpdateText(interactable.promptMessage);
                        numOfPoints += pointsToUpdate;
                        pointsText.text = numOfPoints.ToString();

                        // Destroy all targets before instantiating the new target
                        for (int i = 0; i < allTargets.Count; ++i)
                        {
                            Destroy(allTargets[i]);
                        }

                        // Rotate target 90 degrees if it is on the left or right
                        // walls
                        if (randIdx != 0)
                        {
                            allTargets.Add(Instantiate(spawnObject, chosenPosition,
                                Quaternion.Euler(0, 90, 0)));
                        } else
                        {
                            allTargets.Add(Instantiate(spawnObject, chosenPosition,
                                Quaternion.identity));
                        }
                        StartCoroutine(WaitSeconds(3f));
            
                    }
                }
            }
        }
    }
}
