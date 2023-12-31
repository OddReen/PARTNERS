using System.Collections;
using UnityEngine;

public class CPSewer : MonoBehaviour
{
    public static CPSewer Instance;

    [SerializeField] bool isFilled1 = false, isFilled2 = false, isFilled3 = false, isFilled4 = false;

    [SerializeField] GameObject pipe1, pipe2, pipe3, pipe4;

    private void Start()
    {
        Instance = this;
    }
    public void FillMinigame(string buttonString)
    {
        switch (buttonString)
        {
            case "Button1":
                isFilled1 ^= true;
                isFilled2 ^= true;
                break;
            case "Button2":
                isFilled2 ^= true;
                isFilled4 ^= true;
                break;
            case "Button3":
                isFilled1 ^= true;
                break;
            case "Button4":
                isFilled1 ^= true;
                isFilled2 ^= true;
                isFilled3 ^= true;
                break;
        }
        FillPipe();
        Fix();
    }
    public void FillPipe()
    {
        if (isFilled1)
            pipe1.GetComponent<Animator>().SetTrigger("Fill");
        else
            pipe1.GetComponent<Animator>().SetTrigger("Empty");

        if (isFilled2)
            pipe2.GetComponent<Animator>().SetTrigger("Fill");
        else
            pipe2.GetComponent<Animator>().SetTrigger("Empty");

        if (isFilled3)
            pipe3.GetComponent<Animator>().SetTrigger("Fill");
        else
            pipe3.GetComponent<Animator>().SetTrigger("Empty");

        if (isFilled4)
            pipe4.GetComponent<Animator>().SetTrigger("Fill");
        else
            pipe4.GetComponent<Animator>().SetTrigger("Empty");

        StartCoroutine(ResetTrigger(pipe1.GetComponent<Animator>()));
        StartCoroutine(ResetTrigger(pipe2.GetComponent<Animator>()));
        StartCoroutine(ResetTrigger(pipe3.GetComponent<Animator>()));
        StartCoroutine(ResetTrigger(pipe4.GetComponent<Animator>()));
    }
    IEnumerator ResetTrigger(Animator _animator)
    {
        yield return new WaitForEndOfFrame();
        _animator.ResetTrigger("Fill");
        _animator.ResetTrigger("Empty");
    }
    public void Fix()
    {
        if (isFilled1 && isFilled2 && isFilled3 && isFilled4)
        {
            //Manager_SFX.Instance.win.start();
            Debug.Log("Fixed");
        }
    }
}
