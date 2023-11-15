using System.Collections.Generic;
using UnityEngine;

public class CPEnergy : MonoBehaviour
{
    public static CPEnergy Instance;

    [SerializeField] int patternQuantity;
    [SerializeField] int targetArrowIndex;
    [SerializeField] string targetArrowString;
    [SerializeField] Transform grid;
    [SerializeField] GameObject[] arrowPrefs;
    [SerializeField] List<GameObject> arrowPattern;

    private void Start()
    {
        Instance = this;
        //Instantiate Pattern
        targetArrowIndex = 0;
        for (int i = 0; i < patternQuantity; i++)
        {
            int randomNum = Random.Range(0, arrowPrefs.Length - 1);
            GameObject gameObject = Instantiate(arrowPrefs[randomNum], grid);
            gameObject.name = arrowPrefs[randomNum].name;

            arrowPattern.Add(gameObject);
        }
        targetArrowString = arrowPattern[0].name;
    }
    public void PatternMiniGame(string button)
    {
        bool correctButton = button == targetArrowString;
        if (correctButton)
        {
            Correct();
            targetArrowIndex++;
            if (targetArrowIndex == patternQuantity)
            {
                Fix();
                RestartPatternMinigame();
                return;
            }
            targetArrowString = arrowPattern[targetArrowIndex].name;
        }
        else
        {
            Wrong();
            RestartPatternMinigame();
        }
    }
    void Fix()
    {
        Debug.Log("Fixed");
    }
    void Correct()
    {
        Debug.Log("Correct");
    }
    void Wrong()
    {
        Debug.Log("Wrong");
    }
    void RestartPatternMinigame()
    {
        for (int i = 0; i < arrowPattern.Count; i++)
        {
            Destroy(arrowPattern[i]);
        }
        arrowPattern.Clear();
        for (int i = 0; i < patternQuantity; i++)
        {
            int randomNum = Random.Range(0, arrowPrefs.Length - 1);
            GameObject gameObject = Instantiate(arrowPrefs[randomNum], grid);
            gameObject.name = arrowPrefs[randomNum].name;

            arrowPattern.Add(gameObject);
        }
        targetArrowIndex = 0;
        targetArrowString = arrowPattern[targetArrowIndex].name;
    }
    void Break()
    {
        //Diminuir percentagem de luminosidade
        //CameraConsole deixa de funcionar
        //A porta que fica no corredor ficará sempre fechada e não poderá ser aberta
    }

}
