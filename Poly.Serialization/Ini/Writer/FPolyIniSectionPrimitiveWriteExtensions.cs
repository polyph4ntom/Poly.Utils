using System;

namespace Poly.Serialization
{
	public static class FPolyIniSectionPrimitiveWriteExtensions
	{
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

        public static void AddInt32(this FPolyIniSection section, string key, int value, int lineNumber = 0)
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

        public static void AddInt32ArrayValue(this FPolyIniSection section, string key, int value, int lineNumber = 0)
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
	}
}


