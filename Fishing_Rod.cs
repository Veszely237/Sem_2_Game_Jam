using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class Fishing_Rod : MonoBehaviour
{
    [SerializeField] private GameObject LakeLayer;
    [SerializeField] private ParticleSystem Splash_Effect;

    private float randomTimer;
    private float timePassed = 0.0f;
    private float timeSubmerged = 0.0f;
    private int timeToCatch = 2;
    private bool isSubmerged = false;

    // Start is called before the first frame update
    void Start()
    {
        Splash_Effect.enableEmission = false;
        randomTimer = Random.Range(3f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;

        if (timePassed >= randomTimer)
        {
            SubmergeBobber();
            isSubmerged = true;
        }


        if (isSubmerged)
        {
            if (timeSubmerged < timeToCatch)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.Log("Success!");
                    SubmergeBobber();
                }
                timeSubmerged += Time.deltaTime;
            }
            else
            {
                Debug.Log("Failed!");
            }
        }

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    SubmergeBobber();
        //}
        //else if (Input.GetKeyDown(KeyCode.T))
        //{
        //    CheckTime();
        //}
    }

    // Submerge the bobber.
    void SubmergeBobber()
    {
        if (!LakeLayer.active)
        {
            LakeLayer.SetActive(true);
            Splash_Effect.enableEmission = true;

        }
        else if (LakeLayer.active)
        {
            LakeLayer.SetActive(false);
            Splash_Effect.enableEmission = false;
            timeSubmerged = 0.0f;
        }
    }

    void UndoSubmergeBobber()
    {

    }

    void CheckTime()
    {
        Debug.Log(timePassed);
        Debug.Log(randomTimer);
    }
}
