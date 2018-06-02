using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
            _logger.LogWarning($"Range ---> Download started at {DateTime.Now.TimeOfDay} \n ");

            var fileInfo = new FileInfo(path);
            return PhysicalFile(path, mediaType, "DownloadRang.zip", true);
        }

        [Route("range2")]
        public IActionResult DownloadRange2([FromServices] IMemoryCache memoryCache )
        {

            var range = HttpContext.Request.Headers["Range"];

            if (!string.IsNullOrEmpty(range))
            {
                int count = 1;
                count = ReadFromCache(memoryCache);

                var result = GetRange(range);

                _logger.LogWarning($" #{count} - Download started at {DateTime.Now.TimeOfDay} from {result.Item1} ");
            }

            var fileInfo = new FileInfo(path);
            return PhysicalFile(path, mediaType, "DownloadRang.zip", true);
        }

        private static int ReadFromCache(IMemoryCache memoryCache)
        {
            var cacheExists = memoryCache.TryGetValue("count", out int count);
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(10));

            if (cacheExists)
            {
                count++;
                memoryCache.Set("count", count, cacheEntryOptions);
            }
            else
            {
                memoryCache.Set("count", 1, cacheEntryOptions);
                count = 1;
            }

            return count;
        }

        private Tuple<long, long> GetRange(string range)
        {
            // bytes=1213-4545

            var b = range.Split("=");
            var s = b[1].Split("-");

            if (string.IsNullOrEmpty(s[1]))
            {
                s[1] = "0";
            }

            return new Tuple<long, long>(Convert.ToInt64(s[0]), Convert.ToInt64(s[1]));
        }
    }
}