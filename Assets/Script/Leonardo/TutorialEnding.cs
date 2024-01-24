using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnding : MonoBehaviour
{
    [SerializeField] Transform spawnMelodyPosition;
    [SerializeField] GameObject melodyModelPref;

    public void Execute()
    {
        StartCoroutine(EndingSequence());
    }
    IEnumerator EndingSequence()
    {
        //Restrict Players
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < player.Length; i++)
        {
            Behaviour[] behaviour = player[i].GetComponentsInChildren<Behaviour>();
            for (int d = 0; d < behaviour.Length; d++)
            {
                behaviour[d].enabled = false;
            }
        }

        //Instantiate Melody
        GameObject newMelody = Instantiate(melodyModelPref, spawnMelodyPosition.position, spawnMelodyPosition.rotation);

        //Rotate player cameras to melody
        for (int i = 0; i < player.Length; i++)
        {
            player[i].transform.LookAt(newMelody.transform, Vector3.up);
        }
        yield return new WaitForSeconds(5);

    }
}
