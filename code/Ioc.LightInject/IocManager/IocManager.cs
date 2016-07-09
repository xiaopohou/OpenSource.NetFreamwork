using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LightInject;
using System.Xml.Linq;

namespace IocManager
{
    public class IOCManager
    {
        #region 成员
        private static IServiceContainer container = new ServiceContainer();

        private static IDictionary<String, string> objectDefine = new Dictionary<String,
            string>();

        public static IOCManager Container = new IOCManager();
        #endregion

        #region 构造函数
        static IOCManager()
        {
            InstanceObects();
        }
        #endregion

        #region 方法

        private static void InstanceObects()
        {
            XElement root = XElement.Load("IocManager.xml");
            var objects = from obj in root.Elements() select obj;
            objectDefine = objects.ToDictionary(k => k.Attribute("key").Value, v => v.Attribute("value").Value);
            foreach (var key in objectDefine.Keys)
                RegisterAssembly(key, objectDefine[key]);
        }

        public static void RegisterAssembly(String interfaceAssem, String implAssem)
        {
            RegisterAssembly(Assembly.Load(interfaceAssem), Assembly.Load(implAssem));
        }

        private static void RegisterAssembly(Assembly interfaceAssembly, Assembly impAssembly)
        {
            String tag = impAssembly.GetName().Name.Split('.').Last();
            foreach (TypeInfo interfaceType in interfaceAssembly.DefinedTypes)
            {
                foreach (TypeInfo implType in impAssembly.DefinedTypes)
                {
                    if (interfaceType.IsAssignableFrom(implType))
                    {
                        container.Register(interfaceType, implType, tag);
                    }
                }
            }
        }

        public T GetInstance<T>()
        {
            return container.TryGetInstance<T>(GetMappingStringByType(typeof(T)));
        }

        private String GetMappingStringByType(Type targetType)
        {
            String targetNamespace = targetType.Namespace;
            foreach (var key in objectDefine.Keys)
            {
                if (targetNamespace.StartsWith(key))
                {
                    return objectDefine[key].Split('.').Last();
                }
            }
            return null;
        }
        #endregion
    }
}
