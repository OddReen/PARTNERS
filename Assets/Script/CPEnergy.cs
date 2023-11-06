using System.Collections.Generic;
using UnityEngine;

public class CPEnergy : MonoBehaviour
{
    [SerializeField] float patternQuantity;
    [SerializeField] int targetArrowIndex;
    [SerializeField] string targetArrow;
    [SerializeField] Transform grid;
    [SerializeField] GameObject[] spritePrefs;
    [SerializeField] List<GameObject> sprites;
    [SerializeField] List<string> arrowOrder;
    [SerializeField] GameObject[] buttons;

    private void Start()
    {
        targetArrowIndex = 0;
        for (int i = 0; i < patternQuantity; i++)
        {
            GameObject gameObject = Instantiate(spritePrefs[Random.Range(0, spritePrefs.Length - 1)], grid);
            sprites.Add(gameObject);
            arrowOrder.Add(gameObject.name);
        }
        targetArrow = arrowOrder[0];
    }
    public void Fix(string button)
    {
        if (button == targetArrow)
        {
            targetArrow = arrowOrder[targetArrowIndex];
            targetArrowIndex++;
        }
        else
        {
            for (int i = 0; i < sprites.Count; i++)
            {
                Destroy(sprites[i]);
            }
            for (int i = 0; i < patternQuantity; i++)
            {
                GameObject gameObject = Instantiate(spritePrefs[Random.Range(0, spritePrefs.Length - 1)], grid);
                sprites.Add(gameObject);
                arrowOrder.Add(gameObject.name);
            }
            targetArrowIndex = 0;
            targetArrow = arrowOrder[targetArrowIndex];
        }
    }
    void Break()
    {
        //Diminuir percentagem de luminosidade
        //CameraConsole deixa de funcionar
        //A porta que fica no corredor ficará sempre fechada e não poderá ser aberta
    }
}
