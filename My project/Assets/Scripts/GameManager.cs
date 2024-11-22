using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{
    
   public static float moveSpeed = 0f; 
    public Transform player; 
    public Transform cam;
    public Camera Camera1;
    public GameObject Tree_1; 
    public GameObject Tree_2;
    public GameObject Tree_3;
    public GameObject Mushroom_1;
    public float platformSpacing = 2f; 
    public float platformHeight = 5f; // Height of the platform
    public float horizontalRange = 2f; // Range for horizontal position of platforms
    public bool isGameOver = false;

    private float nextPlatformY; 
    private List<GameObject> gameObjectsList;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameObject GameOverText;
    [SerializeField] GameObject ResetButton;
    
    private void Start()
    {
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
    Camera.main.transform.position += Vector3.up * moveSpeed * Time.deltaTime;
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
    float aspectRatio = Camera1.aspect;
    float halfWidth = halfHeight * aspectRatio;
    float leftEdge = cam.transform.position.x - halfWidth;
    float rightEdge = cam.transform.position.x + halfWidth;
    float randomX = Random.Range(0.5f,4.5f);
    float cameraTopY = Camera.main.transform.position.y + Camera.main.orthographicSize;
    Vector3 platformPosition = new Vector3(randomX, cameraTopY + platformHeight / 2, 0);
    int maxAttempts = 5;
    bool isSpawnValid = false;
    for (int attempt = 0; attempt < maxAttempts; attempt++)
    {

        Collider2D overlap = Physics2D.OverlapCircle(platformPosition, 0.5f); 

       
        if (overlap == null)
        {
            int randomIndex = Random.Range(0, gameObjectsList.Count);
            Instantiate(gameObjectsList[randomIndex], platformPosition, Quaternion.identity);
            isSpawnValid = true;
            break; 
        }
        else
        {
            
            platformPosition.x = Random.Range(1, 4);
        }
    }
    if (!isSpawnValid)
    {
        Debug.Log("Failed to spawn platform after " + maxAttempts + " attempts.");
    }
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

