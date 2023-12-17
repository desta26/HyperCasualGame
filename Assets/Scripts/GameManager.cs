using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject platform;
    [SerializeField] private CinemachineVirtualCamera cam;

    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

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

        //cam.LookAt = player.transform;
        //cam.Follow = player.transform;
    }

    private void CreateInitialPlatforms() {
        Vector3 temp = new Vector3(Random.Range(minX, minX + .2f), Random.Range(minY, maxY), 0);

        Instantiate(platform, temp, Quaternion.identity);

        //temp.y += 3f;

        //Instantiate(player, temp, player.transform.rotation);

        temp = new Vector3(Random.Range(maxX, maxX - .2f), Random.Range(minY, maxY), 0);

        Instantiate(platform, temp, Quaternion.identity);
    }
}
