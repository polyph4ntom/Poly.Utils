using System;
using UnityEngine;

namespace Poly.Common
{
    
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class FPolySubclassPickerAttribute : PropertyAttribute
    {
        public Type BaseType { get; }

        public FPolySubclassPickerAttribute(Type baseType)
        {
            BaseType = baseType;
        }
    }
}