using System;
using System.Reflection;

namespace Poly.Events
{
    internal class FPolyWeakDelegate
    {
        public WeakReference Target { get; private set; }
        public MethodInfo Method { get; private set; }
        public Type DelegateType { get; private set; }

        public FPolyWeakDelegate(Delegate del)
        {
            Target = new WeakReference(del.Target);
            Method = del.Method;
            DelegateType = del.GetType();
        }

        public bool IsDead => Target.Target == null;

        public Delegate Rebuild()
        {
            var target = Target.Target;
            if (target == null)
                return null;

            return Delegate.CreateDelegate(DelegateType, target, Method);
        }

        public bool Matches(Delegate other)
        {
            return Target.Target == other.Target && Method == other.Method;
        }
    }
}
