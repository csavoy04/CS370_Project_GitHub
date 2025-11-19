using UnityEngine;
using System.Collections.Generic;

/*  Script made by: Coleman
        Script that:
            - Checks the symbols to see if they
                are in the proper order
    Date: 11/18/2025
    Made in: C# VsCode
*/


public class SymbolSolveCheck : MonoBehaviour
{
    [Header("GameObjects")]
    // Getting game objects
    public GameObject S1;
    public GameObject S2;
    public GameObject S3;
    public GameObject S4;
    public GameObject S5;
    public GameObject S6;
    // Getting scripts
    public OnClick script1;
    public OnClick script2;
    public OnClick script3;
    public OnClick script4;
    public OnClick script5;
    public OnClick script6;

    [Header("Variables")]
    public List<int> key = new List<int>();
    public List<int> symbol = new List<int>();
    public bool equal = true;

    void Awake() 
    {
        // Getting scripts from symbols
        script1 = S1.GetComponent<OnClick>();
        script2 = S2.GetComponent<OnClick>();
        script3 = S3.GetComponent<OnClick>();
        script4 = S4.GetComponent<OnClick>();
        script5 = S5.GetComponent<OnClick>();
        script6 = S6.GetComponent<OnClick>();
    }

    private bool DoListsMatch(List<int> key, List<int> symbol)
    {
        bool equal = true;
        for (var i = 0; i < key.Count; i++)
        {
            if (key[i] != symbol[i])
            {
                equal = false;
            }
        }
        return equal;
    }

    void Update() 
    {
        // Getting variable i from each symbols OnClick script
        symbol = new List<int> {script1.i, script2.i, script3.i, script4.i, script5.i, script6.i};

        if (DoListsMatch(key, symbol))
        {
            Debug.Log("Code Correct");
            transform.position += new Vector3(0f, -1f, 0f) * Time.deltaTime;
        }
    }
}
