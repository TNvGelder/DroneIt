using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DroneAPI.DAL;
using DroneAPI.Models;
using System.Web.Http.Cors;
using DroneAPI.Models.Database;
/**
 * @author: Henk jan Leusink
 * Controller for handling product functionality
 * */
namespace DroneAPI.Migrations
{
    public class ProductsController : ApiController
    {
        private DroneContext _db = new DroneContext();

        // GET: api/Products
        [EnableCors("*", "*", "GET")]
        public IQueryable<Product> GetProducts()
        {           
            // return all products
            return _db.Products;
        }

        // GET: api/Products/5
        [EnableCors("*", "*", "GET")]
        [ResponseType(typeof(Product))]
        public IHttpActionResult GetProduct(int id)
        {
            // find product by id in database
            Product product = _db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            Product returnProduct = new Product() { Id = product.Id, Name = product.Name };

            return Ok(returnProduct);
        }

        // PUT: api/Products/5
        [EnableCors("*", "*", "PUT")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                return BadRequest();
            }

            _db.Entry(product).State = EntityState.Modified;

            try
            {
                _db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Products
        [EnableCors("*", "*", "POST")]
        [ResponseType(typeof(Product))]
        public IHttpActionResult PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.Products.Add(product);
            _db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [EnableCors("*", "*", "DELETE")]
        [ResponseType(typeof(Product))]
        public IHttpActionResult DeleteProduct(int id)
        {
            Product product = _db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            _db.Products.Remove(product);
            _db.SaveChanges();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return _db.Products.Count(e => e.Id == id) > 0;
        }
    }
}