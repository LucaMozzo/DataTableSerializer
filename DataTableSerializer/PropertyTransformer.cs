using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace DbDataReaderMapper
{
    public class PropertyTransformer
    {
        private Dictionary<PropertyInfo, Delegate> _transformations;
        private Dictionary<PropertyInfo, Type> _transformationOutputTypes;

        public PropertyTransformer()
        {
            _transformations = new Dictionary<PropertyInfo, Delegate>();
            _transformationOutputTypes = new Dictionary<PropertyInfo, Type>();
        }

        /// <summary>
        /// Add a custom transformation for the column type and values
        /// This can be used, for instance, to serialize a nullable type to a string
        /// </summary>
        /// <typeparam name="T">The model class</typeparam>
        /// <typeparam name="U">The original type</typeparam>
        /// <typeparam name="V">The mapped output type</typeparam>
        /// <param name="property">The output property that needs transformation</param>
        /// <param name="transformationFunction">The function that does the transformation</param>
        public PropertyTransformer AddTransformation<T, U, V>(Expression<Func<T, U>> property, Func<U, V> transformationFunction)
        {
            var memberInfo = ((MemberExpression)property.Body).Member;
            if (memberInfo.MemberType == MemberTypes.Property)
            {
                _transformations.Add((PropertyInfo)memberInfo, transformationFunction);
                _transformationOutputTypes.Add((PropertyInfo)memberInfo, typeof(V));
            }
            else
            {
                throw new ArgumentException($"The property selector should reference a property in the model, found {memberInfo.MemberType}");
            }

            return this;
        }

        internal Delegate this[PropertyInfo key]
        {
            get => _transformations.ContainsKey(key) ? _transformations[key] : null;
        }

        internal Type GetOutputType(PropertyInfo propertyInfo) => _transformationOutputTypes.ContainsKey(propertyInfo) ? _transformationOutputTypes[propertyInfo] : null;
    }
}
