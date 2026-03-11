// ConsoleHotkeys.csC:\Feeble snow\Assets\Serebrennikov\Assembly editor helper\ConsoleHotkeys.csConsoleHotkeys.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
public static class ConsoleMi {
    [MenuItem("Serebrennikov/Clear Console")]
    public static void ClearConsole() {
        Type logEntries = Type.GetType("UnityEditor.LogEntries, UnityEditor.dll");
        MethodInfo clearMethod = logEntries.GetMethod("Clear", BindingFlags.Static | BindingFlags.Public);
        clearMethod.Invoke(null, null);
    }
}
