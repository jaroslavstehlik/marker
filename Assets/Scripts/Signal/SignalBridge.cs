using System;

namespace Signals
{
    public class SignalBridge<T> : IDisposable
    {
        private Signal<T> _a = default;
        private Signal<T> _b = default;

        public SignalBridge(Signal<T> a, Signal<T> b)
        {
            _a = a;
            _b = b;

            _a.changed += AChanged;
            _b.changed += BChanged;
        }

        public void Dispose()
        {
            _a.changed -= AChanged;
            _b.changed -= BChanged;
        }

        void AChanged(T value)
        {
            ModifySignal(_b,AChanged, value);
        }

        void BChanged(T value)
        {
            ModifySignal(_a, BChanged, value);
        }
        
        static void ModifySignal<T>(Signal<T> signal, Action<T> action, T value)
        {
            // Unregister event
            signal.changed -= action;
            // Change value
            signal.value = value;
            // Register event
            signal.changed += action;
        }
    }
}