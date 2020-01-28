using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperFunctions
{
    public static void Log(params object[] texts)
    {
        string output = "";
        foreach (var text in texts)
        {
            output += text.ToString().PadRight(8) + "  ";
        }
        Debug.Log(output);
    }
}
