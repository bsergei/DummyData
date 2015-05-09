using System;
using System.Linq.Expressions;
using System.Reflection;

namespace DataGenerator.Core
{
    public class PropertyGenerator<TInstance, TProp> : PropertyGenerator<TInstance>
    {
        private readonly InstanceGenerator<TInstance> instanceGenerator_;
        private readonly Action<TInstance, TProp> setterFunc_;
        private Func<GeneratorContext<TInstance>, Option<TProp>> generateValueFunc_;

        public PropertyGenerator(InstanceGenerator<TInstance> instanceGenerator, Expression<Func<TInstance, TProp>> expression)
        {
            instanceGenerator_ = instanceGenerator;
            setterFunc_ = GetSetter(expression);
        }

        public InstanceGenerator<TInstance> InstanceGenerator
        {
            get { return instanceGenerator_; }
        }

        public void SetValueGenerator(Func<GeneratorContext<TInstance>, Option<TProp>> generateValueFunc, bool canIterate)
        {
            generateValueFunc_ = generateValueFunc;
            CanIterate = canIterate;
        }

        public override bool TryGenerateAndSetValue(GeneratorContext<TInstance> context)
        {
            var newValue = generateValueFunc_(context);
            if (newValue.HasValue)
            {
                var currentInstance = context.CurrentInstance;
                setterFunc_(currentInstance, newValue.Value);
                return true;
            }
            else
            {
                return false;
            }
        }

        private static Action<TInstance, TProp> GetSetter(Expression<Func<TInstance, TProp>> expression)
        {
            var memberExpression = (MemberExpression)expression.Body;
            var setMethod = ((PropertyInfo)memberExpression.Member).GetSetMethod();
            var parameterT = Expression.Parameter(typeof(TInstance), "instance");
            var parameterU = Expression.Parameter(typeof(TProp), "newValue");

            var newExpression =
                Expression.Lambda<Action<TInstance, TProp>>(
                    Expression.Call(parameterT, setMethod, parameterU),
                    parameterT,
                    parameterU);

            return newExpression.Compile();
        }
    }

    public abstract class PropertyGenerator<TInstance>
    {
        public abstract bool TryGenerateAndSetValue(GeneratorContext<TInstance> context);

        public bool CanIterate { get; protected set; }
    }
}