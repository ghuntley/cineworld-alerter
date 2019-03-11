using System.Threading.Tasks;

namespace CineworldAlerter.Core.Services.Local
{
    public interface ILocalStorageService
    {
        Task<bool> FileExistsAsync(string fileName);
        Task WriteAllTextAsync(string fileName, string content);
        Task<string> ReadAllTextAsync(string fileName);
        Task DeleteFileAsync(string fileName);
    }
}
