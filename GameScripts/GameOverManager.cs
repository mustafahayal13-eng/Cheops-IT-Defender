using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;

    [Header("Panneau Game Over")]
    public GameObject gameOverPanel;

    [Header("Textes")]
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI finalScoreText;

    [Header("Bouton")]
    public Button restartButton;

    private int finalScore = 0;

    void Awake()
    {
        Instance = this;
        // Cacher le panneau au départ
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    public void ShowGameOver(int score)
    {
        finalScore = score;

        // Afficher le panneau
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // Mettre à jour le score
        if (finalScoreText != null)
            finalScoreText.text = "Score : " + score;

        // Arrêter le temps
        Time.timeScale = 0f;

        // Relier le bouton Rejouer
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
    }

    public void RestartGame()
    {
        // Remettre le temps à la normale
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
