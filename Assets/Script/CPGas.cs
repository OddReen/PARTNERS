using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPGas : MonoBehaviour
{
    public EventReference Win;
    EventInstance win;

    public EventReference Lose;
    EventInstance lose;

    [SerializeField] bool isShowingPattern = false;
    [SerializeField] float timeBetweenColors;
    [SerializeField] float timeColorOnScreen;
    [SerializeField] int patternQuantity;
    [SerializeField] int targetColorIndex;
    [SerializeField] string targetColorString;
    [SerializeField] GameObject[] colors;
    [SerializeField] List<GameObject> colorPattern;
    [SerializeField] GameObject showPatternButton;

    private void Start()
    {
        lose = RuntimeManager.CreateInstance(Lose);
        lose.start();
        win = RuntimeManager.CreateInstance(Win);
        win.start();
    }
    private void Update()
    {
        lose.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
        win.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
    }
    public void ColorMiniGame(string name)
    {
        if (name == targetColorString)
        {
            Correct();
            targetColorIndex++;
            if (targetColorIndex == patternQuantity)
            {
                Fix();
                RestartPatternMinigame();
                return;
            }
            targetColorString = colorPattern[targetColorIndex].name;
        }
        else if (name == showPatternButton.name && !isShowingPattern)
        {
            StartCoroutine(ShowPattern());
        }
        else if (name == showPatternButton.name && isShowingPattern)
        {
            return;
        }
        else
        {
            Wrong();
            RestartPatternMinigame();
        }
    }
    IEnumerator ShowPattern()
    {
        RestartPatternMinigame();
        for (int i = 0; i < patternQuantity; i++)
        {
            int randomIndex = Random.Range(0, colors.Length);
            colorPattern.Add(colors[randomIndex]);
        }
        for (int i = 0; i < colorPattern.Count; i++)
        {
            colorPattern[i].SetActive(true);
            yield return new WaitForSeconds(timeColorOnScreen);
            colorPattern[i].SetActive(false);
            yield return new WaitForSeconds(timeBetweenColors);
        }
        targetColorString = colorPattern[0].name;
        targetColorString = colorPattern[targetColorIndex].name;
    }
    void RestartPatternMinigame()
    {
        targetColorString = "";
        isShowingPattern = true;
        colorPattern.Clear();
        isShowingPattern = false;
        targetColorIndex = 0;
    }
    void Fix()
    {
        win.start();
        Debug.Log("Fixed");
    }
    void Correct()
    {
        Debug.Log("Correct");
    }
    void Wrong()
    {
        lose.start();
        Debug.Log("Wrong");
    }
    void Break()
    {

    }
}
