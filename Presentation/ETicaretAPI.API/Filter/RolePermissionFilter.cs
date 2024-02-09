using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.CustomAttributies;
using Google.Apis.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Reflection;

namespace ETicaretAPI.API.Filter
{
    public class RolePermissionFilter : IAsyncActionFilter
    {
        readonly IUserService _userService;

        public RolePermissionFilter(IUserService userService)
        {
            _userService = userService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var name = context.HttpContext.User.Identity?.Name;
            if (!string.IsNullOrEmpty(name)&&name!="cengiz" )
            {
                var descriptor = context.ActionDescriptor as ControllerActionDescriptor; //sadece controllerin descripter larını getiriyor.
                var attribute = descriptor.MethodInfo.GetCustomAttribute(typeof(AuthorizeDefinitionAttribute)) as AuthorizeDefinitionAttribute;
                var httpAttribute=descriptor.MethodInfo.GetCustomAttribute(typeof(HttpMethodAttribute)) as HttpMethodAttribute;//Get Post Delete gibi attr alıtyor
                var code = $"{(httpAttribute!=null?httpAttribute.HttpMethods.First():HttpMethods.Get)}.{attribute.ActionType}.{attribute.Definition.Replace(" ","")}";
                var hasRole = await _userService.HasRolePermissionToEndpointAsync(name, code);
                if(!hasRole) {
                    context.Result = new UnauthorizedResult();
                }
                else
                {
                    await next();
                }
            }
            else
            {
                await next();
            }
        }
    }
}
