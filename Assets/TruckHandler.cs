using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckHandler : MonoBehaviour
{
    private PathFollower[] allTrucks; 
    // Start is called before the first frame update
    void Start()
    {
        allTrucks = FindObjectsOfType<PathFollower>();

        foreach (var item in allTrucks)
        {
            item.gameObject.SetActive(false);
        }

        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        while (true)
        {
            yield return null;

            int activeT = 0;
            foreach (var item in allTrucks)
            {
                if (item.gameObject.activeInHierarchy) 
                {
                    activeT += 1;
                }
            }

            if(activeT < GameStore.Instance.mainGameData.trucks)
            {
                int delta = GameStore.Instance.mainGameData.trucks - activeT;
                for (int i = 0; i < delta; i++)
                {
                    foreach (var item in allTrucks)
                    {
                        if(!item.gameObject.activeInHierarchy && !item.gameOver && activeT != GameStore.Instance.mainGameData.trucks)
                        {
                            if (item.AssignedDebrees.localScale.sqrMagnitude >= 0.001f)
                            {
                                item.gameObject.SetActive(true);
                                activeT++;
                            }
                        }
                    }
                }
            }
        }
    }
}
