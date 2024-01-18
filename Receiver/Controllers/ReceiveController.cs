using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Receiver.Model;
using Receiver.Services;

namespace Receiver.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReceiveController : ControllerBase
    {
        private IList<FinanceProgram> _finprogs { get; set; }

        public ReceiveController()
        {
            _finprogs = FinancePrograms.Programs;
        }

        [HttpGet]
        public IActionResult GetFinProg()
        {
            return Ok(_finprogs);
        }
    }
}
