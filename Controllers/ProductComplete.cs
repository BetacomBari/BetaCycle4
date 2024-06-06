using BetaCycle4.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SqlManager.BLogic;

namespace BetaCycle4.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductComplete : ControllerBase
    {
        DbUtility dbUtilityLT2019 = new("Data Source=.\\SQLEXPRESS;Initial Catalog=AdventureWorksLT2019;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

        [HttpGet]
        public List<ProductC> GetProductComplete()
        {
           return dbUtilityLT2019.GetProductComplete();
        }

        [HttpGet("category/{categoryId}")]
        public List<ProductC> GetProductsByCategoryId(int categoryId)
        {
            return dbUtilityLT2019.GetProductsByCategoryId(categoryId);
        }

        [HttpGet("detail/{productId}")]
        public ProductC GetProductForDetail(int productId)
        {
            return dbUtilityLT2019.GetProductForDetail(productId);
        }

        [HttpGet("name/{name}")]
        public List<ProductC> GetProductsByName(string name)
        {
            return dbUtilityLT2019.GetProductsByName(name);

        }

        [HttpGet("GetProductsByPage/{offset}")]
        public List<ProductC> GetProductsByPage(int offset)
        {

            return dbUtilityLT2019.GetProductsByPage(offset);
        }
    }
}
