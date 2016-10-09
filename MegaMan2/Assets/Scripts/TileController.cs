using UnityEngine;
using System.Collections;

public class TileController : MonoBehaviour
{
    private GameObject k_Player;
    private GameObject[] k_Tiles;
    private GameObject[] k_BackgroundTiles;
    private GameObject[] k_Enemies;
    private float[] k_Distance;
    private float[] k_DistanceBack;
    private float[] k_EnemyDistance;


    void Start()
    {
        k_Player = GameObject.Find("Mega_Man");
        k_Tiles = GameObject.FindGameObjectsWithTag("Obstacle");
        k_BackgroundTiles = GameObject.FindGameObjectsWithTag("Background");
        k_Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        k_Distance = new float[k_Tiles.Length];
        k_DistanceBack = new float[k_BackgroundTiles.Length];
        k_EnemyDistance = new float[k_Enemies.Length];
    }

    void Update()
    {
        for (int i = 0; i < k_Tiles.Length; i++)
        {
            k_Distance[i] = Vector3.Distance(k_Player.transform.position, k_Tiles[i].transform.position);

            if (k_Distance[i] >= 15 && k_Tiles[i].activeSelf == true)
            {
                k_Tiles[i].SetActive(false);
            }

            if (k_Distance[i] <= 15 && k_Tiles[i].activeSelf == false)
            {
                k_Tiles[i].SetActive(true);
            }
        }

        for (int i = 0; i < k_BackgroundTiles.Length; i++)
        {
            k_DistanceBack[i] = Vector3.Distance(k_Player.transform.position, k_BackgroundTiles[i].transform.position);

            if (k_DistanceBack[i] >= 15 && k_BackgroundTiles[i].activeSelf == true)
            {
                k_BackgroundTiles[i].SetActive(false);
            }

            if (k_DistanceBack[i] <= 15 && k_BackgroundTiles[i].activeSelf == false)
            {
                k_BackgroundTiles[i].SetActive(true);
            }
        }

        for (int i = 0; i < k_Enemies.Length; i++)
        {
            if (k_Enemies[i] != null)
            {
                k_EnemyDistance[i] = Vector3.Distance(k_Player.transform.position, k_Enemies[i].transform.position);

                if (k_EnemyDistance[i] >= 15 && k_Enemies[i].activeSelf == true)
                {
                    k_Enemies[i].SetActive(false);
                }

                if (k_EnemyDistance[i] <= 15 && k_Enemies[i].activeSelf == false)
                {
                    k_Enemies[i].SetActive(true);
                }
            }
        }
    }
}
