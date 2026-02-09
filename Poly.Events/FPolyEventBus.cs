using System;
using System.Collections.Generic;
using Poly.Common;
using Poly.Log;

namespace Poly.Events
{
	public class FPolyEventBus
	{
		private interface IPolyActionWrapper
		{

		}
		
		private class PolyActionWrapper<T> : IPolyActionWrapper
		{
			public FPolyAction<T> Action { get; private set; }

			public PolyActionWrapper(FPolyAction<T> action)
			{
				Action = action;
			}
		}
		
		private readonly Dictionary<Type, IPolyActionWrapper> eventMap = new();
		private readonly Dictionary<Type, HashSet<object>> broadcasterMap = new();
		private readonly Dictionary<Type, bool> pauseMap = new();

		public void Subscribe<T>(object target, Action<T> listener, bool unique = false) where T : IPolyEvent => SubscribeInternal(target, listener, unique);
		public void SubscribeOnce<T>(object target, Action<T> listener, bool unique = false) where T : IPolyEvent => SubscribeInternal(target, listener, unique, 0, true);
		public void SubscribeWithPriority<T>(object target, Action<T> listener, int priority, bool unique = false) where T : IPolyEvent => SubscribeInternal(target, listener, unique, priority);
		public void SubscribeOnceWithPriority<T>(object target, Action<T> listener, int priority, bool unique = false) where T : IPolyEvent => SubscribeInternal(target, listener, unique, priority, true);

		private void SubscribeInternal<T>(object target, Action<T> listener, bool unique = false, int priority = 0, bool once = false) where T : IPolyEvent
		{
			var actionToUpdate = GetOrCreate<T>();
			if (unique)
			{
				actionToUpdate.AddUnique(target, listener, priority, once);
				return;
			}

			actionToUpdate.Add(target, listener, priority, once);
		}

		public void Unsubscribe<T>(object target, Action<T> listener) where T : IPolyEvent
		{
			if (eventMap.TryGetValue(typeof(T), out var wrapper))
			{
				((PolyActionWrapper<T>)wrapper).Action.Remove(target, listener);
			}
		}

		public void Broadcast<T>(object sender, T evt) where T : IPolyEvent
		{
			if (IsPaused<T>())
			{
				return;
			}
			
			if (broadcasterMap.TryGetValue(typeof(T), out var allowed))
			{
				if (sender == null || !allowed.Contains(sender))
				{
					FPolyLog.Warning("Poly.Events",$"[PolyEventBus] Unauthorized broadcaster for event {typeof(T)}");
					return;
				}
			}

			if (eventMap.TryGetValue(typeof(T), out var wrapper))
			{
				((PolyActionWrapper<T>)wrapper).Action.Broadcast(evt);
			}
		}
		
		public void Pause<T>() where T : IPolyEvent => pauseMap[typeof(T)] = true;
		public void Resume<T>() where T : IPolyEvent => pauseMap[typeof(T)] = false;
		public bool IsPaused<T>() where T : IPolyEvent => pauseMap.TryGetValue(typeof(T), out var paused) && paused;

		public void RegisterBroadcaster<T>(object source) where T : IPolyEvent
		{
			if (!broadcasterMap.TryGetValue(typeof(T), out var set))
			{
				set = broadcasterMap[typeof(T)] = new HashSet<object>();
			}
			
			set.Add(source);
		}
		
		public void UnregisterBroadcaster<T>(object source) where T : IPolyEvent
		{
			if (broadcasterMap.TryGetValue(typeof(T), out var set))
			{
				set.Remove(source);
			}
		}
		
		public void Clear<T>() where T : IPolyEvent
		{
			if (eventMap.TryGetValue(typeof(T), out var wrapper))
			{
				((PolyActionWrapper<T>)wrapper).Action.RemoveAll();
			}
		}

		private FPolyAction<T> GetOrCreate<T>() where T : IPolyEvent
		{
			if (eventMap.TryGetValue(typeof(T), out var wrapper))
			{
				return ((PolyActionWrapper<T>)wrapper).Action;
			}

			var action = new FPolyAction<T>();
			wrapper = new PolyActionWrapper<T>(action);
			eventMap[typeof(T)] = wrapper;
			return action;
		}
	}
}