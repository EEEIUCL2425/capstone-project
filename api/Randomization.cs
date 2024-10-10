using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject ButterCake;
    public GameObject ChocoChips;
    public GameObject ChocoCrunchies;

    float endX1 = -2;
    float endX2 = 2;
    float endZ1 = -2;
    float endZ2 = 2;
    public void SpawnObject(GameObject[] Item, int[] Count, int Seed)
    {
        /*
        SpawnObject(GameObject Item, int Count, int Seed)
            Spawns a Count amount of Item in a random Cartesian coordinate with random rotation. Randomness is defined by Seed.
            The Cartesian coordinate is bounded from (endX1, endZ1) to (endX2, endZ2)
        */
        Random.InitState(Seed);
        for(int j = 0; j < Item.Length; j++)
        {
            for(int i = 0; i < Count[j]; i++)
            {
                float spawnPointX = Random.Range(endX1, endX2);
                float spawnPointY = Random.Range(3, 5);
                float spawnPointZ = Random.Range(endZ1, endZ2);

                float rotX = Random.Range(0, 360);
                float rotY = Random.Range(0, 360);
                float rotZ = Random.Range(0, 360);

                Vector3 spawnPosition = new Vector3(spawnPointX, spawnPointY, spawnPointZ);
                Vector3 spawnRotation = new Vector3(rotX, rotY, rotZ);
                
                Instantiate(Item[j], spawnPosition, Quaternion.Euler(spawnRotation));
            }
        }
    }

    void Start()
    {
        GameObject[] Items = {ButterCake, ChocoChips, ChocoCrunchies};
        int[] Counts = {30, 10, 20};

        SpawnObject(Items, Counts, 40);
    }
}