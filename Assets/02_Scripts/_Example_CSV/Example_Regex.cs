using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ExampleRegex : MonoBehaviour
{
    static string patternSlang = "(개새|씨발|니애|느그애|느금)";
    public List<string> slangs;

    Regex regex = new Regex(@"^01[01678]-[0-9]{4}-[0-9]{4}$");

    void Start()
    {
        for (int i = 0; i < slangs.Count; i++)
        {
            string result = Regex.Replace(slangs[i], patternSlang, "**");
            Debug.Log(result);
        }

        regex = new Regex(@"^[가-힣]{3}$");    //한글 3글자
    }
}