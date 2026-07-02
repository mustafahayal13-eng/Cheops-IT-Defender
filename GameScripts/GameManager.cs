using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Score")]
    public int score = 0;
    public TextMeshProUGUI scoreText;

    [Header("Vies")]
    public int lives = 3;
    public TextMeshProUGUI livesText;

    [Header("Spawner")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 3f;
    private float spawnTimer = 0f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        // Faire apparaître les ennemis
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnEnemy();
            spawnTimer = spawnInterval;
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null || spawnPoints.Length == 0) return;
        int i = Random.Range(0, spawnPoints.Length);
        Instantiate(enemyPrefab, spawnPoints[i].position, Quaternion.identity);
    }

    public void AddScore(int value)
    {
        score += value;
        UpdateUI();
    }

    public void LoseLife()
    {
        lives--;
        UpdateUI();
        if (lives <= 0) GameOver();
    }

    void GameOver()
    {
        Debug.Log("GAME OVER !");
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score : " + score;
        if (livesText != null) livesText.text = "Vies : " + lives;
    }
}
