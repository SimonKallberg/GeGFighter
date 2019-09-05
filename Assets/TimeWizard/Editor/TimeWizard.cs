
using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.Collections.Generic;

namespace GreenerGames
{
    public class TimeWizard : EditorWindow
    {
        private static readonly GUILayoutOption buttonWidth = GUILayout.Width(40.0f);
        private static readonly GUILayoutOption buttonHeight = GUILayout.Height(20.0f);

        private const string pauseIconBase64 = "iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAASUlEQVR42mNgGAUUgFdA/B+KfwFxMZIciP0bSf4lORb8R8MNSHINWORHLRi1YNSCUQtGLRieFrxEq3CKkOSKoGIw+eej9TPZAAB+3KDPBzVBmQAAAABJRU5ErkJggg==";
        private const string playIconBase64 = "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAABQElEQVRYR+3XS0rEQBRG4a+Xobtx4D50Ab6XID5Q0IGrEFEH6kARXIUTQUFRdAM+EZQLHWi0jUlXpTPpmqaSc+px/6p0tNw6LfONBMpmYB+TWMUWPppYrjKBrx7gFWZwlluiqkDBPcQCbnOJ1BUI7ivWsIH3VJFBBArmNeZxnCKRIlBwTzCHEKrdcggENJZis1sxsUSVWy6BAhibcxEHVQ1yCxTcKNco2yjf0taUQEAjuLaxjOe/LJoUKJj3mMZ5P4lhCAT3CWNtCjxivC2BO0zhYtgCRTZEbL8MexNWTsfcm/Cmez4c/Vf/xfNcAhG/690T8q0qPPrlEEi6I6QIRMzO4rTOiH/2HUQgYjXiNWI2+Z5YV2AXS3hIGXXvu1UFLrunW98wSZEpE9jDBFawg88U0CBB1ATv1zdHv2atz8A3jE1MIcHbbjMAAAAASUVORK5CYII=";

        private class TimeKeeperButton
        {
            public TimeWizard timeKeeper;

            public bool selected = false;

            public float timeValue;

            public GUIContent content;

            public TimeKeeperButton(TimeWizard owner, float timeValue, string title)
            {
                this.timeKeeper = owner;
                this.timeValue = timeValue;
                content = new GUIContent(title);
            }

            public TimeKeeperButton(TimeWizard owner, float timeValue, Texture2D image)
            {
                this.timeKeeper = owner;
                this.timeValue = timeValue;
                content = new GUIContent(image);
            }

            public void Draw()
            {
                Color defaultColor = GUI.backgroundColor;
                if (selected)
                {
                    GUI.backgroundColor = Color.gray;
                }

                if (GUILayout.Button(content, buttonWidth, buttonHeight))
                {
                    timeKeeper.SelectTimeButton(this);
                }

                GUI.backgroundColor = defaultColor;
            }
        }

        List<TimeKeeperButton> activeButtons = new List<TimeKeeperButton>();

        Texture2D pauseIcon, playIcon;

        float orginalDeltaTime;
        float timeWizardTime;

        bool newPlayMode;

        [MenuItem("Window/Time Wizard")]
        public static void ShowWindow()
        {
            var win = EditorWindow.GetWindow<TimeWizard>();
            win.minSize = new Vector2(30, 30);
        }

        public TimeWizard()
        {
            EditorApplication.playmodeStateChanged += OnStateChanged;
            titleContent = new GUIContent("Time Wizard");
            minSize = new Vector2(100, 30);
        }

        private void OnStateChanged()
        {
            bool tempPlayMode = newPlayMode;
            newPlayMode = EditorApplication.isPlaying;

            if (newPlayMode && !tempPlayMode)
            {
                orginalDeltaTime = Time.timeScale;
                timeWizardTime = Time.time;
            }
            else if (!newPlayMode && tempPlayMode)
            {
                Time.timeScale = orginalDeltaTime;
                timeWizardTime = Time.timeScale;

                for (int i = 0; i < activeButtons.Count; ++i)
                {
                    activeButtons[i].selected = false;
                }
            }
        }

        void OnEnable()
        {
            pauseIcon = CreateTexture(pauseIconBase64);
            playIcon = CreateTexture(playIconBase64);

            activeButtons = new List<TimeKeeperButton>()
        {
            new TimeKeeperButton(this,0,pauseIcon),
            new TimeKeeperButton(this,0.1f,"x⅒"),
            new TimeKeeperButton(this,0.25f,"x¼"),
            new TimeKeeperButton(this,0.5f,"x½"),
            new TimeKeeperButton(this,0.75f,"x¾"),
            new TimeKeeperButton(this,1,playIcon),
            new TimeKeeperButton(this,2f,"x2"),
            new TimeKeeperButton(this,4f,"x4"),
            new TimeKeeperButton(this,8f,"x8"),
        };
        }

        void OnGUI()
        {
            GUILayout.BeginHorizontal();
            {
                if (Application.isPlaying)
                {
                    GUILayout.Label((Time.timeScale == timeWizardTime ? "(TimeWizard)" : "(Custom Set)") + " Current Time Scale : " + (Time.timeScale == 0 ? "(Paused)" : timeWizardTime.ToString()), GUILayout.Width(400));
                    GUI.enabled = true;
                }
                else
                {
                    GUILayout.Label("Time Wizard, Press Play to wake up", GUILayout.Width(400));
                    GUI.enabled = false;
                }

                for (int i = 0; i < activeButtons.Count; ++i)
                {
                    activeButtons[i].Draw();
                }

                GUILayout.BeginVertical();
                GUILayout.FlexibleSpace();
                EdPopup.Popup("Custom Values", OpenCustomPopup, GUILayout.Width(150));
                GUILayout.FlexibleSpace();
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();

            GUI.enabled = true;
        }


        private void SelectTimeButton(TimeKeeperButton timeKeeperButton)
        {
            for (int i = 0; i < activeButtons.Count; ++i)
            {
                if (activeButtons[i] == timeKeeperButton)
                {
                    activeButtons[i].selected = true;
                }
                else
                {
                    activeButtons[i].selected = false;
                }
            }

            Time.timeScale = timeKeeperButton.timeValue;
            timeWizardTime = timeKeeperButton.timeValue;
        }

        void OpenCustomPopup()
        {
            //EditorPrefs.DeleteKey("TimeWizardCustoms");
            GenericMenu menu = new GenericMenu();

            string customs = EditorPrefs.GetString("TimeWizardCustoms", "");

            if (!string.IsNullOrEmpty(customs))
            {
                string[] c = customs.Split(","[0]);

                for (int i = 0; i < c.Length; i++)
                {
                    if (!string.IsNullOrEmpty(c[i]))
                    {
                        string[] values = c[i].Split("/"[0]);
                        menu.AddItem(new GUIContent(values[0]), false, SelectCustomValue, values[1]);
                    }
                }
            }
            menu.DropDown(EdPopup.ButtonRect);
        }

        void SelectCustomValue(object timeValue)
        {
            for (int i = 0; i < activeButtons.Count; ++i)
            {
                activeButtons[i].selected = false;
            }

            Time.timeScale = float.Parse(timeValue.ToString());
            timeWizardTime = float.Parse(timeValue.ToString());
        }

        private static Texture2D CreateTexture(string base64)
        {
            byte[] data_Data = Convert.FromBase64String(base64);
            var tex = new Texture2D(1, 1);
            tex.hideFlags = HideFlags.HideAndDontSave;
            tex.LoadImage(data_Data);
            return tex;
        }
    }

    public class TimeWizardCustomValue
    {
        public string name;
        public string speed;
    }
}
