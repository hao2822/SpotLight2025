using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Utils
{
    public class ReflectionUtil
    {
        public static Type[] GetTypesOfAttribute(Type attributeType)
        {
            var asm = Assembly.GetAssembly(attributeType);
            var types = asm.GetExportedTypes();

            bool IsMyAttribute(Attribute[] o)
            {
                foreach (Attribute a in o)
                {
                    if (a.GetType() == attributeType) return true;
                }

                return false;
            }

            var cosType = types.Where(o => IsMyAttribute(Attribute.GetCustomAttributes(o, true))).ToArray();

            return cosType;
        }

        public static Type[] GetTypesOfMethod(Assembly assembly, string methodName, BindingFlags bindingFlags)
        {
            var types = assembly.GetExportedTypes();

            var cosType = types.Where(type => type.GetMethod(methodName, bindingFlags) != null).ToArray();

            return cosType;
        }

        public static Type[] GetTypesInheritFrom(Type parentType)
        {
            var types = Assembly.GetAssembly(parentType).GetTypes();
            List<Type> subclasses = new List<Type>();
            foreach (var type in types)
            {
                var baseType = type.BaseType; //获取基类
                while (baseType != null) //获取所有基类
                {
                    if (baseType.Name == parentType.Name)
                    {
                        var objtype = Type.GetType(type.FullName, true);
                        if (!objtype.IsAbstract)
                        {
                            subclasses.Add(objtype);
                        }

                        break;
                    }
                    else
                    {
                        baseType = baseType.BaseType;
                    }
                }
            }

            return subclasses.ToArray();
        }

        public static Type[] GetTypesInheritFrom(Type parentType, List<Type> excludedParentTypes)
        {
            var types = Assembly.GetAssembly(parentType).GetTypes();
            var subclasses = new List<Type>();
            foreach (var type in types)
            {
                var baseType = type.BaseType; //获取基类
                while (baseType != null) //获取所有基类
                {
                    if (baseType.Name == parentType.Name)
                    {
                        var objtype = Type.GetType(type.FullName, true);
                        if (!objtype.IsAbstract)
                        {
                            subclasses.Add(objtype);
                        }

                        break;
                    }
                    else if (excludedParentTypes.Find(excludedType => excludedType.Name == baseType.Name) != null)
                    {
                        break;
                    }
                    else
                    {
                        baseType = baseType.BaseType;
                    }
                }
            }

            return subclasses.ToArray();
        }
    }
}