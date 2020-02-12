using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class FileFinder 
{
 //public static IEnumerable<string> Find(string path,bool recursive,params string[] extenstions)
 //   {
 //       var files = Directory.EnumerateFiles(path, "*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
 //       foreach(var file in files)
 //       {
 //           if (extenstions.Contains(Path.GetExtension(file), StringComparer.OrdinalIgnoreCase)
 //           {
 //               yield return file;
 //           }

                             
                
 //       }
 //   }
    public static string Find(string path,string fileName)
    {
        var files = Directory.GetFiles(path, fileName, SearchOption.AllDirectories);
        foreach(var x in files)
        {
           
                if (x.Split('\\').Last() == fileName)
                    try
                    {
                    return x; 
                    }
                    catch (Exception)
                    {
                        Debug.LogError("file : " + fileName + " Not Found");
                      
                        throw;
                    }

        


        }

        return null;
    }
}
