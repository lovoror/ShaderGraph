using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class StringBuilderTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string str1 = "China";
        string str2 = "China";
        Debug.Log(str1 == str2);
        Debug.Log(str1.Equals(str2));
        Debug.Log(Object.ReferenceEquals(str1,str2));

        string s = str1 + str2;
        Debug.Log(s);

        Action a = printString;
        a();

        Func<string,int> b = test2;
        Console.WriteLine(b("xxx"));
    }
    static void printString()
        {
            Console.WriteLine("www");
        }

    static int test2(string str)
    {
        return 1;
    }
}
