using BetaCycle4.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BetaCycle4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerRegisterController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> PostCustomerNew(CustomerRegister customerRegister)
        {

            CredentialsController credentialsController = new CredentialsController();
            Credentials credentials = credential;

            bool isNewUser = credentialsController.PostCredentials(credentials);

            return CreatedAtAction("GetCustomerNew", new { id = customerNew.CustomerId }, customerNew);
        }
    }
}
