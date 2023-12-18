using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject platformPrefab;

    private float minX = -2.5f;
    private float maxX = 2.5f;
    private float minY = -4.7f;
    private float maxY = -3.7f;

    private bool lerpCamera;
    private float lerpTime = 1.5f;
    private float lerpX;

    [Header("UI Components")]
    [SerializeField] private TMP_Text scoreText;

    
    public int score { get; private set; }
    public UnityEvent OnTextChange;

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
        CreateInitialPlatforms();
                
    }

    private void Update()
    {
        if (lerpCamera) {
            LerpTheCamera();
        }
    }

    private void CreateInitialPlatforms() {
        Vector3 temp = new Vector3(Random.Range(minX, minX + .2f), Random.Range(minY, maxY), 0);

        Instantiate(platformPrefab, temp, Quaternion.identity);

        temp.y += 4f;

        Instantiate(playerPrefab, temp, playerPrefab.transform.rotation);

        temp = new Vector3(Random.Range(maxX, maxX - .2f), Random.Range(minY, maxY), 0);

        Instantiate(platformPrefab, temp, Quaternion.identity);
    }

    private void CreateNewPlatform() {
        float camX = Camera.main.transform.position.x;
        float newMaxX = (maxX * 2) + camX;

        Instantiate(platformPrefab, new Vector3(Random.Range(newMaxX, newMaxX - .2f), Random.Range(maxY, maxY - .2f), 0), Quaternion.identity);

    }

    public void CreateNewPlatformAndLerp(float lerpPosition) {
        CreateNewPlatform();

        lerpX = lerpPosition + maxX;
        lerpCamera = true;
    }

    private void LerpTheCamera() {
        float x = Camera.main.transform.position.x;
        x = Mathf.Lerp(x, lerpX, lerpTime * Time.deltaTime);

        Camera.main.transform.position = new Vector3(x, Camera.main.transform.position.y, Camera.main.transform.position.z);

        if (Camera.main.transform.position.x >= (lerpX - 0.07f)) {
            lerpCamera = false;
        }
    }

    public void AddScore(int value) {
        score += value;

        scoreText.SetText(score.ToString());
        OnTextChange?.Invoke();
    }

    
}
