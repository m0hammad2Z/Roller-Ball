using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSystem : MonoBehaviour
{
    public GameObject[] obstacles;
    public GameObject EndPointObject;
    public Transform spawnPoint;
    public int currentLevel;
    public List<Level> levels;

    float timeBetweenGenerate;
    int nOGObsicales;

    void Start()
    {
        currentLevel = LoadLevelNumber();
        timeBetweenGenerate = Random.Range(0.85f, 0.93f);
        InvokeRepeating("GenerateObstacle", 0.3f, timeBetweenGenerate);
    }

    public void ArrayDecleare()
    {
        for (int i = 0; i < levels.Count; i++)
        {
            levels[i].levelID = i + 1;
            levels[i].numberOfObstacls = Mathf.CeilToInt(((i + 1) * 0.75f) + 1);

            levels[i].obstaclsOrder.Clear();
            for (int j = 0; j <= levels[i].numberOfObstacls; j++)
                levels[i].obstaclsOrder.Add(Random.Range(0, obstacles.Length - 1));
        }
    }

    void GenerateObstacle()
    {
        if (GameManager.isCounting == false)
        {
            if (nOGObsicales < levels[currentLevel].numberOfObstacls)
            {
                Instantiate(obstacles[levels[currentLevel].obstaclsOrder[nOGObsicales]], spawnPoint.position,Quaternion.Euler(new Vector3(0,0, Random.Range(-180, 180))));
                nOGObsicales++;
            }
            else if (nOGObsicales == levels[currentLevel].numberOfObstacls)
            {
                Instantiate(EndPointObject, spawnPoint.position, Quaternion.identity);
                nOGObsicales++;
            }
        }
    }



    public void SaveLevelNumber()
    {
        PlayerPrefs.SetInt("levelnNumber", currentLevel);
    }

    public int LoadLevelNumber()
    {
        return PlayerPrefs.GetInt("levelnNumber");
    }

    //Other methods
    public void AddLevels(int numberOfLevels)
    {
        //for(int i = 0; i < numberOfLevels; i++)
        //{
        //    print(i);
        //    Level l = new Level();
        //    l.levelID = levels.Count + 1;
        //    l.numberOfObstacls = Mathf.CeilToInt(((levels.Count + 1) * 0.75f) + 1);
        //    levels.Add(l);
        //}

        for (int i = 0; i < numberOfLevels; i++)
        {
            Level l = new Level();
            l.levelID = levels.Count + 1;
            l.numberOfObstacls = Mathf.CeilToInt(((levels.Count + 1) * 0.75f) + 1);

            l.obstaclsOrder.Clear();
            for (int j = 0; j <= l.numberOfObstacls; j++)
                l.obstaclsOrder.Add(Random.Range(0, obstacles.Length - 1));

            levels.Add(l);

        }
    }

}

[System.Serializable]
public class Level
{
    public int levelID;
    public int numberOfObstacls;
    public List<int> obstaclsOrder = new List<int>();

}
