using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

namespace screencapture
{
    public class LanguageModel
    {

        public static List<ConfidenceScore> RunCmd(string cmd, string args, string workingDir)
        {
            List<ConfidenceScore> res = new List<ConfidenceScore>();
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = @"C:\Users\gerhas\AppData\Local\Programs\Python\Python37\python.exe";
            start.Arguments = string.Format("\"{0}\" \"{1}\"", cmd, args);
            start.UseShellExecute = false;// Do not use OS shell
            start.WorkingDirectory = workingDir;
            start.CreateNoWindow = true; // We don't need new window
            start.RedirectStandardOutput = true;// Any output, generated by application will be redirected back
            start.RedirectStandardError = true; // Any error in standard output will be redirected back (for example exceptions)
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string stderr = process.StandardError.ReadToEnd(); // Here are the exceptions from our Python script
                    while (!reader.EndOfStream)
                    {
                        string tag = reader.ReadLine();
                        string confidence = reader.ReadLine();
                        ConfidenceScore s = new  ConfidenceScore();
                        s.Hashtag = tag;
                        s.Confidence = float.Parse(confidence);
                        res.Add(s);
                    }
                    return res;
                }
            }
        }
    }
}