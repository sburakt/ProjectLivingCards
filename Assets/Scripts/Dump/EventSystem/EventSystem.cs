using System;
using System.Collections.Generic;

namespace EventSystem
{
    public interface IEvent { }
    public interface IActionHolder { }
    public class ActionHolder<T> : IActionHolder where T : IEvent
    {
        public Action<T> Action = delegate(T eventData) { }; 
        public ActionHolder(Action<T> action)
        {
            Action = action;
        }
        public void Invoke(T eventData)
        {
            Action.Invoke(eventData);
        }

        public void Add(Action<T> action)
        {
            Action += action;
        }

        public void Remove(Action<T> action)
        {
            Action -= action;
        }
    }

    public class EventSystem
    {
        private Dictionary<Type, IActionHolder> _actions = new ();
        
        public void Subscribe<T>(Action<T> action) where T : IEvent
        {
            if (!_actions.TryGetValue(typeof(T), out IActionHolder untypedActionHolder))
            {
                _actions[typeof(T)] = new ActionHolder<T>(action);
            }
            else if (untypedActionHolder is ActionHolder<T> typedActionHolder)
            {
                typedActionHolder.Add(action);
            }
            else
            {
                throw new Exception("Typemissmach : IActionHolder cannot be cast to ActionHolder");
            }
        }
        
        public void Unsubscribe<T>(Action<T> action) where T : IEvent
        {
            if (!_actions.TryGetValue(typeof(T), out IActionHolder untypedActionHolder))
            {
                return;
            }
            else if (untypedActionHolder is ActionHolder<T> typedActionHolder)
            {
                typedActionHolder.Remove(action);
            }
            else
            {
                throw new Exception("Typemissmach : IActionHolder cannot be cast to ActionHolder");
            }
        }

        public void Invoke<T>(T eventData) where T : IEvent
        {
            if (!_actions.TryGetValue(typeof(T), out IActionHolder untypedActionHolder))
                return;
            if (untypedActionHolder is ActionHolder<T> typedActionHolder)
            {
                typedActionHolder.Invoke(eventData);
            }
        }
    }

    public class LocalEvenSystem : EventSystem
    { }

    public class GlobalEventSystem : EventSystem
    {
        public static GlobalEventSystem Instance;

        static GlobalEventSystem()
        {
            Initialize();
        }

        private static void Initialize()
        {
            Instance = new GlobalEventSystem();
        }
    }
}