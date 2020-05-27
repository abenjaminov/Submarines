using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MStudios.Inspector
{
    #if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ComponentActionSelector))]
    public class ComponentSelectorDrawerUIE  : PropertyDrawer
    {
        private Component[] allComponents;
        private string[] allComponentNames;
        private int size = 0;
        
        private TypeReference[] types = {new TypeReference()
        {
            type = typeof(float),
            displayName = "Float"
        }, new TypeReference()
        {
            type = typeof(Vector3),
            displayName = "Vector3"
        },
        new TypeReference() {
            type = typeof(string),
            displayName = "String"
        }};
        
        private int selectedType = 0;
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) + size;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            int height = 30;
            int heightIncrement = 25;
            EditorGUI.BeginProperty(position, label, property);
            
            var labelRect = new Rect(position.x, position.y,position.width, 35);

            EditorGUI.LabelField(labelRect,"Component Action Selector");
            
            var ownerProperty = property.FindPropertyRelative("_owner");
            var selectComponentProperty = property.FindPropertyRelative("_selectedComponentIndex");
            var selectMethodProperty = property.FindPropertyRelative("_selectedMethodIndex");
            var selectTypeProperty = property.FindPropertyRelative("_selectedTypeIndex");
            var typeProperty = property.FindPropertyRelative("_typeName");
            var forceTyping = property.FindPropertyRelative("forceParameterType");
            
            var methodNameProperty = property.FindPropertyRelative("methodName");
            var componentNameProperty = property.FindPropertyRelative("componentName");

            methodNameProperty.stringValue = "";
            componentNameProperty.stringValue = "";
            
            var ownerRect = new Rect(position.x, position.y+ height,position.width, 20);
            height += heightIncrement;
            ownerProperty.objectReferenceValue = (GameObject)EditorGUI.ObjectField(ownerRect, "Component Owner",ownerProperty.objectReferenceValue, typeof(GameObject),true);

            var forceTypeRect = new Rect(position.x, position.y+ height,position.width, 20);
            height += heightIncrement;
            forceTyping.boolValue = EditorGUI.Toggle(forceTypeRect,"Force Type", forceTyping.boolValue);
            
            if (forceTyping.boolValue)
            {
                var typeRect = new Rect(position.x, position.y+ height,position.width, 20);
                height += heightIncrement;
                selectTypeProperty.intValue = EditorGUI.Popup(typeRect, "Attribute Type", selectTypeProperty.intValue, types.Select(x => x.displayName).ToArray());    
            }
            
            var type = types[selectTypeProperty.intValue].type;
            typeProperty.stringValue = type.ToString();
            
            if (ownerProperty.objectReferenceValue)
            {
                 var gameObject = (GameObject)ownerProperty.objectReferenceValue;
                
                 if (gameObject == null) return;
                
                 if (allComponents == null)
                 {
                     allComponents = gameObject.GetComponents(typeof(Component));
                     allComponentNames = allComponents.Select(x => x.GetType().Name).ToArray();
                 }
                
                 var componentRect = new Rect(position.x, position.y+ height,position.width, 20);
                 height += heightIncrement;
                 selectComponentProperty.intValue = EditorGUI.Popup(componentRect,"Component", selectComponentProperty.intValue, allComponentNames);

                 componentNameProperty.stringValue = allComponentNames[selectComponentProperty.intValue];
                 
                 var selectedComponent = allComponents[selectComponentProperty.intValue];
                 var componentMethods = GetRelevantMethods(selectedComponent, forceTyping.boolValue ? type : null);
                 var componentMethodNames = componentMethods.Select(x => x.Name).ToArray();

                 var actionRect = new Rect(position.x, position.y+ height,position.width, 20);
                 height += heightIncrement;
                 selectMethodProperty.intValue = EditorGUI.Popup(actionRect,"Action", selectMethodProperty.intValue, componentMethodNames);

                 if (selectMethodProperty.intValue < componentMethodNames.Length)
                 {
                    methodNameProperty.stringValue = componentMethodNames[selectMethodProperty.intValue];
                 }
            }
            
            property.serializedObject.ApplyModifiedProperties();
            EditorGUI.EndProperty();
            size = height;
        }

        private static IEnumerable<MethodInfo> GetRelevantMethods(Component selectedComponent, Type parameterType)
        {
            var methodsQuery = selectedComponent.GetType().GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance).AsEnumerable();
            
            if (parameterType != null)
            {
                methodsQuery = methodsQuery.Where(x => x.GetParameters().Length == 1 && x.GetParameters()[0].ParameterType == parameterType);    
            }
            else
            {
                methodsQuery = methodsQuery.Where(x => x.GetParameters().Length == 0);    
            }
            
            return methodsQuery;
        }

        private class TypeReference
        {
            public Type type;
            public string displayName;
        }
    }
    #endif
}