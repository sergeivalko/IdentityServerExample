using System.Collections.Generic;
using System.Threading.Tasks;

namespace Profile.Application.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveAsync(IEnumerable<byte> fileData);
        void Delete(string fileName);
        Task<byte[]> GetAsync(string fileName);
        string GetFilePathByFileName(string fileName);
    }
}
