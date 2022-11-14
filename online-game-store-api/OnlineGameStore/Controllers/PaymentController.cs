using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.BLL.Models;
using OnlineGameStore.BLL.Services;
using OnlineGameStore.Extensions;
using System.IO;
using System.Threading.Tasks;

namespace OnlineGameStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentMethodService _paymentMethodService;
        private readonly IWebHostEnvironment _environment;
        private readonly UserClaims _userClaims;

        public PaymentController(IWebHostEnvironment webHostEnvironment, IPaymentMethodService paymentMethodService, UserClaims userClaims)
        {
            _paymentMethodService = paymentMethodService;
            _environment = webHostEnvironment;
            _userClaims = userClaims;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var payments = await _paymentMethodService.GetAllAsync();
            return Ok(payments);
        }

        [HttpGet("bank")]
        public async Task<IActionResult> BankPaymentAsync()
        {
            string directoryToInvoice = Path.Combine(_environment.ContentRootPath, $"Resources\\BankInvoice\\InvoiceTemplate.docx");

            await PayAsync(new BankPaymentService(), _userClaims.Id);

            string fileType = "application/octet-stream";
            string fileName = "BankInvoice.docx";

            return PhysicalFile(directoryToInvoice, fileType, fileName);
        }

        [HttpPost("ibox")]
        public async Task<IActionResult> IBoxPaymentAsync(IBoxModel iboxModel)
        {
            await PayAsync(new IBoxPaymentService(), iboxModel);
            return Ok();
        }

        [HttpPost("visa")]
        public async Task<IActionResult> VisaPaymentAsync(VisaModel visaModel)
        {
            await PayAsync(new VisaPaymentService(), visaModel);
            return Ok();
        }

        [HttpPost("timeout")]
        public async Task<IActionResult> StartPaymentTimeout([FromBody] int timeoutMinutes)
        {
            await _paymentMethodService.StartTimeout(_userClaims.Id, timeoutMinutes);
            return Ok();
        }

        private async Task PayAsync(IPaymentStrategy paymentStrategy, object paymentInfo)
        {
            _paymentMethodService.SetPaymentMethod(paymentStrategy);
            await _paymentMethodService.PayAsync(_userClaims.Id, paymentInfo);
        }
    }
}
