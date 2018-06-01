using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;

namespace ClientApp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.Run(async (context) =>
            {
                var url = new Uri("http://localhost:5000/normal");
                var size = GetFileSize(url);

                DownloadFile(url, size, env.ContentRootPath + @"\down.zip");               

                await context.Response.WriteAsync(size.ToString());
            });
        }

        private static void DownloadFile(Uri url,long range,string downloadPath)
        {
            var client = new WebClient();

            client.Headers.Add(HttpRequestHeader.Range, "bytes=0-"+range);

            client.DownloadFile(url, downloadPath);
        }

        private static long GetFileSize(Uri uriPath)
        {
            var webRequest = HttpWebRequest.Create(uriPath);
            webRequest.Method = "HEAD";

            using (var webResponse = webRequest.GetResponse())
            {
                var fileSize = webResponse.Headers.Get("Content-Length");
                return Convert.ToInt64(fileSize);
            }
        }
    }
}