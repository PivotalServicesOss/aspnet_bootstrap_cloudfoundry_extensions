using System;
using System.Reflection;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Reflection
{
    public static class ReflectionHelper
    {
        public static T GetNonPublicInstanceFieldValue<T>(this object parentObject, string fieldName)
        {
            var field = parentObject.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);

            if (field == null)
                throw new MissingMemberException(parentObject.GetType().FullName, fieldName);

            return (T)field.GetValue(parentObject);
        }
    }
}
