using System;

namespace Poly.Serialization
{
    public sealed class FPolyTypeIniValueSerializer : IPolyIniValueSerializer<Type>
    {
        public static readonly FPolyTypeIniValueSerializer @default = new(new FPolyTypeIniValueSerializerOptions());

        private readonly FPolyTypeIniValueSerializerOptions options;
        
        public FPolyTypeIniValueSerializer(FPolyTypeIniValueSerializerOptions options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }
        
        public bool TryParse(ReadOnlySpan<char> text, out Type value)
        {
            var tokenText = FPolyIniTextUtility.TrimSpacesAndTabs(text).ToString();
            if (tokenText.Length == 0)
            {
                value = null!;
                return false;
            }

            if (options.Resolver != null)
            {
                var resolved = options.Resolver(tokenText);
                if (resolved != null)
                {
                    value = resolved;
                    return true;
                }
            }

            if (options.UseTypeGetTypeFallback)
            {
                var resolved = Type.GetType(tokenText, throwOnError: false);
                if (resolved != null)
                {
                    value = resolved;
                    return true;
                }
            }

            value = null!;
            return false;
        }
        
        public string Format(Type value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (options.UseAssemblyQualifiedName)
            {
                return value.AssemblyQualifiedName ?? value.FullName ?? value.Name;
            }

            return value.FullName ?? value.Name;
        }
    }
}