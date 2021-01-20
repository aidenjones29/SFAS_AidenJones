using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnBags : MonoBehaviour
{
    [SerializeField] private GameObject Rubbish;

    [SerializeField] public float AreaScale;
    [SerializeField] private int RubbishAmount;

    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(Random.Range(0, 999));
        
        for (int i = 0; i < RubbishAmount; i++)
        {
            GameObject rubbishSpawn = Instantiate(Rubbish, gameObject.transform) as GameObject;
            rubbishSpawn.name = rubbishSpawn.name.Replace("(Clone)", "");
            //GameObject rubbishSpawn = Instantiate(Rubbish, gameObject.transform);
            moveObject(AreaScale, rubbishSpawn);
        }
    }

    public void moveObject(float areaScale, GameObject rubbish)
    {
        float randomX = Random.Range(-areaScale, areaScale);
        float randomZ = Random.Range(-areaScale, areaScale);
        Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        rubbish.transform.localPosition = new Vector3(randomX, 0.2f, randomZ);
        rubbish.transform.rotation = rotation;
    }
}