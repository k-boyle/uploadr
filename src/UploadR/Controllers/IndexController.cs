﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UploadR.Database;
using UploadR.Services;

namespace UploadR.Controllers
{
    [Route("")]
    public class IndexController : UploadRController
    {
        private readonly UploadsService _fs;

        public IndexController(UploadRContext dbContext, UploadsService fs) : base(dbContext)
        {
            _fs = fs;
        }

        [Route("privacy"), HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [Route(""), HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("{name}"), HttpGet, ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> GetFile(string name, [FromQuery(Name = "p")] string password)
        {
            var file = await _fs.TryGetUploadByNameAsync(name);
            if (file.IsSuccess)
            {
                if (file.Value.Password != null && file.Value.Password != password)
                {
                    return Unauthorized("This file is protected, provide a valid password in your query.");
                }

                var path = $"./uploads/{file.Value.FileName}";
                var fileBytes = System.IO.File.ReadAllBytes(path);

                return File(fileBytes, file.Value.ContentType);
            }

            return file.Code switch
            {
                1 => Redirect("/"),
                2 => NotFound("File is removed."),
                3 => NotFound("File not found. It has been marked as removed."),
                _ => BadRequest()
            };
        }
    }
}
