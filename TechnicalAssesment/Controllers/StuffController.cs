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
using TechnicalAssesment.Models;
using TechnicalAssesment.Models.Entities;

namespace TechnicalAssesment.Controllers
{
    public class StuffController : ApiController
    {
        private TechnicalAssesmentContext db = new TechnicalAssesmentContext();
        private string WorkingKey = "TechAssessment1234567890";
        // GET: api/stuff/GetAllStuffs
        public IQueryable<tStuff> GetAllStuffs()
        {
            var authHeader = Request.Headers.Authorization;
            string authCode = "";

            if (authHeader != null && authHeader.ToString().StartsWith("Basic"))
            {
                //Extract credentials
                authCode = authHeader.ToString().Replace("Basic ","");
                //Check authcode
                if(authCode != "abcdefg01234567890")
                {
                    throw new Exception("The authorization header is not authorize");
                }
                return db.tStuffs;
            }
            else
            {
                //Handle what happens if that isn't the case
                throw new Exception("The authorization header is either empty or isn't Basic.");
            }
        }

        // GET: api/Stuff/5
        [ResponseType(typeof(tStuff))]
        public IHttpActionResult GettStuff(int id)
        {
            tStuff tStuff = db.tStuffs.Find(id);
            if (tStuff == null)
            {
                return NotFound();
            }

            return Ok(tStuff);
        }

        // PUT: api/Stuff/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PuttStuff(int id, tStuff tStuff)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tStuff.id)
            {
                return BadRequest();
            }

            db.Entry(tStuff).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tStuffExists(id))
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

        // POST: api/Stuff
        [ResponseType(typeof(Models.Obj.CreateStuff.responseData))]
        public IHttpActionResult PosttStuff(Models.Obj.CreateStuff.requestData requestData)
        {
            var authHeader = Request.Headers.Authorization;
            string authCode = "";

            if (authHeader != null && authHeader.ToString().StartsWith("Basic"))
            {
                //Extract credentials
                authCode = authHeader.ToString().Replace("Basic ", "");
                //Check authcode
                if (authCode != "abcdefg01234567890")
                {
                    throw new Exception("The authorization header is not authorize");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                tStuff tStuff = new tStuff();
                tStuff.FirstName = requestData.FirstName;
                tStuff.LastName = requestData.LastName;
                tStuff.JoinedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                tStuff.Position = requestData.Position;

                db.tStuffs.Add(tStuff);
                db.SaveChanges();

                return CreatedAtRoute("DefaultApi", new { id = tStuff.id, signature ="123213123213" }, tStuff);
            }
            else
            {
                //Handle what happens if that isn't the case
                throw new Exception("The authorization header is either empty or isn't Basic.");
            }

        }

        // DELETE: api/Stuff/5
        [ResponseType(typeof(tStuff))]
        public IHttpActionResult DeletetStuff(int id)
        {
            tStuff tStuff = db.tStuffs.Find(id);
            if (tStuff == null)
            {
                return NotFound();
            }

            db.tStuffs.Remove(tStuff);
            db.SaveChanges();

            return Ok(tStuff);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tStuffExists(int id)
        {
            return db.tStuffs.Count(e => e.id == id) > 0;
        }
    }
}