using System;

namespace Signals
{
    /// <summary>
    /// The Signal interface.
    /// </summary>
    public interface ISignal
    {
        Action OnTriggered { get; }

        void Trigger();
    }

    /// <summary>
    /// The Signal interface.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="Value"/>.</typeparam>
    public interface ISignal<T>
    {
        /// <summary>
        /// The current value of the Signal. 
        /// Setting the value triggers the <see cref="OnChanged"/> event. 
        /// </summary>
        T Value { get; set; }

        /// <summary>
        /// The event invoked when a <see cref="Value"/> is assigned to the Signal.
        /// </summary>
        Action<T> OnChanged { get; }

        /// <summary>
        /// Triggers the <see cref="OnChanged"/> event with the current value.
        /// </summary>
        void ForceChange();
    }
}