using UnityEngine;

public class CPSewer : MonoBehaviour
{
    public static CPSewer Instance;

    [SerializeField] GameObject pipe1, pipe2, pipe3;
    bool isFilled1 = false, isFilled2 = false, isFilled3 = false;
    private void Start()
    {
        FillPipe();
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
                isFilled3 ^= true;
                break;
            case "Button3":
                isFilled3 ^= true;
                break;
            default:
                break;
        }
        FillPipe();
        Fix();
    }
    public void FillPipe()
    {
        if (isFilled1)
            pipe1.transform.localScale = Vector3.one;
        else
            pipe1.transform.localScale = new Vector3(1, .1f, 1);

        if (isFilled2)
            pipe2.transform.localScale = Vector3.one;
        else
            pipe2.transform.localScale = new Vector3(1, .1f, 1);

        if (isFilled3)
            pipe3.transform.localScale = Vector3.one;
        else
            pipe3.transform.localScale = new Vector3(1, .1f, 1);
    }
    public void Fix()
    {
        if (isFilled1 && isFilled2 && isFilled3)
        {
            Manager_SFX.Instance.win.start();
            Debug.Log("Fixed");
        }
    }
}
