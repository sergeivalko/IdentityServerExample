using System;
using System.IO;
using System.Threading.Tasks;
using Profile.Application.Interfaces;

namespace Profile.Infrastructure
{
    public class DirectoryFileService : IFileService
    {
        private static readonly string DefaultDirectory =
            $"{Path.Combine(Environment.CurrentDirectory, "ProfilePhoto")}";

        public async Task<string> SaveAsync(byte[] fileData)
        {
            var fileName = Guid.NewGuid().ToString("N");
            var filePath = GetFilePath(fileName);
            await File.WriteAllBytesAsync(filePath, fileData);
            return fileName;
        }

        public void Delete(string fileName)
        {
            var filePath = GetFilePath(fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public async Task<byte[]> GetAsync(string fileName)
        {
            var filePath = GetFilePath(fileName);
            var fileData = await File.ReadAllBytesAsync(filePath);
            return fileData;
        }

        public string GetFilePathByFileName(string fileName)
        {
            return GetFilePath(fileName);
        }

        private string GetFilePath(string fileName)
        {
            if (!Directory.Exists(DefaultDirectory))
            {
                Directory.CreateDirectory(DefaultDirectory);
            }

            return Path.Combine(DefaultDirectory, fileName);
        }
    }
}