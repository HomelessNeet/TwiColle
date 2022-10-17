using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TwiColle.Models;
using System.Text;

namespace TwiColle.Controllers
{
    public class ArtistController : ApiController
    {
        /// <summary>
        /// Get all Artists
        /// </summary>
        public HttpResponseMessage Get()
        {
            using (TweetEntities db = new TweetEntities())
            {
                var query = db.Artist.Select(a => a.Name).ToList();
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, query);
                return response;
            }
        }
        /// <summary>
        /// Search by Artist Name
        /// </summary>
        public HttpResponseMessage Get(string name)
        {
            using (TweetEntities db = new TweetEntities())
            {
                HttpResponseMessage response;
                var query = db.Photo.Where(p => p.Artist.Name == name).Select(p => new PhotoData
                {
                    Id = p.Id,
                    Artist = p.Artist.Name,
                    Source = p.Source,
                    Tweet = p.Tweet,
                    Tag = p.Tag.Select(t => t.Name).ToList()
                }).ToList();
                if (query.Any())
                {
                    response = Request.CreateResponse(HttpStatusCode.OK, query);
                    return response;
                }
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.NotFound);
                    return response;
                }
            }
        }      
        /// <summary>
        /// 用於修改推特名稱
        /// </summary>
        public HttpResponseMessage Put([FromUri]string name,string newname)
        {
            using(TweetEntities db = new TweetEntities())
            {
                HttpResponseMessage response;
                Artist artist = db.Artist.SingleOrDefault(a => a.Name==name);
                if (artist != null)
                {
                    artist.Name = newname;
                    db.SaveChanges();
                    response = Request.CreateResponse(HttpStatusCode.OK, $"已成功將{name}更改為{newname}");
                    return response;
                }
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.NotFound);
                    return response;
                }
            }
        }
    }
}
