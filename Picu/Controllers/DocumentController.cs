using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Picu.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Picu.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly ILogger _logger;

        #region Constructor

        public DocumentController(IDocumentService documentService, ILogger<DocumentController> logger)
        {
            _documentService = documentService;
            _logger = logger;
        }

        #endregion

        #region Post

        [HttpPost]
        public IActionResult SaveInSql()
        {
            try
            {
                var result = _documentService.SaveInSql();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Critical, $"{DateTime.Now} - {ex.Message} \n \t {ex.StackTrace}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveInMongoDb()
        {
            try
            {
                var result = await _documentService.SaveInMongoDb();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Critical, $"{DateTime.Now} - {ex.Message} \n \t {ex.StackTrace}");
                return BadRequest(ex.Message);
            }
        }

        #endregion
    }
}
