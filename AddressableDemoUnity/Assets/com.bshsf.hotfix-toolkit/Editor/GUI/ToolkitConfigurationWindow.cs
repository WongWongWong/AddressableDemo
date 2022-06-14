﻿namespace zFramework.Hotfix.Toolkit
{
    using UnityEditor;
    using UnityEditor.Presets;
    using UnityEngine;
    using static AssemblyHotfixManager;
    using static GlobalConfiguration;
    public class ToolkitConfigurationWindow : EditorWindow
    {
        private Editor editor;
        int selected = 0;
        GUIContent[] toolbarContents;
        [MenuItem(MenuNode)]
        static void OpenWindow()
        {
            var window = GetWindow<ToolkitConfigurationWindow>(ToolTitle, true);
            window.toolbarContents = new GUIContent[] { BT_LT, BT_RT };
            window.editor = Editor.CreateEditor(AssemblyHotfixManager.Instance);
            window.minSize = new Vector2(360, 300);
        }
        private void OnGUI()
        {
            GUILayout.Space(10);
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                var idx = EditorPrefs.GetInt("Hotfix Toolkit Tab Index", 0);
                selected = GUILayout.Toolbar(idx, toolbarContents, GUILayout.Height(EditorGUIUtility.singleLineHeight * 1.2f));

                if (selected != idx)
                {
                    idx = selected;
                    EditorPrefs.SetInt("Hotfix Toolkit Tab Index", idx);
                }
                GUILayout.FlexibleSpace();
            }
            if (selected == 0)
            {
                var rect_preset = GUILayoutUtility.GetLastRect();
                rect_preset.x = rect_preset.width - 15;
                rect_preset.width = 24;
                rect_preset.y += EditorGUIUtility.singleLineHeight * 1.2f / 4;
                var content = EditorGUIUtility.IconContent("Preset.Context");
                content.tooltip = "点击存储或加载 Preset .";
                if (GUI.Button(rect_preset, content))
                {
                    var target = AssemblyHotfixManager.Instance;
                    var receiver = CreateInstance<PresetReceiver>();
                    receiver.Init(target, this);
                    PresetSelector.ShowSelector(target, null, true, receiver);
                }
            }
            GUILayout.Space(15);
            using (new EditorGUILayout.VerticalScope())
            {
                if (selected == 0)
                {
                    Editor.CreateCachedEditor(AssemblyHotfixManager.Instance, typeof(AssemblyHotfixManagerEditor), ref editor);
                }
                else
                {
                    Editor.CreateCachedEditor(HotfixAssembliesData.Instance, typeof(HotfixAssembliesDataEditor), ref editor);
                }
                editor?.OnInspectorGUI();

                GUILayout.FlexibleSpace();
                using (var hr = new EditorGUILayout.HorizontalScope(GUI.skin.box))
                {
                    GUILayout.FlexibleSpace();
                    var color = GUI.color;
                    GUI.color = new Color32(127, 214, 253, 255);
                    var isbuild_bt = selected == 0;
                    if (GUILayout.Button(isbuild_bt ? build_content : sort_content, GUILayout.Height(36)))
                    {
                        bool state = false;
                        if (isbuild_bt)
                        {
                            state = ForceLoadAssemblies();
                        }
                        else
                        {
                            state = AssembliesBinaryHandler();
                        }
                        if (!state)
                        {
                            ShowNotification(op_fail_content, 2);
                        }
                    }
                    GUI.color = color;
                    GUILayout.FlexibleSpace();
                }
            }
        }
        static GUIContent BT_LT = new GUIContent("Editor", "编辑器下使用的配置");
        static GUIContent BT_RT = new GUIContent("Runtime", "运行时使用的配置");
        static GUIContent build_content = new GUIContent("Assembly Force Build ", "强制构建所有Player Assemblies！");
        static GUIContent sort_content = new GUIContent(" Sort Binaries Manually ", "重新载入并排序程序集转存文件");
        static GUIContent op_fail_content = new GUIContent("操作失败：Hotfix Toolkit 配置异常！");
    }
}