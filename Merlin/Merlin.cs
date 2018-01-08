using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Merlin
{
    static class Merlin
    {
        public static List<MerlinMod> Mods = new List<MerlinMod>();

        public static void LoadMods()
        {
            Log("Merlin Initialized.", true);

            var listOfAllDlls = GetModFilesOf(GetModsPath());

            foreach(var modfile in listOfAllDlls)
            {
                Log("Loading Mod File: " + modfile, false);
                LoadModFile(modfile);
            }
        }



        private static List<string> GetModFilesOf(string path)
        {
            var list = new List<string>();

            //all Files in Directory

            foreach (var file in Directory.GetFiles(path))
            {
                if (file.EndsWith(".dll"))
                {
                    try
                    {
                        Log("Mod File Found: " + file, false);
                        list.Add(file);
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e);
                    }
                }

            }

            //all Directories in Directory

            foreach (var dir in Directory.GetDirectories(path))
            {
                try
                {
                    //recursion
                    foreach(var file in GetModFilesOf(dir))
                    {
                        list.Add(file);
                    }
                }
                catch(Exception e)
                {
                    Debug.Log(e);
                }
            }

            return list;
        }

        private static void Log(string logLine, bool clearFile)
        {

            string logPath = Path.Combine(GetModsPath(), "Merlin-log.txt");
            try
            {
                if (clearFile)
                {
                    StreamWriter deleter = new StreamWriter(logPath);
                    deleter.Close();
                }
                StreamWriter writer = new StreamWriter(logPath, true);
                writer.WriteLine("[" + System.DateTime.Now + "] " + logLine);
                writer.Close();
            } catch(Exception e)
            {
                Debug.Log(e);
            }
        }

        private static string GetModsPath()
        {
            var executingDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var rootDir = Directory.GetParent(executingDir).Parent;

            var modDir = Path.Combine(rootDir.FullName, "Mods");

            if (!Directory.Exists(modDir))
                Directory.CreateDirectory(modDir);

            return modDir;
        }

        private static void LoadModFile(string path)
        {
                var assembly = Assembly.LoadFrom(path);
            try
            {
                foreach (var type in FindModTypes(assembly))
                {
                    try
                    {
                        var mod = Activator.CreateInstance(type) as MerlinMod;
                        mod.OnLoad();
                        Mods.Add(mod);
                        Log("Mod File loaded: " + path, false);
                    }
                    catch (Exception e)
                    {
                        Log("error", false);
                    }
                }
            }catch(Exception e)
            {
                Log("Err_Message: " + e.Message, false);
                Log("Err_Source: " + e.Source, false);
                Log("Err_Stacktrace: " + e.StackTrace, false);
            }
        }

        private static IEnumerable<Type> FindModTypes(Assembly assembly)
        {
            return assembly.GetTypes().Where(t => typeof(MerlinMod).IsAssignableFrom(t));
        }

        public static void Dispatch(Action<MerlinMod> action)
        {
            Mods.ForEach(action);
        }

        public static object GetField<T>(string field, object instance = null)
        {
            return typeof(T).GetField(field, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).GetValue(instance);
        }

        public static void SetField<T>(object value, string field, object instance = null)
        {
            typeof(T).GetField(field, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).SetValue(instance, value);
        }
    }
}
