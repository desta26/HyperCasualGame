using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Prefabs")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject[] platformPrefabs;
    [SerializeField] private GameObject flyingEnemy;

    [Header("Object")]
    public GameObject player;
    [SerializeField] private Transform deadZone;

    #region Platform Settings
    private float minX = -2.5f;
    private float maxX = 2.5f;
    private float minY = -4.7f;
    private float maxY = -3.7f;
    #endregion

    private bool lerping;
    private float lerpTime = 1.5f;
    private float lerpX;

    [Header("UI Components")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private TMP_Text bestResultText;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Audio Components")]
    [SerializeField] private AudioSource audioSource;

    #region Score
    private int score;
    private int bestScore;
    #endregion

    [Header("Events")]
    public UnityEvent OnAddScore;
    public UnityEvent OnGameOver;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeScore();

        CreateInitialPlatforms();


    }

    private void InitializeScore()
    {
        score = 0;
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
    }

    private void Update()
    {
        if (lerping) {
            LerpTheCamera();
            LerpTheDeadZone();
        }
    }

    private void CreateInitialPlatforms() {
        Vector3 temp = new Vector3(Random.Range(minX, minX + .2f), Random.Range(minY, maxY), 0);

        Instantiate(platformPrefabs[Random.Range(0,platformPrefabs.Length)], temp, Quaternion.identity);

        temp.y += 4f;

        player = Instantiate(playerPrefab, temp, playerPrefab.transform.rotation);

        temp = new Vector3(Random.Range(maxX, maxX - .2f), Random.Range(minY, maxY), 0);

        Instantiate(platformPrefabs[Random.Range(0, platformPrefabs.Length)], temp, Quaternion.identity);
    }

    private void CreateNewPlatform() {
        float camX = Camera.main.transform.position.x;
        float newMaxX = (maxX * 3) + camX;

        Instantiate(platformPrefabs[Random.Range(0, platformPrefabs.Length)], new Vector3(Random.Range(newMaxX, newMaxX - .2f), Random.Range(maxY, maxY - 1.5f), 0), Quaternion.identity);

    }

    public void CreateNewPlatformAndLerp(float lerpPosition) {
        CreateNewPlatform();

        lerpX = lerpPosition + maxX;
        lerping = true;
    }

    private void LerpTheCamera() {
        float x = Camera.main.transform.position.x;
        x = Mathf.Lerp(x, lerpX, lerpTime * Time.deltaTime);

        Camera.main.transform.position = new Vector3(x, Camera.main.transform.position.y, Camera.main.transform.position.z);

        if (Camera.main.transform.position.x >= (lerpX - 0.07f)) {
            lerping = false;
        }
    }

    private void LerpTheDeadZone() {
        float x = deadZone.position.x;
        x = Mathf.Lerp(x, lerpX, lerpTime * Time.deltaTime);

        deadZone.position = new Vector3(x, deadZone.position.y, deadZone.position.z);

        if (deadZone.position.x >= (lerpX - 0.07f))
        {
            lerping = false;
        }
    }

    public void AddScore(int value) {
        score += value;

        scoreText.SetText(score.ToString());

        OnAddScore?.Invoke();
    }

    public void GameOver() {

        if (score >= bestScore)
        {
            PlayerPrefs.SetInt("BestScore", score);
        }

        resultText.text = $"Score : {score}";
        bestResultText.text = $"Best Score : {PlayerPrefs.GetInt("BestScore")}";

    
        gameOverPanel.SetActive(true);
        OnGameOver?.Invoke();
    }

    public void Retry()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void EventSound(AudioClip clip)
    {
        audioSource.clip = null;
        audioSource.clip = clip;
        audioSource.Play();
    }
}
