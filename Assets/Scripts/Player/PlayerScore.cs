using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Text.RegularExpressions;

public class PlayerScore : MonoBehaviour
{
    #region Variables
    public int score;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] private GameObject gameHUD;    
    [SerializeField] private GameObject readyHUD;
    [SerializeField] private GameObject deathMenu;    
    [SerializeField] private GameObject winMenu;

    [SerializeField] private TextMeshProUGUI deathMessageText;

    [SerializeField] private int levelEndSeconds = 60;
    private int levelSeconds = 0;
    private int levelMinutes = 0;

    [HideInInspector] public bool isDead = false;
    [HideInInspector] public bool hasWon = false;
    private float waitTimer = 0.0f;

    [SerializeField] private string[] deathTimerMessages;

    private GameManager gameManager;

    public bool isReady = false;

    [SerializeField] private AudioClip winSFX;
    [SerializeField] private AudioClip loseSFX;
    private AudioSource audioSource;
    #endregion
    private void Start()
    {
        SetLevelTimer();

        // Set initial UI
        readyHUD.SetActive(true);
        gameHUD.SetActive(true);       
        deathMenu.SetActive(false);        
        winMenu.SetActive(false);

        TimerTextUpdate();

        gameManager = GameObject.FindObjectOfType<GameManager>();
        audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isReady)
        {
            if (!isDead && !hasWon)
            {
                LevelTimer();
            }
        }
    }

    public void Die(string deathMessage)
    {
        if (!isDead && isReady)
        {
            gameHUD.SetActive(false);
            deathMessageText.text = deathMessage;
            deathMenu.SetActive(true);
            isDead = true;
            Cursor.visible = true;

            audioSource.PlayOneShot(loseSFX);
            GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioSource>().Stop();
        }

    }

    public void Ready()
    {
        readyHUD.SetActive(false);
        isReady = true;
        Cursor.visible = false;
    }

    public void Win(int levelToUnlock = 0)
    {
        hasWon = true;
        gameHUD.SetActive(false);
        deathMenu.SetActive(false);
        winMenu.SetActive(true);
        Cursor.visible = true;

        if (!gameManager)
        {
            Debug.LogError(this.gameObject.name + " hasn't found GameManager.");
            return;
        }

        gameManager.ChangeBoats(score);
        gameManager.CheckUnlockLevel(levelToUnlock);

        audioSource.PlayOneShot(winSFX);
        GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioSource>().Stop();
    }

    void LevelTimer()
    {
        waitTimer += Time.deltaTime;
        if (waitTimer > 1 && (levelSeconds > 0 || levelMinutes >0))
        {
            levelSeconds--;
            if (levelSeconds <= 0)
            {
                if (levelMinutes > 0)
                {
                    levelMinutes--;
                    levelSeconds = 59;
                }
                else
                {
                    Die(deathTimerMessages[Random.Range(0, deathTimerMessages.Length)]);
                }
            }

            TimerTextUpdate();

            waitTimer = 0;
        }
    }

    void TimerTextUpdate()
    {
        string minutesString = levelMinutes.ToString();
        string secondsString = levelSeconds.ToString();

        if (levelMinutes < 10)
            minutesString = "0" + levelMinutes;
        if (levelSeconds < 10)
            secondsString = "0" + levelSeconds;

        timerText.text = minutesString + ":" + secondsString;
    }

    void SetLevelTimer()
    {
        if (levelEndSeconds >= 60)
        {
            levelMinutes = Mathf.RoundToInt(levelEndSeconds / 60);
            levelSeconds = Mathf.RoundToInt(levelEndSeconds - levelMinutes * 60);
        }
        else
        {
            levelSeconds = levelEndSeconds;
        }
    }

    public void ChangeScore(int amount)
    {
        score += amount;
        scoreText.text = "x" + score;
    }
}
