using System;
using System.Reflection;
using OpenSource.Aspect.Controllers;

namespace OpenSource.Web.Listed
{
    public static class ViewController
    {
        #region 方法

        public static object GetWebController(SysChannel channel, string requestPath, out MethodInfo actionMethod)
        {
            string controllerParam = string.Empty;
            return GetController(channel, requestPath, controllerParam, out actionMethod);
        }

        public static object GetController(SysChannel channel, string requestPath, string controllerParam, out MethodInfo actionMethod)
        {
            object controller;
            var mappingParams = requestPath.Split('/');
            string moduleParam = "Default";
            Type controllerType;
            if (mappingParams.Length > 2)
            {
                moduleParam = mappingParams[1];
                controllerParam = mappingParams[2];
            }
            //获取控制器类型
            controllerType = GetControllerType(moduleParam);
            //获取控制器方法
            actionMethod = GetMethodInfo(controllerType, controllerParam);
            //获取控制器
            controller = GetController(channel, controllerType);

            return controller;
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 获取控制器
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="controllerType"></param>
        /// <returns></returns>
        private static object GetController(SysChannel channel, Type controllerType)
        {
            object controller = null;

            if (controllerType != null)
            {
                controller = Activator.CreateInstance(controllerType, channel);
            }
            return controller;
        }

        /// <summary>
        /// 获取控制器类型
        /// </summary>
        /// <param name="moduleParam"></param>
        /// <returns></returns>
        private static Type GetControllerType(string moduleParam)
        {
            Assembly controllerAssem = Assembly.Load("OpenSource.Aspect.Controllers");
            Type[] types = controllerAssem.GetTypes();
            foreach (Type type in types)
            {
                if (type.Name == (moduleParam + "Controller"))
                {
                    return type;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取控制器方法信息
        /// </summary>
        /// <param name="controllerType">控制器类型</param>
        /// <param name="controllerMethod">控制器方法</param>
        /// <returns></returns>
        private static MethodInfo GetMethodInfo(Type controllerType, string controllerMethod)
        {
            if (controllerType != null)
            {
                MethodInfo[] methods = controllerType.GetMethods();
                foreach (MethodInfo method in methods)
                {
                    if (method.Name.ToUpper() == controllerMethod.ToUpper())
                    {
                        return method;
                    }
                }
            }
            return null;
        }
        #endregion
    }
}