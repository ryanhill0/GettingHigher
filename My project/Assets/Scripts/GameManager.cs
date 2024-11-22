using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
   public static float moveSpeed = 0f; // Speed at which the screen moves up
    public Transform player; // Reference to the player
    public Transform cam;
    public Camera Camera1;
    public GameObject Tree_1; // Reference to the platform prefab
    public GameObject Tree_2;
    public GameObject Tree_3;
    public GameObject Mushroom_1;
    public float platformSpacing = 2f; // Space between platforms
    public float platformHeight = 5f; // Height of the platform
    public float horizontalRange = 2f; // Range for horizontal position of platforms
    public bool isGameOver = false;

    private float nextPlatformY; // Position to spawn the next platform
    private List<GameObject> gameObjectsList;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameObject GameOverText;
    [SerializeField] GameObject ResetButton;
    
    private void Start()
    {
        // Initialize the next platform position to the camera's upper edge
        nextPlatformY = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y + platformHeight / 2;
        gameObjectsList = new List<GameObject>
        {
            Tree_1,
            Tree_2,
            Tree_3,
            Mushroom_1
        };
    }

    private void Update()
{
    // Move the camera up
    Camera.main.transform.position += Vector3.up * moveSpeed * Time.deltaTime;
    // Ensure you're accessing the player's current position
        float playerYPosition = player.position.y;
    if(playerYPosition > cam.position.y)
    {
                cam.position = new Vector3(cam.position.x, Mathf.Lerp(cam.position.y, player.position.y, moveSpeed * Time.deltaTime), cam.position.z);

    }
        

    if (player.position.y < cam.position.y -5.5)
    {
        
            endGame();
    }
        float CameraPos = Camera.main.transform.position.y;
    float cameraTopY = CameraPos + Camera.main.orthographicSize;
    if (cameraTopY >= nextPlatformY)
    {
        SpawnPlatform();
    }
        updateScore(CameraPos);
}

private void SpawnPlatform()
{
    float halfHeight = Camera1.orthographicSize;

    // Get the aspect ratio (width / height) of the camera's screen
    float aspectRatio = Camera1.aspect;

    // Calculate the half-width of the camera's view
    float halfWidth = halfHeight * aspectRatio;

    // Calculate the left and right edges in world coordinates
    float leftEdge = cam.transform.position.x - halfWidth;
    float rightEdge = cam.transform.position.x + halfWidth;

    // Generate a random X position within the camera's bounds
    float randomX = Random.Range(0.5f,4.5f);

    // Calculate the top edge of the camera
    float cameraTopY = Camera.main.transform.position.y + Camera.main.orthographicSize;

    // Define the platform's spawn position
    Vector3 platformPosition = new Vector3(randomX, cameraTopY + platformHeight / 2, 0);

    // Maximum number of attempts to find a valid spawn position
    int maxAttempts = 5;
    bool isSpawnValid = false;

    // Retry up to maxAttempts times if there's an overlap
    for (int attempt = 0; attempt < maxAttempts; attempt++)
    {
        // Check if there's an overlap at the platform's spawn position (using a small radius check)
        Collider2D overlap = Physics2D.OverlapCircle(platformPosition, 0.5f); // Adjust radius as needed

        // If no overlap is found, spawn the platform
        if (overlap == null)
        {
            int randomIndex = Random.Range(0, gameObjectsList.Count);
            Instantiate(gameObjectsList[randomIndex], platformPosition, Quaternion.identity);
            isSpawnValid = true;
            break; // Exit loop if spawn is successful
        }
        else
        {
            // If there's an overlap, adjust position and try again
            platformPosition.x = Random.Range(1, 4);
        }
    }

    if (!isSpawnValid)
    {
        Debug.Log("Failed to spawn platform after " + maxAttempts + " attempts.");
    }

    // Set the next platform's Y position for the next spawn
    nextPlatformY = cameraTopY + platformHeight + platformSpacing;
}
    private void updateScore(float cameraPos){
        int roundedScore = Mathf.RoundToInt(cameraPos * 10);
        scoreText.text = "Score: " + roundedScore.ToString();
    }
    public void endGame(){
        GameOverText.SetActive(true);
        ResetButton.SetActive(true);
        moveSpeed = 0f;
    }
    
}

