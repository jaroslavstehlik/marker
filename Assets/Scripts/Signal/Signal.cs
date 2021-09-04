using System;
using Newtonsoft.Json;

namespace Signals
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Signal : ISignal
    {
        protected Action _onTriggered = default;

        /// <summary>
        /// Invoked when <see cref="Trigger"/> is called.
        /// </summary>
        public Action OnTriggered {
            get {
                return _onTriggered;
            }
        }

        public Signal() { }

        /// <summary>
        /// Triggers the <see cref="OnTriggered"/> event.
        /// </summary>
        public void Trigger()
        {
            _onTriggered.Invoke();
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class Signal<T>
    {        
        protected T _value = default;

        /// <summary>
        /// The current value of the Signal. 
        /// Setting the value triggers the <see cref="changed"/> event. 
        /// If you want to add a check before setting the value and triggering the event override the <see cref="ValidateValue"/> method and make sure <see cref="UseValidation"/> is true.
        /// </summary>

        [JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]
        public T value {
            get {
                return _value;
            }

            set {
                ProcessValue(ref value);
                if (!_useValidation || ValidateValue(value))
                {
                    _value = value;
                    _changed?.Invoke(value);
                }
            }
        }

        protected Action<T> _changed = default;

        /// <summary>
        /// The event invoked when a <see cref="value"/> is assigned to the Signal.
        /// </summary>
        public Action<T> changed {
            get {
                return _changed;
            }
            set {
                _changed = value;
            }
        }

        protected bool _useValidation = true;
        public bool useValidation { get => _useValidation; set => _useValidation = value; }

        public Signal()
        {
            _value = default;
        }

        public Signal(T value, bool useValidation = true)
        {
            _useValidation = useValidation;
            ProcessValue(ref value);
            InitValue(value);
        }

        /// <summary>
        /// Triggers the <see cref="changed"/> event with the current value.
        /// </summary>
        public void ForceChange()
        {
            _changed?.Invoke(_value);
        }

        /// <summary>
        /// Initialize value of the Signal without triggering events.
        /// </summary>
        public virtual void InitValue(T value)
        {
            _value = value;
        }

        /// <summary>
        /// Override this method to preprocess values before applying them.
        /// </summary>
        /// <param name="value">The value to process</param>
        protected virtual void ProcessValue(ref T value) { }

        /// <summary>
        /// Override this method to check whether a value is valid and/or if it has changed. 
        /// If <see cref="UseValidation"/> is true the <see cref="value"/> is set and the <see cref="changed"/> event is invoked only if this method returns true.
        /// </summary>
        /// <param name="value">The new value.</param>
        /// <returns>True if the <see cref="value"/> shoud be updated and the <see cref="changed"/> event should be triggered, false otherwise.</returns>
        protected virtual bool ValidateValue(T value)
        {
            return !UnityEngine.Object.Equals(_value, value);
        }
    }
}