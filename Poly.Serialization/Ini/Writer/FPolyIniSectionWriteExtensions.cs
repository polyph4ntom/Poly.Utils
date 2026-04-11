using System;
using UnityEngine;

namespace Poly.Serialization
{
	public static class FPolyIniSectionWriteExtensions
	{
        //=======================
        // SCALAR
        //=======================
		 public static void AddString(this FPolyIniSection section, string key, string value, int lineNumber = 0)
        {
            if (section == null)
            {
                throw new ArgumentNullException(nameof(section));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            section.AddEntry(EPolyIniAssignmentKind.Set, key, FPolyIniValueSerializers.String.Format(value), lineNumber);
        }

        public static void AddBoolean(this FPolyIniSection section, string key, bool value, int lineNumber = 0)
        {
            if (section == null)
            {
                throw new ArgumentNullException(nameof(section));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            section.AddEntry(EPolyIniAssignmentKind.Set, key, FPolyIniValueSerializers.Boolean.Format(value), lineNumber);
        }

        public static void AddInt(this FPolyIniSection section, string key, int value, int lineNumber = 0)
        {
            if (section == null)
            {
                throw new ArgumentNullException(nameof(section));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            section.AddEntry(EPolyIniAssignmentKind.Set, key, FPolyIniValueSerializers.Integer.Format(value), lineNumber);
        }

        public static void AddSingle(this FPolyIniSection section, string key, float value, int lineNumber = 0)
        {
            if (section == null)
            {
                throw new ArgumentNullException(nameof(section));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            section.AddEntry(EPolyIniAssignmentKind.Set, key, FPolyIniValueSerializers.Single.Format(value), lineNumber);
        }
        
        public static void AddGuid(this FPolyIniSection section, string key, Guid value, int lineNumber = 0)
        {
            if (section == null)
            {
                throw new ArgumentNullException(nameof(section));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            
            section.AddEntry(EPolyIniAssignmentKind.Set, key, FPolyIniValueSerializers.Guid.Format(value), lineNumber);
        }

        public static void AddVector2(this FPolyIniSection section, string key, Vector2 value, int lineNumber = 0)
        {
            if (section == null)
            {
                throw new ArgumentNullException(nameof(section));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            
            section.AddEntry(EPolyIniAssignmentKind.Set, key, FPolyIniValueSerializers.Vector2.Format(value), lineNumber);
        }

        public static void AddVector3(this FPolyIniSection section, string key, Vector3 value, int lineNumber = 0)
        {
            if (section == null)
            {
                throw new ArgumentNullException(nameof(section));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            
            section.AddEntry(EPolyIniAssignmentKind.Set, key, FPolyIniValueSerializers.Vector3.Format(value), lineNumber);
        }

        public static void AddColor(this FPolyIniSection section, string key, Color value, int lineNumber = 0)
        {
            if (section == null)
            {
                throw new ArgumentNullException(nameof(section));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            
            section.AddEntry(EPolyIniAssignmentKind.Set, key, FPolyIniValueSerializers.Color.Format(value), lineNumber);
        }

        public static void AddQuaternion(this FPolyIniSection section, string key, Quaternion value, int lineNumber = 0)
        {
            if (section == null)
            {
                throw new ArgumentNullException(nameof(section));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            
            section.AddEntry(EPolyIniAssignmentKind.Set, key, FPolyIniValueSerializers.Quaternion.Format(value), lineNumber);
        }
        
        //=======================
        // ARRAY
        //=======================

        public static void AddStringArrayValue(this FPolyIniSection section, string key, string value, int lineNumber = 0)
        {
            if (section == null)
            {
                throw new ArgumentNullException(nameof(section));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            section.AddEntry(EPolyIniAssignmentKind.Add, key, FPolyIniValueSerializers.String.Format(value), lineNumber);
        }

        public static void AddBooleanArrayValue(this FPolyIniSection section, string key, bool value, int lineNumber = 0)
        {
            if (section == null)
            {
                throw new ArgumentNullException(nameof(section));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            section.AddEntry(EPolyIniAssignmentKind.Add, key, FPolyIniValueSerializers.Boolean.Format(value), lineNumber);
        }

        public static void AddIntArrayValue(this FPolyIniSection section, string key, int value, int lineNumber = 0)
        {
            if (section == null)
            {
                throw new ArgumentNullException(nameof(section));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            section.AddEntry(EPolyIniAssignmentKind.Add, key, FPolyIniValueSerializers.Integer.Format(value), lineNumber);
        }

        public static void AddSingleArrayValue(this FPolyIniSection section, string key, float value, int lineNumber = 0)
        {
            if (section == null)
            {
                throw new ArgumentNullException(nameof(section));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            section.AddEntry(EPolyIniAssignmentKind.Add, key, FPolyIniValueSerializers.Single.Format(value), lineNumber);
        }
        
        public static void AddVector2ArrayValue(this FPolyIniSection section, string key, Vector2 value, int lineNumber = 0)
        {
            if (section == null)
            {
                throw new ArgumentNullException(nameof(section));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            section.AddEntry(EPolyIniAssignmentKind.Add, key, FPolyIniValueSerializers.Vector2.Format(value), lineNumber);
        }
        
        public static void AddVector3ArrayValue(this FPolyIniSection section, string key, Vector3 value, int lineNumber = 0)
        {
            if (section == null)
            {
                throw new ArgumentNullException(nameof(section));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            section.AddEntry(EPolyIniAssignmentKind.Add, key, FPolyIniValueSerializers.Vector3.Format(value), lineNumber);
        }
        
        public static void AddColorArrayValue(this FPolyIniSection section, string key, Color value, int lineNumber = 0)
        {
            if (section == null)
            {
                throw new ArgumentNullException(nameof(section));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            section.AddEntry(EPolyIniAssignmentKind.Add, key, FPolyIniValueSerializers.Color.Format(value), lineNumber);
        }
        
        public static void AddQuaternionArrayValue(this FPolyIniSection section, string key, Quaternion value, int lineNumber = 0)
        {
            if (section == null)
            {
                throw new ArgumentNullException(nameof(section));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            section.AddEntry(EPolyIniAssignmentKind.Add, key, FPolyIniValueSerializers.Quaternion.Format(value), lineNumber);
        }
	}
}


