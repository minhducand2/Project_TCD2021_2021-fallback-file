using ApiGen.Data;
using ApiGen.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGen.Contracts
{
    public interface ISinhVienManager : IRepository<E500SinhVien>
    {
        Task<(IEnumerable<E500SinhVien> SinhViens, Pagination Pagination)> GetSinhViensAsync(UrlQueryParameters urlQueryParameters);

        //Add more class specific methods here when neccessary
        Task<IEnumerable<object>> test(); 
    }
}
