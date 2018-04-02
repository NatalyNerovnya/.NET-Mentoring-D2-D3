using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace CustomMapper
{
    public class ObjectsMapper
    {
        public TDestination Map<TDestination>(object obj)
        {
            var ctor = Expression.New(typeof(TDestination));
            MemberBinding[] bindings = CreateInitBlock(obj, typeof(TDestination)).ToArray();

            var memberInit = Expression.MemberInit(ctor, bindings);

            var mapFunc = Expression.Lambda<Func<TDestination>>(memberInit);

            return mapFunc.Compile().Invoke();
        }

        private IEnumerable<MemberBinding> CreateInitBlock(object source, Type destinationType)
        {
            var createdTypeProperties = source.GetType().GetProperties();
            var newTypeProperties = destinationType.GetProperties();
            var bindings = new List<MemberAssignment>();

            foreach (var prop in newTypeProperties)
            {
                var current =
                    createdTypeProperties.FirstOrDefault(
                        p =>
                            p.Name.Equals(prop.Name, StringComparison.InvariantCultureIgnoreCase) &&
                            p.PropertyType == prop.PropertyType);

                if (current != null)
                {
                    bindings.Add(Expression.Bind(prop, Expression.Constant(current.GetValue(source))));
                }
            }

            return bindings;
        }

    }
}
