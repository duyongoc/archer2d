using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

namespace ViewGenerator
{

#if UNITY_EDITOR
    public static class Generation
    {

        private static string viewTemplate = "ViewClass.txt";
        private static string objectViewTemplate = "ObjectViewClass.txt";

        private const string noTemplateFound = "No template found!";
        private const string targetPath = "Tools/ViewGenerated/View";


        private static List<string> views = new List<string> {
        "MenuView",
        "InGameView",
        "GameOverView",
        "SettingView",
        "NoneView",
    };


        private static string pathTemplate = Path.Combine(Application.dataPath, "Tools/ViewGenerated/Templates/");


        [MenuItem("Auto/Generated View")]
        private static void GeneratedView()
        {
            CreateViewParent();
            CreateObjectView();

            AssetDatabase.Refresh();
        }


        private static void CreateViewParent()
        {
            // path
            string path = Path.Combine(Application.dataPath, targetPath, "View.cs");

            // create teamplate
            ScriptGenerator script = new ScriptGenerator();
            script.className = "View";
            script.textContent = GetTemplate(viewTemplate);

            // Debug.Log(script.textContent);
            using (var writer = new StreamWriter(path))
            {
                writer.Write(script.ToString());
                writer.Dispose();
            }
        }

        private static void CreateObjectView()
        {
            foreach (var viewName in views)
            {
                // path
                string path = Path.Combine(Application.dataPath, targetPath, viewName + ".cs");

                // create teamplate
                ScriptGenerator script = new ScriptGenerator();
                script.className = viewName;
                script.textContent = GetTemplate(objectViewTemplate);

                // Debug.Log(script.textContent);
                using (var writer = new StreamWriter(path))
                {
                    writer.Write(script.ToString());
                    writer.Dispose();
                }
            }
        }



        private static string GetTemplate(string name)
        {
            string path = Path.Combine(pathTemplate, name);
            if (File.Exists(path))
                return File.ReadAllText(path);

            return noTemplateFound;
        }


    }
#endif

}