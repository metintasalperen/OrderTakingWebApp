using Autofac;
using Business.Abstract;
using Business.Concrete;
using Core.Utilities.Security.Jwt;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Microsoft.AspNetCore.Http;

namespace Business.DependecyResolver
{
    public class AutofacBusinessModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<MenuManager>().As<IMenuService>();
            builder.RegisterType<OrderManager>().As<IOrderService>();
            builder.RegisterType<TableManager>().As<ITableService>();
            builder.RegisterType<EfMenuDal>().As<IMenuDal>();
            builder.RegisterType<EfOrderDal>().As<IOrderDal>();
            builder.RegisterType<EfTableDal>().As<ITableDal>();
            builder.RegisterType<EfUserDal>().As<IUserDal>();
            builder.RegisterType<AuthManager>().As<IAuthService>();
            builder.RegisterType<JwtHelper>().As<ITokenHelper>();
            builder.RegisterType<UserOperationManager>().As<IUserOperationService>();
            builder.RegisterType<EfOperationClaimsDal>().As<IOperationClaimsDal>();
            builder.RegisterType<EfUserOperationClaimsDal>().As<IUserOperationClaimsDal>();
            builder.RegisterType<OperationManager>().As<IOperationService>();
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>();
        }
    }
}