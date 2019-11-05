using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace PCF.Replatform.Test.Helpers
{
    public static class TestHelper
    {
        public static TReturn InvokeNonPublicInstanceMethod<TReturn>(this object parentObject, string methodName, params object[] methodParameters)
        {
            return (TReturn)InvokeNonPublicInstanceMethod<TReturn>(parentObject, methodName, methodParameters);
        }

        public static object InvokeNonPublicInstanceMethod(this object parentObject, string methodName, params object[] methodParameters)
        {
            var method = parentObject.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);

            if (method == null)
                throw new MissingMethodException(parentObject.GetType().FullName, methodName);

            try
            {
                return method.Invoke(parentObject, methodParameters);
            }
            catch (Exception ex)
            {
                throw ex.InnerException ?? ex;
            }
        }

        public static object InvokePrivateStaticMethod(Type parentType, string methodName, params object[] methodParameters)
        {
            var method = parentType.GetMethod(methodName);

            if (method == null)
                throw new MissingMethodException(parentType.FullName, methodName);

            try
            {
                return method.Invoke(null, methodParameters);
            }
            catch (Exception ex)
            {
                throw ex.InnerException ?? ex;
            }
        }

        public static object GetNonPublicStaticFieldValue(Type parentType, string fieldName)
        {
            var field = parentType.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);

            if (field == null)
                throw new MissingMemberException(parentType.FullName, fieldName);

            return field.GetValue(null);
        }

        public static void SetNonPublicStaticFieldValue(Type parentType, string fieldName, object value)
        {
            var field = parentType.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);

            if (field == null)
                throw new MissingMemberException(parentType.FullName, fieldName);

            field.SetValue(null, value);
        }

        public static TReturn GetNonPublicInstanceFieldValue<TReturn>(this object parentObject, string fieldName)
        {
            var field = parentObject.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);

            if (field == null)
                throw new MissingMemberException(parentObject.GetType().FullName, fieldName);

            return (TReturn)field.GetValue(parentObject);
        }

        public static object GetNonPublicInstanceFieldValue(this object parentObject, string fieldName)
        {
            var field = parentObject.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);

            if (field == null)
                throw new MissingMemberException(parentObject.GetType().FullName, fieldName);

            return field.GetValue(parentObject);
        }

        public static void SetNonPublicInstanceFieldValue(this object parentObject, string fieldName, object value)
        {
            var field = parentObject.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);

            if (field == null)
                throw new MissingMemberException(parentObject.GetType().FullName, fieldName);

            field.SetValue(parentObject, value);
        }
    }

    public class ConfigBuilderStub : IConfigurationBuilder
    {
        public IDictionary<string, object> Properties => throw new NotImplementedException();

        public IList<IConfigurationSource> Sources { get; private set; } = new List<IConfigurationSource>();

        public IConfigurationBuilder Add(IConfigurationSource source)
        {
            Sources.Add(source);
            return this;
        }

        public IConfigurationRoot Build()
        {
            throw new NotImplementedException();
        }
    }
}
