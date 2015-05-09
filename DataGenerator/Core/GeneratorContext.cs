using System;
using System.Collections.Generic;

namespace DataGenerator.Core
{
    public class GeneratorContext<TInstance> : GeneratorContext
    {
        private IDictionary<PropertyGenerator<TInstance>, object> valueGenerators_ =
            new Dictionary<PropertyGenerator<TInstance>, object>();

        private readonly List<TInstance> generatedInstances_ = new List<TInstance>();

        public GeneratorContext(GeneratorContext parent)
            : base(parent)
        {
        }

        public List<TInstance> GeneratedInstances
        {
            get
            {
                if (!Initialized)
                    throw new InvalidOperationException();

                return generatedInstances_;
            }
        }

        public TInstance CurrentInstance
        {
            get
            {
                return GeneratedInstances[CurrentInstanceIndex];
            }
        }

        public void Initialize(IDictionary<PropertyGenerator<TInstance>, object> valueGenerators)
        {
            Initialized = true;
            valueGenerators_ = valueGenerators;
        }

        public IValueGenerator<TV> GetValueGenerator<TV>(PropertyGenerator<TInstance, TV> instance)
        {
            if (!Initialized)
                throw new InvalidOperationException();

            return (IValueGenerator<TV>) valueGenerators_[instance];
        }
    }

    public abstract class GeneratorContext
    {
        private int currentInstanceIndex_ = -1;

        protected GeneratorContext(GeneratorContext parent)
        {
            Parent = parent;
        }

        public int CurrentInstanceIndex
        {
            get
            {
                if (!Initialized)
                    throw new InvalidOperationException();

                return currentInstanceIndex_;
            }
            set { currentInstanceIndex_ = value; }
        }

        public bool Initialized { get; protected set; }

        public GeneratorContext Parent { get; private set; }
    }
}