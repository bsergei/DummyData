using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataGenerator.Core
{
    public static class GeneratorBuilder
    {
        public static T Generate<T>(this T instance, Action<InstanceGenerator<T>> initializer)
        {
            var generator = new InstanceGeneratorBootstrapper<T>(instance);
            initializer(generator);
            generator.Generate();
            return instance;
        }

        public static PropertyGenerator<T, TV> Property<T, TV>(this InstanceGenerator<T> instance, Expression<Func<T, TV>> expression)
        {
            var propertyValueGenerator = new PropertyGenerator<T, TV>(instance, expression);
            instance.AddPropertyValueGenerator(propertyValueGenerator);
            return propertyValueGenerator;
        }

        public static InstanceGenerator<T> WithStaticValue<T, TV>(this PropertyGenerator<T, TV> instance, TV value)
        {
            instance.SetValueGenerator(_ => Option.Any(value), false);
            return instance.InstanceGenerator;
        }

        public static InstanceGenerator<T> WithGeneratedValue<T, TV, TW>(this PropertyGenerator<T, TV> instance, Func<TW, TV> converter, Func<GeneratorContext<T>, IValueGenerator<TW>> generatorFactory)
        {
            Func<GeneratorContext<T>, IValueGenerator<TV>> convertedFactory = _ =>
                {
                    var origFactory = generatorFactory(_);
                    return new LambdaValueGenerator<TV>(() =>
                        {
                            var value = origFactory.Next();
                            return value.HasValue ? Option.Any(converter(value.Value)) : Option.None<TV>();
                        });
                };

            return WithGeneratedValue(instance, convertedFactory);
        }

        public static InstanceGenerator<T> WithGeneratedValue<T, TV>(this PropertyGenerator<T, TV> property, Func<GeneratorContext<T>, IValueGenerator<TV>> valueGeneratorFactory)
        {
            property.InstanceGenerator.AddValueGeneratorFactory(property, valueGeneratorFactory);
            property.SetValueGenerator(
                parentContext =>
                {
                    var valueGenerator = parentContext.GetValueGenerator<TV>(property);
                    return valueGenerator.Next();
                },
                true);
            return property.InstanceGenerator;
        }

        public static InstanceGenerator<T> WithNewArray<T, TV>(this PropertyGenerator<T, TV[]> property, Action<InstanceGenerator<TV>> initializer) where TV : new()
        {
            return WithNewArray(property, InstanceFactory.Create<TV>(), initializer);
        }

        public static InstanceGenerator<T> WithNewArray<T, TV, TW>(this PropertyGenerator<T, TV[]> property, Func<GeneratorContext<TW>, TW> factory, Action<InstanceGenerator<TW>> initializer)
            where TW : TV
        {
            var instanceGenerator = new InstanceGenerator<TW>(factory);
            initializer(instanceGenerator);
            property.SetValueGenerator(
                parentContext =>
                    {
                        var context = new GeneratorContext<TW>(parentContext);
                        var objs = new List<TV>();
                        bool anyPropertyGeneratorIterable = instanceGenerator.AnyPropertyGeneratorIterable;
                        Option<TW> generatedItem;
                        while ((generatedItem = instanceGenerator.Generate(context)).HasValue)
                        {
                            objs.Add(generatedItem.Value);
                            if (!anyPropertyGeneratorIterable)
                                break;
                        }

                        return Option.Any(objs.ToArray());
                    },
                false);

            return property.InstanceGenerator;
        }
    }

    public static class InstanceFactory
    {
        public static Func<GeneratorContext<T>, T> Create<T>() where T : new()
        {
            return _ => new T();
        }

        public static Func<GeneratorContext<T>, T> Create<T>(Func<GeneratorContext<T>, T> factory)
        {
            return factory;
        }
    }
}