using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace ServerApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger _logger;
        
        private readonly string mediaType = "application/x-rar-compressed";
        private readonly string path;

        public HomeController(ILoggerFactory loggerFactory, IHostingEnvironment _hostingEnvironment)
        {
            _logger = loggerFactory.CreateLogger("Download");
            path = Path.Combine(_hostingEnvironment.ContentRootPath, @"files\small.zip");
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("normal")]
        public IActionResult DownloadNormal()
        {
            _logger.LogWarning("Normal ---> download started at {} \n ", DateTime.Now.TimeOfDay);

            var fileInfo = new FileInfo(path);
            return PhysicalFile(path, mediaType, "DownloadNormal.zip");
        }

        [Route("range")]
        public IActionResult DownloadRange()
        {
            _logger.LogWarning("Range ---> Download started at {} \n ", DateTime.Now.TimeOfDay);

            var fileInfo = new FileInfo(path);
            return PhysicalFile(path, mediaType, "DownloadRang.zip", true);
        }
    }
}