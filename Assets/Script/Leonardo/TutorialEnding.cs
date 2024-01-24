using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnding : MonoBehaviour
{
    Transform spawnMelodyPosition;
    GameObject melodyModelPref;

    void Execute()
    {

    }
    IEnumerator EndingSequence()
    {
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        GameObject gameObject = Instantiate(melodyModelPref, spawnMelodyPosition.position, spawnMelodyPosition.rotation);

        yield return new WaitForSeconds(2);


    }
}
