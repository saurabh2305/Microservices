using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CatalogApi.Infrastructure;
using CatalogApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace CatalogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CatalogController : ControllerBase
    {
        private CatalogContext db;
        public CatalogController(CatalogContext db)
        {
            this.db = db;
        }
        [HttpGet("",Name ="GetProducts")]
        [AllowAnonymous]
        public async Task<ActionResult<List<CatalogItem>>> GetProducts()
        {
            var result = await this.db.Catalog.FindAsync<CatalogItem>(FilterDefinition<CatalogItem>.Empty);
            return result.ToList();
        }

        [Authorize(Roles ="admin")]
        [HttpPost("", Name = "AddProduct")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public ActionResult<CatalogItem> AddProduct(CatalogItem item)
        {
            TryValidateModel(item);
            if (ModelState.IsValid)
            {
                this.db.Catalog.InsertOne(item);
                return Created("", item); //201
            }
            else
            {
                return BadRequest(ModelState);// 400
            }
            
        }

        [HttpGet("{id}",Name ="FindById")]
        public async Task<ActionResult<CatalogItem>>FindProductById(string id)
        {
            var builder = Builders<CatalogItem>.Filter;
            var filter = builder.Eq("Id", id);
            var result = await db.Catalog.FindAsync(filter);
            var item = result.FirstOrDefault();
            if(item== null)
            {
                return NotFound(); // not found status code 400
            }
            else
            {
                return Ok(item);  // Not found status code 200
            }
        }

        //[HttpPost("product")]
        //public ActionResult<CatalogItem>AddProduct()
        //{
            
        //    var imageName = SaveimageToLocal(Request.Form.Files[0]);
        //    var catologItem = new CatalogItem()
        //    {
        //        Name = Request.Form["name"],
        //        Price = double.Parse(Request.Form["price"]),
        //        Quantity = Int32.Parse(Request.Form["quantity"]),
        //        ManufacturingDate = DateTime.Parse(Request.Form["manufacturingDate"]),
        //        ReorderLevel = Int32.Parse(Request.Form["reorderLevel"]),
        //    };
        //    db.Catalog.InsertOne(catalogItem);
        //    return catalogItem;
        //    }
        //[NonAction]
        //private string SaveimageToLocal(IFormFile image)
        //{
        //    var iamageName = $"{Guid.NewGuid()}_{image.FileName}";
        //    var image = Request.Form.Files[0];

        //    var catalogItem = new CatalogItem()
        //    {
        //        Name = Request.Form["name"],
        //        Price = double.Parse(Request.Form["price"]),
        //        Quantity = Int32.Parse(Request.Form["quantity"]),
        //        ReorderLevel = Int32.Parse(Request.Form["ReorderLevel"]),
        //        ManufacturingDate = DateTime.Parse(Request.Form["manufacturingDate"]),
        //        Vendors = new List<Vendor>(),
        //        ImageUrl = imageName
        //    };
           

        //    var dirName = Path.Combine(Directory.GetCurrentDirectory(), "Images");
        //    if (!Directory.Exists(dirName))
        //    {
        //        Directory.CreateDirectory(dirName);
        //    }
        //    var filePath = Path.Combine(dirName, imageName);
        //    using (FileStream fs = new FileStream(filePath, FileMode.Create))
        //    {
        //        image.CopyTo(fs);
        //    }
        }
      
    
}