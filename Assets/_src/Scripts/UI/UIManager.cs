using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    [SerializeField] PlayerDeathbox playerDeathbox;
    
    [Header("Main Menu")]
    [SerializeField] GameObject mainMenuObject;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float timerCountdown;
    [SerializeField] float timerDelay;

    [Header("Game Over")]
    [SerializeField] GameObject gameOverObject;

    WaitForSeconds timerWait;
    Coroutine countdownRoutine;

    void Start ()
    {
        playerDeathbox.OnPlayerDeath += HandlePlayerDeath;
        playerInput.CanInput = false;
        timerWait = new WaitForSeconds(timerDelay);
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space) && countdownRoutine == null)
            countdownRoutine = StartCoroutine(CountdownRoutine());

        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void HandlePlayerDeath ()
    {
        if (gameOverObject.activeInHierarchy)
            return;
        
        gameOverObject.SetActive(true);
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
        countdownRoutine = null;
    }
}