using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    [SerializeField] List<LevelCreateController> levels;
    [SerializeField] List<Transform> bottleCreate;
    [SerializeField] List<LevelCreateController> randomLevels;
    
    [SerializeField] LevelCreateController testLevel;
    [SerializeField] LevelCreateController activeLevel;
    
    [SerializeField] private TMP_Text levelText;
    [SerializeField] GameObject bottle;

    List<Color> colors, levelColors;
    
    public static int levelWinPoint = 0;
    int firstBlock, secondBlock;
    float bottleSpace;

    void Start()
    {
        int randomLevel = UnityEngine.Random.Range(0, randomLevels.Count);

        activeLevel = ActiveLevelController.activeLevel < 50 ? levels[ActiveLevelController.activeLevel] : randomLevels[randomLevel];

        bottleSpace = activeLevel.bottles.Count < 11 ? 0.5f : 0.4f;

        CreateColorByLevel(randomLevels[randomLevel]);

        if (activeLevel.bottles.Count > 5)
        {
            firstBlock = activeLevel.bottles.Count / 2 + activeLevel.bottles.Count % 2;
            secondBlock = activeLevel.bottles.Count / 2;
            CreateBlocks(firstBlock, 0);
            CreateBlocks(secondBlock, 1);
        }
        else
        {
            firstBlock = activeLevel.bottles.Count;

            CreateBlocks(firstBlock, 2);

        }
    }

    private void CreateColors(int color)
    {
        colors = new List<Color>();
        colors.Add(new Color(0.2235294f, 0.4862745f, 0.8666667f, 1));
        colors.Add(new Color(0.7830189f, 0.136659f, 0.136659f, 1));
        colors.Add(new Color(0.7568628f, 0.7450981f, 0.1921569f, 1));
        colors.Add(new Color(0.7960784f, 0.2156863f, 0.7803922f, 1));
        colors.Add(new Color(0.1680731f, 0.7843137f, 0.1372549f, 1));
        colors.Add(new Color(0.4243051f, 0.05606978f, 0.5660378f, 1));
        colors.Add(new Color(0.9058824f, 0.4105133f, 0.04085084f, 1));
        colors.Add(new Color(0.4278213f, 0.4303301f, 0.4339623f, 1));
        colors.Add(new Color(0.1222447f, 0.1981132f, 0.002803484f, 1));
        colors.Add(new Color(0.02069241f, 0.05299112f, 0.8773585f, 1));

    }

    private void CreateBlocks(int blockCount, int index)
    {

        int leftCount = 1, rightCount = 1;
        GameObject obj;
        for (int i = 0; i < blockCount; i++)
        {
            if (i == 0)
            {
                obj = Instantiate(bottle, bottleCreate[index].position, Quaternion.identity);
            }
            else if (i % 2 == 1)
            {
                obj = Instantiate(bottle, new Vector3(bottleCreate[index].position.x - leftCount * bottleSpace, bottleCreate[index].position.y, bottleCreate[index].position.z), Quaternion.identity);
                leftCount++;
            }
            else
            {
                obj = Instantiate(bottle, new Vector3(bottleCreate[index].position.x + rightCount * bottleSpace, bottleCreate[index].position.y, bottleCreate[index].position.z), Quaternion.identity);
                rightCount++;
            }

            int a = index >= 2 ? 0 : index;
            for (int j = 0; j < 4; j++)
            {
                obj.GetComponent<BottleController>().bottleColors[j] = activeLevel.bottles[i + a * firstBlock].colors[j];
            }
            obj.GetComponent<BottleController>().numberOfColorInBottle = activeLevel.bottles[i + a * firstBlock].numberBottle;
            obj.GetComponent<BottleController>().lineRenderer = GameObject.Find("LineRenderer").GetComponent<LineRenderer>();
            obj.transform.parent = gameObject.transform;

            obj = null;
        }

        levelWinPoint = activeLevel.winBottleCount;
        levelText.text = "LEVEL " + (ActiveLevelController.activeLevel + 1).ToString();
    }

    private void CreateColorByLevel(LevelCreateController lvl)
    {
        levelColors = new List<Color>();
        CreateColors(lvl.bottles.Count);
        for (int i = 0; i < lvl.bottles.Count - 2; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                levelColors.Add(colors[i]);
            }
        }

        for (int i = 0; i < lvl.bottles.Count - 2; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                int a = UnityEngine.Random.Range(0, levelColors.Count);
                lvl.bottles[i].colors[j] = levelColors[a];
                levelColors.RemoveAt(a);
            }
        }
    }
}
