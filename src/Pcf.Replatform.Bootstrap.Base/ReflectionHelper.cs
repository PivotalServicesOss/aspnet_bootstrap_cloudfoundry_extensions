using System;
using System.Reflection;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Reflection
{
    public static class ReflectionHelper
    {
        public static T GetNonPublicInstanceFieldValue<T>(this object parentObject, string fieldName)
        {
            var field = parentObject.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);

            if (field == null)
                throw new MissingMemberException(parentObject.GetType().FullName, fieldName);

            return (T)field.GetValue(parentObject);
        }

        public static T GetNonPublicStaticFieldValue<T>(Type parentType, string fieldName)
        {
            var field = parentType.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);

            if (field == null)
                throw new MissingMemberException(parentType.FullName, fieldName);

            return (T)field.GetValue(null);
        }

        public static T GetNonPublicInstancePropertyValue<T>(this object parentObject, string propertyName)
        {
            var property = parentObject.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance);

            if (property == null)
                throw new MissingMemberException(parentObject.GetType().FullName, propertyName);

            return (T)property.GetValue(parentObject);
        }

        public static T GetNonPublicStaticPropertyValue<T>(Type parentType, string propertyName)
        {
            var property = parentType.GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Static);

            if (property == null)
                throw new MissingMemberException(parentType.FullName, propertyName);

            return (T)property.GetValue(null);
        }
    }
}
