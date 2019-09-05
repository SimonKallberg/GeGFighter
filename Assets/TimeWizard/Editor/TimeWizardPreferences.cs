using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GreenerGames
{
    public class TimeWizardPreferences : MonoBehaviour
    {
        // Have we loaded the prefs yet
        private static bool prefsLoaded = false;

        public static List<TimeWizardCustomValue> customValues = new List<TimeWizardCustomValue>();

        [PreferenceItem("Time Wizard")]
        public static void PreferencesGUI()
        {
            // Load the preferences
           if (!prefsLoaded)
           {
                customValues.Clear();

                string stringValues = EditorPrefs.GetString("TimeWizardCustoms", "");
                if (!string.IsNullOrEmpty(stringValues))
                {
                    string[] c = stringValues.Split(","[0]);

                    for (int i = 0; i < c.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(c[i]))
                        {
                            string[] values = c[i].Split("/"[0]);
                            TimeWizardCustomValue tw = new TimeWizardCustomValue()
                            {
                                name = values[0],
                                speed = values[1]
                            };

                            customValues.Add(tw);
                        }
                    }
                }

                prefsLoaded = true;
            }

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(Resources.Load("Time-Wizard-Small") as Texture2D);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Name", GUILayout.Width(100));
            GUILayout.Label("Speed Value");
            GUILayout.EndHorizontal();

            foreach (TimeWizardCustomValue cv in customValues.ToList())
            {
                GUILayout.BeginHorizontal();
                cv.name = GUILayout.TextField(cv.name, GUILayout.Width(100));
                cv.speed = EditorGUILayout.FloatField(float.Parse(cv.speed)).ToString();
                if(GUILayout.Button("X", GUILayout.Width(50)))
                {
                    customValues.Remove(cv);
                }
                GUILayout.EndHorizontal();
            }

            if(GUILayout.Button("Add New Custom Value"))
            {
                TimeWizardCustomValue tw = new TimeWizardCustomValue();
                tw.name = "New Value";
                tw.speed = "1";
                customValues.Add(tw);

            }

            // Save the preferences
            if (GUI.changed)
            {
                string saveFile = "";
                for (int i = 0; i < customValues.Count; i++)
                {
                    saveFile += customValues[i].name + "/" + customValues[i].speed.ToString() + ",";
                }
                EditorPrefs.SetString("TimeWizardCustoms", saveFile);
            }
        }
    }
}
