using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Globalization;
using System.Collections.Generic;


namespace Studio {

public static class Tools {

    // --- STRINGS & PARSING ---

    public static bool IsEmpty(string s){ return string.IsNullOrEmpty(s); }

    public static string[] Split(string s, string separator = "|"){ 
        if(IsEmpty(s)) return new string[0];
        return s.Split(new string[] { separator }, StringSplitOptions.None); 
    }

    //========================================================================
    // Universal array to string - works with any type
    public static string ArrayToString<T>(T[] arr, string separator = ", "){
        if(arr == null || arr.Length == 0) return "";
        StringBuilder sb = new StringBuilder();
        for(int i=0; i<arr.Length; i++){
            string val = arr[i]?.ToString() ?? "";
            if(arr[i] is float || arr[i] is double || arr[i] is decimal){ val = val.Replace(",", "."); }
            sb.Append(val);
            if(i < arr.Length - 1) sb.Append(separator);
        }
        return sb.ToString();
    }

    // Overload with coloring support for comparable types (numbers)
    public static string ArrayToString<T>(T[] arr, T colorGreater, T colorLess, string separator = ", ") where T : IComparable<T> {
        if(arr == null || arr.Length == 0) return "";
        StringBuilder sb = new StringBuilder();
        for(int i=0; i<arr.Length; i++){
            string val = arr[i]?.ToString() ?? "";
            if(arr[i] is float || arr[i] is double || arr[i] is decimal){ val = val.Replace(",", "."); }

            if(i == 0 && arr[i].CompareTo(colorGreater) > 0 && arr[i].CompareTo(colorLess) < 0){
                sb.Append("<color=#66EEFF>").Append(val).Append("</color>");
            } else {
                sb.Append(val);
            }
            if(i < arr.Length - 1) sb.Append(separator);
        }
        return sb.ToString();
    }

    // Universal list to string - works with any type
    public static string ListToString<T>(List<T> list, string separator = ", "){
        if(list == null || list.Count == 0) return "";
        StringBuilder sb = new StringBuilder();
        for(int i=0; i<list.Count; i++){
            string val = list[i]?.ToString() ?? "";
            if(list[i] is float || list[i] is double || list[i] is decimal){ val = val.Replace(",", "."); }
            sb.Append(val);
            if(i < list.Count - 1) sb.Append(separator);
        }
        return sb.ToString();
    }
    //========================================================================

    public static void ParseDialogue(string input, out string character, out string remarks, out string speech){
        character = ""; remarks = ""; speech = "";
        if(IsEmpty(input)) return;

        string[] lines = input.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        if(lines.Length == 0) return;

        character = lines[0].Trim();
        if(lines.Length < 2) return;

        int index = 1;
        List<string> remarkLines = new List<string>();

        // Collect remarks (lines in parentheses)
        while(index < lines.Length){
            string line = lines[index].Trim();
            if(line.StartsWith("(") && line.EndsWith(")")){
                remarkLines.Add(line);
                index++;
            } else {
                break; // Speech starts
            }
        }

        remarks = string.Join("\n", remarkLines);
        
        // Remaining lines are speech
        List<string> speechLines = new List<string>();
        while(index < lines.Length){
            speechLines.Add(lines[index].Trim());
            index++;
        }
        speech = string.Join("\n", speechLines);
    }

    // --- COLOR ---

    public static Color HexToColor(string hex){
        if(IsEmpty(hex)) return Color.white;
        hex = hex.Replace("#", "");
        
        byte r = byte.Parse(hex.Substring(0,2), NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2,2), NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4,2), NumberStyles.HexNumber);
        byte a = 255;
        if(hex.Length == 8){ a = byte.Parse(hex.Substring(6,2), NumberStyles.HexNumber); }
        
        return new Color32(r, g, b, a);
    }

    public static string ColorToHex(Color c){
        return ColorUtility.ToHtmlStringRGBA(c);
    }




    //-----------------------------------------------------------------------------
    public static string DuoNum(int num){
        string duo = num+"";  if(duo.Length < 2){ duo = "0"+duo; };  return duo;
    }

    //-----------------------------------------------------------------------------
    public static string LoadTextFile(string path){
        try {
            if (File.Exists(path)){
                return File.ReadAllText(path);
            } else {
                Debug.LogError("File not found: " + path);
                return string.Empty;
            }
        } catch (Exception e){
            Debug.LogError("Failed to read file: " + e.Message);
            return string.Empty;
        }

    }
    //-----------------------------------------------------------------------------
    public static string FormatSec(int sec){
        TimeSpan timeSpan = TimeSpan.FromSeconds(sec);
        return timeSpan.ToString(@"mm\:ss");
    }


    //-----------------------------------------------------------------------------
    public static string GetFileExtension(string filePathWithoutExtension){
        string[] possibleExtensions = { ".mp3", ".mp4", ".png" };

        foreach (string extension in possibleExtensions){
            string fullPath = filePathWithoutExtension + extension;
            if (File.Exists(fullPath)){
                return extension;
            }
        }

        return string.Empty;
    }


    //============================ IO & FILES ============================

    public static string GetTimestamp(){ return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); }
    public static string GetDatePath(){ return DateTime.Now.ToString("yyyy/MM/dd"); } // For Vault folders

    public static string SanitizeFileName(string name){
        foreach(char c in Path.GetInvalidFileNameChars()){ name = name.Replace(c, '_'); }
        return name;
    }

    public static void EnsureDirectory(string path){
        if(!Directory.Exists(path)) Directory.CreateDirectory(path);
    }

    // --- UNITY HELPERS ---

    public static void DestroyChildren(Transform t){
        foreach(Transform child in t){ UnityEngine.Object.Destroy(child.gameObject); }
    }

    public static void SetLayerRecursive(GameObject go, int layer){
        go.layer = layer;
        foreach(Transform child in go.transform){ SetLayerRecursive(child.gameObject, layer); }
    }

    public static void CopyToClipboard(string str){
        GUIUtility.systemCopyBuffer = str;
    }
}
}