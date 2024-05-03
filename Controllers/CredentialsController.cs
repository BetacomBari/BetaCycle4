using BetaCycle4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SqlManager.BLogic;

namespace BetaCycle4.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CredentialsController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> PostCredentials(Credentials credentials)
        {
            //DB CREDENTIALS
            DbUtility dbUtilityCredentials = new("Data Source=.\\SQLEXPRESS;Initial Catalog=CustomerCredentials;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            //inserimento email password in db

            //return CreatedAtAction("GetCustomerNew", new { id = customerNew.CustomerId }, customerNew);
            return Ok();
        }

    }
}
