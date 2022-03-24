using Microsoft.AspNetCore.Mvc;
using Profile.Application.Interfaces;

namespace Profile.API.Controllers
{
    [Route("api/files")]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet("{fileName}")]
        public IActionResult GetFile([FromRoute] string fileName)
        {
            var filePath = _fileService.GetFilePathByFileName(fileName);
            return PhysicalFile(filePath, "image/jpeg");
        }
    }
}