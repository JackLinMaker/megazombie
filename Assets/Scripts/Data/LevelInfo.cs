using UnityEngine;
using System.Collections;

[System.Serializable]
public class LevelInfo 
{
    public enum ResultType
    {
        None,
        S,
        A,
        B,
        C,
    };

    public int MainLevel;
    public int SubLevel;
    public string Name;
    public bool IsComplete;
    public ResultType Result;

    
}
