using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;

namespace screencapture
{

    public class OcrHelperWindows
    {
        public static string GetFullText(string pathToImg)
        {
            //execute powershell cmdlets or scripts using command arguments as process
            ProcessStartInfo processInfo = new ProcessStartInfo();
            processInfo.FileName = @"powershell.exe";
            //execute powershell script using script file
            processInfo.Arguments = @"& { C:\Users\gerhas\Documents\GitHub\screencapture\Powershell\ocrtest2.ps1 " + pathToImg + "}";
            //execute powershell command
            
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;
            processInfo.UseShellExecute = false;
            processInfo.CreateNoWindow = true;

            //start powershell process using process start info
            Process process = new Process();
            process.StartInfo = processInfo;
            process.Start();

            string result = process.StandardOutput.ReadToEnd();

            Root2 myDeserializedClass = JsonConvert.DeserializeObject<Root2>(result); 
            return myDeserializedClass.Text;

            
            

        }
        
    }
}