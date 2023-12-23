using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPGas : MonoBehaviour
{
    public static CPGas Instance;

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
        Instance = this;
    }
    public void ColorMiniGame(string name)
    {
        if (isShowingPattern)
            return;
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
        isShowingPattern = true;
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
        isShowingPattern = false;
    }
    void RestartPatternMinigame()
    {
        targetColorString = "";
        colorPattern.Clear();
        targetColorIndex = 0;
    }
    void Fix()
    {
        Manager_SFX.Instance.win.start();
        Debug.Log("Fixed");
    }
    void Correct()
    {
        Debug.Log("Correct");
    }
    void Wrong()
    {
        Manager_SFX.Instance.lose.start();
        Debug.Log("Wrong");
    }
    void Break()
    {

    }
}
