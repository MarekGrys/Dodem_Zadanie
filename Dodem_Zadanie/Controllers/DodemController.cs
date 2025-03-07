using Dodem_Zadanie.Entities;
using Dodem_Zadanie.Models;
using Dodem_Zadanie.Services;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Cms;

namespace Dodem_Zadanie.Controllers
{
    [Route("Dodem")]
    public class DodemController: ControllerBase
    {
        private readonly IDodemService _service;

        public DodemController(IDodemService service)
        {
            _service = service;
        }

        [HttpPost("SendEmail")]
        public ActionResult<string> SendEmail([FromQuery]string recipient,[FromQuery] int templateID,[FromBody] PlaceholderBase model)
        {
            try
            {
                var result = _service.SendEmail(recipient, templateID, model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetTemplates")]
        public ActionResult<List<MailTemplate>> GetTemplates()
        {
            try
            {
                var result = _service.GetTemplates();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
