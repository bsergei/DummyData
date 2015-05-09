using System;
using System.Collections.Generic;
using System.Linq;

namespace DataGenerator.Core
{
    public class InstanceGenerator<TInstance>
    {
        private readonly Func<GeneratorContext<TInstance>, TInstance> instanceFactory_;
        private readonly List<PropertyGenerator<TInstance>> propertyGenerators_ = new List<PropertyGenerator<TInstance>>();
        private TInstance instance_;
        private readonly IDictionary<PropertyGenerator<TInstance>, Func<GeneratorContext<TInstance>, object>> valueGeneratorFactories_ =
            new Dictionary<PropertyGenerator<TInstance>, Func<GeneratorContext<TInstance>, object>>();

        public InstanceGenerator(Func<GeneratorContext<TInstance>, TInstance> instanceFactory)
        {
            instanceFactory_ = instanceFactory;
        }

        protected InstanceGenerator(TInstance instance)
        {
            instance_ = instance;
        }

        public void AddPropertyValueGenerator(PropertyGenerator<TInstance> propertyGenerator)
        {
            propertyGenerators_.Add(propertyGenerator);
        }

        public void AddValueGeneratorFactory<TProperty>(
            PropertyGenerator<TInstance, TProperty> propertyGenerator, 
            Func<GeneratorContext<TInstance>, IValueGenerator<TProperty>> factory)
        {
            valueGeneratorFactories_.Add(propertyGenerator, factory);
        }

        public bool AnyPropertyGeneratorIterable
        {
            get { return propertyGenerators_.Any(_ => _.CanIterate); }
        }

        public Option<TInstance> Generate(GeneratorContext<TInstance> context)
        {
            if (!context.Initialized)
            {
                var valueGenerators = valueGeneratorFactories_.ToDictionary(pair => pair.Key, pair => pair.Value(context));
                context.Initialize(valueGenerators);
            }

            if (instanceFactory_ != null)
                instance_ = instanceFactory_(context);

            var list = context.GeneratedInstances;
            list.Add(instance_);
            context.CurrentInstanceIndex = list.Count - 1;

            bool allSet = true;
            foreach (var propertyValueGenerator in propertyGenerators_)
                allSet = allSet && propertyValueGenerator.TryGenerateAndSetValue(context);

            return new Option<TInstance>(allSet, instance_);
        }
    }

    public class InstanceGeneratorBootstrapper<T> : InstanceGenerator<T>
    {
        public InstanceGeneratorBootstrapper(T instance)
            : base(instance)
        {
        }

        public void Generate()
        {
            Generate(new GeneratorContext<T>(null));
        }
    }
}