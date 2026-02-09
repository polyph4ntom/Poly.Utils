using System;
using System.Collections.Generic;
using Poly.Common;

namespace Poly.Events
{
    public class FPolyAction
    {
        private readonly List<Listener> listeners = new();
        private bool isDirty = false;
        
        private class Listener
        {
            public FPolyWeakDelegate Delegate { get; private set; }
            public int Priority { get; private set; }
            public bool Once { get; private set; }
            public object Target { get; private set; }

            public Listener(object target, Delegate del, int priority = 0, bool once = false)
            {
                Target = target;
                Delegate = new FPolyWeakDelegate(del);
                Priority = priority;
                Once = once;
            }
            
            public bool IsDead => Delegate.IsDead;
            public Delegate Rebuild() => Delegate.Rebuild();
            public bool Matches(Delegate other) => Delegate.Matches(other);
        }

        public void Add(object target, Action callback, int priority = 0, bool once = false)
        {
            listeners.Add(new Listener(target, callback, priority, once));
            isDirty = true;
        }

        public void AddUnique(object target, Action callback, int priority = 0, bool once = false)
        {
            if (Contains(callback))
            {
                return;
            }
            
            Add(target, callback, priority, once);
        }

        public void Remove(object target, Action callback)
        {
            listeners.RemoveAll(l => l.Target == target && l.Matches(callback));
        }

        public void RemoveAll()
        {
            listeners.Clear();
        }

        public void RemoveAll(object target)
        {
            listeners.RemoveAll(l => l.Target == target);
        }

        public void Broadcast()
        {
            if (isDirty)
            {
                listeners.Sort((a, b) => b.Priority.CompareTo(a.Priority));
                isDirty = false;
            }

            for (int i = 0; i < listeners.Count; i++)
            {
                var listener = listeners[i];

                if (listener.IsDead)
                {
                    listeners.RemoveAt(i);
                    continue;
                }

                var del = (Action)listener.Rebuild();
                try
                {
                    del?.Invoke();
                }
                catch (Exception e)
                {
                    FPolyLog.Error("Poly.Events",$"FPolyAction Exception: {e}");
                }

                if (listener.Once)
                {
                    listeners.RemoveAt(i);
                }
            }
        }
        
        private bool Contains(Action callback)
        {
            foreach (var l in listeners)
            {
                if (l.Matches(callback))
                {
                    return true;
                }
            }
            return false;
        }
    }
    
    public class FPolyAction<T>
    {
        private readonly List<Listener> listeners = new();
        private bool isDirty = false;

        private class Listener
        {
            public FPolyWeakDelegate Delegate { get; private set; }
            public int Priority { get; private set; }
            public bool Once { get; private set; }
            public object Target { get; private set; }

            public Listener(object target, Delegate del, int priority = 0, bool once = false)
            {
                Target = target;
                Delegate = new FPolyWeakDelegate(del);
                Priority = priority;
                Once = once;
            }
            
            public bool IsDead => Delegate.IsDead;
            public Delegate Rebuild() => Delegate.Rebuild();
            public bool Matches(Delegate other) => Delegate.Matches(other);
        }

        public void Add(object target, Action<T> callback, int priority = 0, bool once = false)
        {
            listeners.Add(new Listener(target, callback, priority, once));
            isDirty = true;
        }

        public void AddUnique(object target, Action<T> callback, int priority = 0, bool once = false)
        {
            if (Contains(callback))
            {
                return;
            }

            Add(target, callback, priority, once);
        }

        public void Remove(object target, Action<T> callback)
        {
            listeners.RemoveAll(l => l.Target == target && l.Matches(callback));
        }

        public void RemoveAll()
        {
            listeners.Clear();
        }

        public void RemoveAll(object target)
        {
            listeners.RemoveAll(l => l.Target == target);
        }

        public void Broadcast(T arg0)
        {
            if (isDirty)
            {
                listeners.Sort((a, b) => b.Priority.CompareTo(a.Priority));
                isDirty = false;
            }

            for (int i = 0; i < listeners.Count; i++)
            {
                var listener = listeners[i];

                if (listener.IsDead)
                {
                    listeners.RemoveAt(i);
                    continue;
                }

                var del = (Action<T>)listener.Rebuild();
                try
                {
                    del?.Invoke(arg0);
                }
                catch (Exception e)
                {
                    FPolyLog.Error("Poly.Events",$"FPolyAction<{typeof(T)}> Exception: {e}");
                }

                if (listener.Once)
                {
                    listeners.RemoveAt(i);
                }
            }
        }
        
        private bool Contains(Action<T> callback)
        {
            foreach (var l in listeners)
            {
                if (l.Matches(callback))
                {
                    return true;
                }

            }
            return false;
        }
    }
    
    public class FPolyAction <T1, T2>
    {
        private readonly List<Listener> listeners = new();
        private bool isDirty = false;
        
        private class Listener
        {
            public FPolyWeakDelegate Delegate { get; private set; }
            public int Priority { get; private set; }
            public bool Once { get; private set; }
            public object Target { get; private set; }

            public Listener(object target, Delegate del, int priority = 0, bool once = false)
            {
                Target = target;
                Delegate = new FPolyWeakDelegate(del);
                Priority = priority;
                Once = once;
            }
            
            public bool IsDead => Delegate.IsDead;
            public Delegate Rebuild() => Delegate.Rebuild();
            public bool Matches(Delegate other) => Delegate.Matches(other);
        }

        public void Add(object target, Action<T1, T2> callback, int priority = 0, bool once = false)
        {
            listeners.Add(new Listener(target, callback, priority, once));
            isDirty = true;
        }

        public void AddUnique(object target, Action<T1, T2> callback, int priority = 0, bool once = false)
        {
            if (Contains(callback))
            {
                return;
            }


            Add(target, callback, priority, once);
        }

        public void Remove(object target, Action<T1, T2> callback)
        {
            listeners.RemoveAll(l => l.Target == target && l.Matches(callback));
        }

        public void RemoveAll()
        {
            listeners.Clear();
        }

        public void RemoveAll(object target)
        {
            listeners.RemoveAll(l => l.Target == target);
        }

        public void Broadcast(T1 arg0, T2 arg1)
        {
            if (isDirty)
            {
                listeners.Sort((a, b) => b.Priority.CompareTo(a.Priority));
                isDirty = false;
            }

            for (int i = 0; i < listeners.Count; i++)
            {
                var listener = listeners[i];

                if (listener.IsDead)
                {
                    listeners.RemoveAt(i);
                    continue;
                }

                var del = (Action<T1, T2>)listener.Rebuild();
                try
                {
                    del?.Invoke(arg0, arg1);
                }
                catch (Exception e)
                {
                    FPolyLog.Error("Poly.Events",$"FPolyAction<{typeof(T1)}, {typeof(T2)}> Exception: {e}");
                }

                if (listener.Once)
                {
                    listeners.RemoveAt(i);
                }
            }
        }
        
        private bool Contains(Action<T1, T2> callback)
        {
            foreach (var l in listeners)
            {
                if (l.Matches(callback))
                {
                    return true;
                }
            }
            return false;
        }
    }
}