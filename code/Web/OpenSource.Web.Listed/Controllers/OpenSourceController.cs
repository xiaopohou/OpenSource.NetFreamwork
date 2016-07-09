using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using OpenSource.Aspect.Controllers;
using ActionResult = System.Web.Mvc.ActionResult;

namespace OpenSource.Web.Listed.Controllers
{
    public class OpenSourceController : Controller
    {
        public ActionResult Freamwork()
        {
            SysChannel channel = new SysChannel();
            channel.In.Parameters = Request.Params;
            return MappingAction(channel);
        }

        protected ActionResult MappingAction(SysChannel channel)
        {
            MethodInfo actionMethod;
            var controller = ViewController.GetWebController(channel, Request.Path, out actionMethod);
            if (controller == null || actionMethod == null) return Content("no controller");

            SysOutput output = (actionMethod.Invoke(controller, null) as SysChannel).Out;
            //TODO: Headers
            foreach (var head in output.Headers)
            {
                Request.Headers[head.Key] = head.Value;
            }
            //TODO: Cookie
            foreach (var store in output.Stores) Response.Cookies.Set(new HttpCookie(store.Key, store.Value));
            //TODO: output.Content is *  :填写自己的条件
            if (output.Content is JsonActionResult) return Json(((JsonActionResult)output.Content).Model);

            return Content("no controller");
        }
    }
}