﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        readonly IConfiguration _configuration;

        public FilesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> GetBaseUrl()
        {
            return Ok(_configuration["BaseStorageUrl"]);
        }
    }
}
