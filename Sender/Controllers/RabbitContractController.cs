using Microsoft.AspNetCore.Mvc;
using Sender.Model;
using Sender.Services;


namespace Sender.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RabbitContractController : ControllerBase
    {
        private IList<Contract> _contract { get; set; }
        private readonly IRabbitMqSender _sender;
        public RabbitContractController(IRabbitMqSender rabbitMsgSender)
        {
            _contract = new List<Contract>();
            _sender = rabbitMsgSender;
        }

        [HttpPost]
        public IActionResult MakeANewContract(Contract contract)
        {
            _contract.Add(contract);
            _sender.MessageSend(contract);
            return Ok();
        }
    }
}
