using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApiGen.Contracts
{
    public interface IServiceRegistration
    {
        void RegisterAppServices(IServiceCollection services, IConfiguration configuration);
    }
}
