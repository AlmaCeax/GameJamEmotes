using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CereButtonsPuzzle : MonoBehaviour
{

    private GameObject button1;
    private GameObject button2;
    public GameObject obstacle1;
    public GameObject obstacle2;
    private int count1;
    private int count2;
    public int total_count;

    // Start is called before the first frame update
    void Start()
    {
        count1 = 0;
        count2 = 0;
        total_count = 0;
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void CheckUpdatePressed(GameObject button)
    {
        if (!button1)
            button1 = button;
        else if (!button2)
            button2 = button;

        if (button1 == button)
        {
            count1++;
            total_count++;
        }
            
        else if (button2 == button)
        {
            count2++;
            total_count++;
        }


        if (count1 < count2 || count1 > count2 + 1)
            ResetAll();
    }

    void ResetAll()
    {
        count1 = 0;
        count2 = 0;
        total_count = 0;
        button1 = null;
        button2 = null;
        obstacle1.transform.localScale = new Vector3(obstacle1.transform.localScale.x, 1, obstacle1.transform.localScale.z);
        obstacle2.transform.localScale = new Vector3(obstacle2.transform.localScale.x, 1, obstacle2.transform.localScale.z);
    }


    
}

