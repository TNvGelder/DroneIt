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

namespace DroneAPI.Controllers
{
    public class QualityChecksController : ApiController
    {
        private DroneContext db = new DroneContext();

        // GET: api/QualityChecks
        [EnableCors("*", "*", "GET")]
        public IQueryable<QualityCheck> GetQualityChecks()
        {
            return db.QualityChecks;
        }

        // GET: api/QualityChecks/5
        [ResponseType(typeof(QualityCheck))]
        public IHttpActionResult GetQualityCheck(int id)
        {
            QualityCheck qualityCheck = db.QualityChecks.Find(id);
            if (qualityCheck == null)
            {
                return NotFound();
            }

            return Ok(qualityCheck);
        }

        // PUT: api/QualityChecks/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutQualityCheck(int id, QualityCheck qualityCheck)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != qualityCheck.Id)
            {
                return BadRequest();
            }

            db.Entry(qualityCheck).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QualityCheckExists(id))
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

        // POST: api/QualityChecks
        [ResponseType(typeof(QualityCheck))]
        public IHttpActionResult PostQualityCheck(QualityCheck qualityCheck)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.QualityChecks.Add(qualityCheck);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = qualityCheck.Id }, qualityCheck);
        }

        // DELETE: api/QualityChecks/5
        [ResponseType(typeof(QualityCheck))]
        public IHttpActionResult DeleteQualityCheck(int id)
        {
            QualityCheck qualityCheck = db.QualityChecks.Find(id);
            if (qualityCheck == null)
            {
                return NotFound();
            }

            db.QualityChecks.Remove(qualityCheck);
            db.SaveChanges();

            return Ok(qualityCheck);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool QualityCheckExists(int id)
        {
            return db.QualityChecks.Count(e => e.Id == id) > 0;
        }
    }
}