using Autofac;
using FluentValidation;
using HotelSystem.Data.Repository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Reflection;
using ConnectChain.Controllers.BaseEndpoints;
using ConnectChain.Data.Context;
using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Data.Repositories.UserRepository;
using ConnectChain.Helpers;
using ConnectChain.Models;

namespace ConnectChain.Config
{
    public class ApplicationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConnectChainDbContext>().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            builder.RegisterType(typeof(UserRepository)).As(typeof(IUserRepository)).InstancePerLifetimeScope();


            builder.RegisterType<MailServices>().As<IMailServices>().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(BaseEndpointParameters<>))
              .AsSelf()
              .InstancePerLifetimeScope();
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
           #region Register MediatR
            builder.RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
    .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
    .AsImplementedInterfaces()
    .InstancePerLifetimeScope();
            builder.Register<Func<Type, object>>(ctx =>
            {
                var context = ctx.Resolve<IComponentContext>();
                return type => context.ResolveOptional(type);
            }).InstancePerLifetimeScope();


            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AsClosedTypesOf(typeof(IRequestHandler<,>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            #endregion


            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Name.EndsWith("Services"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
