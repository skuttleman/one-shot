using System;
using UnityEngine;

public static class Logger
{
    public static T Spy<T>(T e)
    {
        Debug.Log(Format("[ SPY ]", e));
        return e;
    }

    public static void Info(params object[] objs) =>
        Debug.Log(Format("[INFO ]", objs));
    static string Format<T>(string identifier, T obj) =>
        identifier + " "  + obj;
}
