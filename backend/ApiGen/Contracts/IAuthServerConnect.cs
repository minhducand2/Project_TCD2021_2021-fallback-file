using System.Threading.Tasks;

namespace ApiGen.Contracts
{
    public interface IAuthServerConnect
    {
        Task<string> RequestClientCredentialsTokenAsync();
    }
}
