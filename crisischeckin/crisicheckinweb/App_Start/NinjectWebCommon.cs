using Models;

[assembly: WebActivator.PreApplicationStartMethod(typeof(crisicheckinweb.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(crisicheckinweb.App_Start.NinjectWebCommon), "Stop")]

namespace crisicheckinweb.App_Start
{
    using System;
    using System.Web;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using Services.Interfaces;
    using Services;
    using crisicheckinweb.Wrappers;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IDisaster>().To<DisasterService>().InRequestScope();
            kernel.Bind<IVolunteerService>().To<VolunteerService>().InRequestScope();
            kernel.Bind<IDataService>().To<DataService>().InRequestScope();
            kernel.Bind<IAdmin>().To<AdminService>().InRequestScope();
            kernel.Bind<ICluster>().To<ClusterService>().InRequestScope();
            kernel.Bind<CrisisCheckin>().ToSelf().InRequestScope();
            kernel.Bind<IWebSecurityWrapper>().To<WebSecurityWrapper>().InRequestScope();
            kernel.Bind<IMessageService>().To<MessageService>();
        }        
    }
}
