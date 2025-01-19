namespace FoxCultGames.Editor
{
    using UnityEditor;
    using UnityEngine;
    using UnityToolbarExtender;
    using Utilities;

    [InitializeOnLoad]
    public sealed class RemoveSaveFileButton
    {
        static RemoveSaveFileButton()
        {
            ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUI);
        }

        private static void OnToolbarGUI()
        {
            GUILayout.FlexibleSpace();
            
            if (GameSaveHelper.DoesSaveExist && GUILayout.Button("Remove Save", EditorStyles.toolbarButton))
            {
                GameSaveHelper.RemoveSaveFile();
            }
        }
    }
}