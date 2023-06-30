using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class PipelineValue<T>
    {
        private List<Middleware> list = new List<Middleware>();

        public T GetValue()
        {
            T value = default;
            foreach (var item in list)
            {
                var result = item.Process(value);
                value = result.Value;
                if (result.Break)
                    return value;
            }
            return value;
        }

        public PipelineValue<T> Append(Middleware middleware)
        {
            list.Add(middleware);
            return this;
        }

        public PipelineValue<T> NewSwitchable(bool active, System.Func<T, Middleware.Result> func, out SwitchableMiddleware middleware)
        {
            middleware = NewSwitchable(active, func);
            return this;
        }

        public SwitchableMiddleware NewSwitchable(bool active, System.Func<T, Middleware.Result> func)
        {
            var middleware = SwitchableMiddleware.FromDelegate(func, active);
            list.Add(middleware);
            return middleware;
        }

        public PipelineValue<T> Remove(Middleware middleware)
        {
            list.Remove(middleware);
            return this;
        }


        public static implicit operator PipelineValue<T>(T baseValue)
        {
            return new PipelineValue<T>().Append(new SimpleValueMiddleware(() => baseValue));
        }


        public abstract class Middleware
        {
            public abstract Result Process(T value);

            public struct Result
            {
                public T Value { get; }
                public bool Break { get; }


                public Result(T value, bool breakPipeline)
                {
                    Value = value;
                    Break = breakPipeline;
                }

                public static Result ReturnBreak(T value)
                {
                    return new Result(value, true);
                }

                public static Result Continue(T value)
                {
                    return new Result(value, false);
                }
            }
        }

        public class SimpleValueMiddleware : Middleware
        {
            public System.Func<T> ValueGetter { get; set; }

            public SimpleValueMiddleware(System.Func<T> valueGetter)
            {
                ValueGetter = valueGetter;
            }

            public override Result Process(T value)
            {
                return Result.Continue(ValueGetter());
            }
        }

        public abstract class SwitchableMiddleware : Middleware, ISwitchable
        {
            public bool Active { get; set; }

            public SwitchableMiddleware() : this(false) { }

            public SwitchableMiddleware(bool active)
            {
                Active = active;
            }

            public override Result Process(T value)
            {
                if (Active)
                    return ProcessIfActive(value);

                return Result.Continue(value);
            }

            protected abstract Result ProcessIfActive(T value);


            public static SwitchableMiddleware FromDelegate(System.Func<T, Result> func, bool active = true)
            {
                return new DelegateSwitchableMiddleware(func, active);
            }

            private class DelegateSwitchableMiddleware : SwitchableMiddleware
            {
                private System.Func<T, Result> modifier;

                public DelegateSwitchableMiddleware(System.Func<T, Result> func, bool active) : base(active)
                {
                    modifier = func;
                }

                protected override Result ProcessIfActive(T value)
                {
                    return modifier(value);
                }
            }
        }
        public abstract class ModifyMiddleware : SwitchableMiddleware
        {
            public ModifyMiddleware() { }
            public ModifyMiddleware(bool active) : base(active) { }

            protected override Result ProcessIfActive(T value)
            {
                return Result.Continue(Modify(value));
            }

            protected abstract T Modify(T value);

            public static ModifyMiddleware FromDelegate(System.Func<T, T> func, bool active = true)
            {
                return new DelegateModifyMiddleware(func, active);
            }

            private class DelegateModifyMiddleware : ModifyMiddleware
            {
                private System.Func<T, T> modifier;

                public DelegateModifyMiddleware(System.Func<T, T> func, bool active) : base(active)
                {
                    modifier = func;
                }

                protected override T Modify(T value)
                {
                    return modifier(value);
                }
            }
        }
    }

    public interface ISwitchable
    {
        bool Active { get; set; }
    }

    public sealed class SwitchableMiddlewaresGroup : ISwitchable, IEnumerable<ISwitchable>
    {
        public bool Active
        {
            get => activeSelf;
            set
            {
                activeSelf = value;
                foreach (var item in list)
                {
                    item.Active = value;
                }
            }
        }

        private bool activeSelf;


        private List<ISwitchable> list = new();


        public T Add<T>(T middleware) where T : ISwitchable
        {
            list.Add(middleware);
            return middleware;
        }

        public IEnumerator<ISwitchable> GetEnumerator()
        {
            return ((IEnumerable<ISwitchable>)list).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)list).GetEnumerator();
        }
    }
}