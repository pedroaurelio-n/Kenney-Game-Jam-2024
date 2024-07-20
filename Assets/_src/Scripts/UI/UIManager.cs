using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    const string HIGH_SCORE_KEY = "HighScore";
    
    [SerializeField] PlayerInput playerInput;
    [SerializeField] PlayerDeathbox playerDeathbox;
    
    [Header("Main Menu")]
    [SerializeField] GameObject mainMenuObject;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float timerCountdown;
    [SerializeField] float timerDelay;

    [Header("HUD")]
    [SerializeField] TimeManager timeManager;
    [SerializeField] GameObject hudObject;
    [SerializeField] GameObject arrowDirection;
    [SerializeField] TextMeshProUGUI pointsHudText;
    [SerializeField] TextMeshProUGUI highScoreHudText;
    
    [Header("Game Over")]
    [SerializeField] GameObject gameOverObject;
    [SerializeField] GameObject deathByCollisionText;
    [SerializeField] GameObject deathByTimeText;
    [SerializeField] TextMeshProUGUI pointsOverText;
    [SerializeField] TextMeshProUGUI highScoreOverText;

    WaitForSeconds timerWait;
    Coroutine countdownRoutine;

    int points;
    int currentHighScore;

    void Start ()
    {
        playerDeathbox.OnPlayerDeath += HandlePlayerDeath;
        playerInput.CanInput = false;
        timerWait = new WaitForSeconds(timerDelay);
        pointsHudText.text = points.ToString();
        currentHighScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY);
        highScoreHudText.text = currentHighScore.ToString();
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space) && countdownRoutine == null)
            countdownRoutine = StartCoroutine(CountdownRoutine());

        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
        if (Input.GetKeyDown(KeyCode.Return))
            UpdatePoints(1);
        
        if (Input.GetKeyDown(KeyCode.LeftControl))
            PlayerPrefs.DeleteKey(HIGH_SCORE_KEY);
    }

    public void UpdatePoints (int point)
    {
        points += point;
        int actualHighScore = points > currentHighScore ? points : currentHighScore;
        pointsHudText.text = points.ToString();
        highScoreHudText.text = actualHighScore.ToString();
    }

    void HandlePlayerDeath (bool deathByCollision)
    {
        if (gameOverObject.activeInHierarchy)
            return;
        
        hudObject.SetActive(false);
        arrowDirection.SetActive(false);
        
        currentHighScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY);
        if (points > currentHighScore)
        {
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, points);
            currentHighScore = points;
        }
        
        gameOverObject.SetActive(true);
        deathByCollisionText.SetActive(deathByCollision);
        deathByTimeText.SetActive(!deathByCollision);
        pointsOverText.text = points.ToString();
        highScoreOverText.text = currentHighScore.ToString();
    }

    IEnumerator CountdownRoutine ()
    {
        mainMenuObject.SetActive(false);
        timerText.gameObject.SetActive(true);
        timerText.text = ((int)timerCountdown).ToString();
        
        yield return timerWait;

        float timer = timerCountdown;
        while (timer >= -0.5f)
        {
            timer -= Time.deltaTime;
            timerText.text = Mathf.Max(0, Mathf.CeilToInt(timer)).ToString();

            if (timer <= 0)
                playerInput.CanInput = true;
            yield return null;
        }
        
        yield return timerWait;
        
        timerText.gameObject.SetActive(false);
        hudObject.SetActive(true);
        arrowDirection.SetActive(true);
        timeManager.StartTimer();
    }
}