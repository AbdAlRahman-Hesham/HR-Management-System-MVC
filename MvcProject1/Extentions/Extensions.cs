using MvcProject1.BLL.UnitOfWork;
using MvcProject1.PL.Helpers;

namespace MvcProject1.PL.Extentions
{
    public static class Extensions
    {
        public static IServiceCollection 
            AddAppServices(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ISendEmail, SendEmail>();
            services.AddTransient<ISendSms, SendSms>();
            return services;
        }
    }
}
