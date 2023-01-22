using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input : MonoBehaviour
{
    [SerializeField] private GameObject Square;

    private bool isKeyDown = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();

        if (isKeyDown)
        {
            Square.transform.localScale = new Vector3(Square.transform.localScale.x,
                        Square.transform.localScale.y + 0.001f, Square.transform.localScale.z);
        }
    }

    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isKeyDown = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            isKeyDown = false;
        }
    }
}
