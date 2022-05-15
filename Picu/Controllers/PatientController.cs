using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Picu.Services.Interfaces;

namespace Picu.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly IMongoDbService _mongoDbService;
        private readonly ILogger _logger;

        #region Constructor

        public PatientController(
            IPatientService patientService,
            IMongoDbService mongoDbService,
            ILogger<PatientController> logger)
        {
            _patientService = patientService;
            _mongoDbService = mongoDbService;
            _logger = logger;
        }

        #endregion

        #region Get

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var result = _patientService.GetAll();
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e.HResult, e.InnerException == null ? e.Message : e.InnerException.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public IActionResult GetById([FromQuery] string patientId)
        {
            try
            {
                var result = _patientService.GetById(patientId);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e.HResult, e.InnerException == null ? e.Message : e.InnerException.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetByIdNoSql([FromQuery] string patientId)
        {
            try
            {
                var result = await _mongoDbService.GetByIdAsync(patientId);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e.HResult, e.InnerException == null ? e.Message : e.InnerException.Message);
                return BadRequest(e.Message);
            }
        }

        #endregion
    }
}
