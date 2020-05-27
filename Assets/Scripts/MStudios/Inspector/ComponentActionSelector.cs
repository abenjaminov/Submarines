using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MStudios.Inspector
{
    [Serializable]
    public class ComponentActionSelector
    {
        [SerializeField] private GameObject _owner;
        [SerializeField] private int _selectedComponentIndex;
        public string componentName;
        [SerializeField] private int _selectedMethodIndex;
        public string methodName;
        [SerializeField] private int _selectedTypeIndex;
        [SerializeField] private string _typeName;
        public bool forceParameterType;
    }
}