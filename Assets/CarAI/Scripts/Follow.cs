using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    private CarAI[] cars;
    private List<Transform> dest = new();
    public Transform platform;

    // Start is called before the first frame update
    void Start()
    {
        cars = FindObjectsOfType<CarAI>();

        FillDest();
        StartCoroutine(Coutin());
    }

    private void FillDest()
    {
        Traverse(platform);
        Debug.Log("Baboooooo " + dest.Count);
    }

    private int iteration = 0;
    private void Traverse(Transform parent)
    {
        iteration++;
        if (iteration > 1000) Debug.LogError("Baboooooo");
        foreach (Transform child in parent)
        {
            if (child.name == "GameObject") dest.Add(child);
            if (child.childCount > 0) Traverse(child);
        }
    }

    IEnumerator Coutin()
    {
        while (true)
        {
            yield return null;
            foreach (var item in cars)
            {
                if(item.CustomDestination == null || Vector3.Distance(item.CustomDestination.position, item.transform.position) < 5)
                {
                    item.CustomDestination = dest[Random.Range(0, dest.Count - 1)];
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
