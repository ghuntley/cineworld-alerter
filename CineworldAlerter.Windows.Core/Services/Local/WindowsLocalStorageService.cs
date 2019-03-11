using System.Threading.Tasks;
using Cimbalino.Toolkit.Services;
using CineworldAlerter.Core.Services.Local;

namespace CineworldAlerter.Windows.Core.Services.Local
{
    public class WindowsLocalStorageService : ILocalStorageService
    {
        private readonly IStorageService _storageService;

        public WindowsLocalStorageService(
            IStorageService storageService) 
            => _storageService = storageService;

        public Task<bool> FileExistsAsync(string fileName) 
            => _storageService.Local.FileExistsAsync(fileName);

        public Task WriteAllTextAsync(string fileName, string content) 
            => _storageService.Local.WriteAllTextAsync(fileName, content);

        public Task<string> ReadAllTextAsync(string fileName) 
            => _storageService.Local.ReadAllTextAsync(fileName);

        public Task DeleteFileAsync(string fileName) 
            => _storageService.Local.DeleteFileAsync(fileName);
    }
}
