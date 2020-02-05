using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.Text.RegularExpressions;

public class ComponentManager : EditorWindow
{
    public static Component[] components;    
    
    // % = Ctrl 
    // # = Shift
    // & = Alt
    // _r would be just r
    [MenuItem ("Window/Component Manager/Manager %#r")]
    public static void  ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ComponentManager));
        components = new Component[0];
        GetGameObjectComponents();
    }
    
    void OnEnable()
    {
        Selection.selectionChanged += SelectionChanged;
        Undo.undoRedoPerformed += UndoRedo;
    }
    
    void OnDisable()
    {
        Selection.selectionChanged -= SelectionChanged;
        Undo.undoRedoPerformed -= UndoRedo;
    }
    
    void SelectionChanged()
    {
        GetGameObjectComponents();
        Repaint();
    }
    
    void UndoRedo()
    {
        GetGameObjectComponents();
        Repaint();
    }
    
    void OnGUI()
    {
        GameObject activeGO = Selection.activeGameObject;
        if(activeGO != null)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label (activeGO.name);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
        // Start at 1 because component 0 is the transform
        if(components.Length > 1 && components != null)
        {
            for(int i = 1; i < components.Length; i++)
            {
                if(components[i] != null)
                {
                    EditorGUILayout.BeginHorizontal();
                    string componentName = components[i].ToString();
                    componentName = Regex.Replace(componentName, "(UnityEngine.)", "");
                    componentName = Regex.Replace(componentName, "[()]", "");
                    string objectName = "(" + activeGO.name + ")";
                    componentName = Regex.Replace(componentName, objectName, "");
                    if(GUILayout.Button("Remove " + componentName))
                    {
                        // Undo.RecordObject(Selection.activeGameObject, "Remove " + componentName);
                        Undo.DestroyObjectImmediate(components[i]);
                        // DestroyImmediate(components[i]);
                        // EditorUtility.SetDirty(Selection.activeGameObject);
                        GetGameObjectComponents();
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
        }
        else
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("No components found or no object selected");
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
    }
    
    public static void GetGameObjectComponents()
    {
        GameObject go = Selection.activeGameObject;
        if(go != null)
            components = go.GetComponents<Component> ();
        else
            components = new Component[0];
    }
}