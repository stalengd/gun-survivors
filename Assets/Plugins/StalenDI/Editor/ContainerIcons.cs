 using UnityEditor;
 using UnityEngine;
 using System.Collections.Generic;

namespace Stalen.DI
{
    [InitializeOnLoad]
    public class ContainerIcons
    {
        public static Texture2D MainIcon { get; private set; }

        static ContainerIcons()
        {
            MainIcon = EditorGUIUtility.Load("Assets/Plugins/StalenDI/Editor/Resources/ServicesContainer.psd") as Texture2D;
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyItemCB;
        }

        private static void HierarchyItemCB(int instanceID, Rect selectionRect)
        {
            if (MainIcon == null)
            {
                return;
            }

            var r = new Rect(selectionRect)
            {
                width = 20,
                height = 20,
            };
            r.x -= 2;
            r.y -= 2;

            GameObject go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (go != null && go.GetComponent<MonoContainer>())
            {
                var style = GUI.skin.button;
                style.padding = new RectOffset(2, 2, 2, 2);
                if (GUI.Button(r, MainIcon, style))
                    ContainersWindow.ShowWindow();
            }
        }
    }
}