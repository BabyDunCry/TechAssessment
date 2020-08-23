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
        private string WorkingKey = "7acLhVlqauztVPxar6gDz+u/tg6hrOgNjdg6I/+ecKU=";

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

        // POST: api/PosttStuff
        [ResponseType(typeof(Models.Obj.CreateStuff.responseData))]
        public IHttpActionResult PosttStuff(Models.Obj.CreateStuff.requestData requestData)
        {
            //Signaure string 
            //FirstName  + LastName + Position 

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

                var responseResult = new Models.Obj.CreateStuff.responseData();
                responseResult.FirstName = tStuff.FirstName;
                responseResult.LastName = tStuff.LastName;
                responseResult.ID = tStuff.id;
                responseResult.JoinedData = tStuff.JoinedDate;
                responseResult.Position = tStuff.Position;

                var encryptstring = responseResult.ID + responseResult.FirstName + responseResult.JoinedData;
                responseResult.signature = Utils.AesEncryption(encryptstring, WorkingKey);
                var testde = Utils.AesDecryption(responseResult.signature, WorkingKey);

                return CreatedAtRoute("DefaultApi", new { id = tStuff.id }, responseResult);
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