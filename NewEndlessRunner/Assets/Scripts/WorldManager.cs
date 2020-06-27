using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldManager : MonoBehaviour
{

    public GameObject ground;

    GameObject obstaclePrefab;

    public GameObject[] obsPrefabs;

    public Transform spawnPoint1, spawnPoint2, spawnPoint3;

    Transform spawnPoint;

    public Transform belt;

    public float maxBeltSpeed;

    float beltSpeed;

    int spawnPointSelector = 1;

    int obsSelector = 0;

    Material groundMat;

    float curOffset = 0;
    public float worldScrollSpeed = 1f;
    public float groundScrollSpeed = 3f;

    public int lives = 3;
    public GameObject livesText;

    public static WorldManager instance;

    public GameObject overText;
    public GameObject restart;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
    }

    private void Start()
    {
        groundMat = ground.GetComponent<MeshRenderer>().material;
        InvokeRepeating("ObstacleGeneration", 0.5f, 2f);
    }

    void Update()
    {
        worldScrollSpeed += 0.001f;
        beltSpeed = (worldScrollSpeed / 100);
        beltSpeed = Mathf.Clamp(beltSpeed, 0, maxBeltSpeed);
        curOffset += (groundScrollSpeed / 1000);
        groundMat.mainTextureOffset = new Vector2(0, curOffset);
        belt.Translate(transform.forward * beltSpeed);
        livesText.GetComponent<Text>().text = "Lives: " + lives;
    }

    void ObstacleGeneration()
    {
        spawnPointSelector = Random.Range(1, 3);
        if (spawnPointSelector == 1)
        {
            spawnPoint = spawnPoint1;
        }
        else if (spawnPointSelector == 2)
        {
            spawnPoint = spawnPoint2;
        }
        else
        {
            spawnPoint = spawnPoint3;
        }

        obsSelector = Random.Range(0, obsPrefabs.Length);

        obstaclePrefab = obsPrefabs[obsSelector];

        if (obsSelector == 1)
        {
            obsSelector = Random.Range(0, 3);

            if (obsSelector == 1)
            {
                Instantiate(obstaclePrefab, spawnPoint.position, spawnPoint.rotation, belt);
            }
            else
            {
                Instantiate(obstaclePrefab, spawnPoint.position + Vector3.up, spawnPoint.rotation, belt);
            }

        }
        else
        {
            Instantiate(obstaclePrefab, spawnPoint.position, spawnPoint.rotation, belt);
        }

    }

    public void TakeDamage()
    {
        lives--;

        if (lives <= 0)
        {
            Destroy(gameObject);
            overText.SetActive(true);
            //restart.SetActive(true);
            lives = 0;
            livesText.GetComponent<Text>().text = "Lives: " + lives;
        }
    }

   
}
