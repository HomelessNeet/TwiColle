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
    public class TagController : ApiController
    {
        /// <summary>
        /// Get all Tags
        /// </summary>
        public HttpResponseMessage Get()
        {
            using (TweetEntities db = new TweetEntities())
            {
                var query = db.Tag.Select(t => t.Name).ToList();
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, query);
                return response;
            }
        }

        /// <summary>
        /// Search by Tag Name
        /// </summary>
        public HttpResponseMessage Get(string name)
        {
            using (TweetEntities db = new TweetEntities())
            {
                HttpResponseMessage response;
                var query = db.Photo.Where(p => p.Tag.Any(t => t.Name == name)).Select(p => new PhotoData
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
        /// 為Photo新增Tag
        /// </summary>
        public HttpResponseMessage Post([FromBody]PhotoTag pt)
        {
            using (TweetEntities db = new TweetEntities())
            {
                HttpResponseMessage response;
                Photo photo = db.Photo.SingleOrDefault(p => p.Id == pt.PhotoId);
                if (photo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.NotFound);
                    return response;
                }
                Tag tag = db.Tag.SingleOrDefault(t => t.Name == pt.TagName);
                if (tag != null)       //資料表為複合主鍵因此省略Tag重複檢查
                {
                    photo.Tag.Add(tag);
                    db.SaveChanges();
                    response = Request.CreateResponse(HttpStatusCode.OK, $"已加入標籤{pt.TagName}");
                    return response;
                }
                else
                {
                    Tag newTag = new Tag { Name = pt.TagName };
                    db.Tag.Add(newTag);
                    photo.Tag.Add(newTag);
                    db.SaveChanges();
                    response = Request.CreateResponse(HttpStatusCode.OK, $"已加入新標籤{pt.TagName}");
                    return response;
                }
            }
        }       
        
        public HttpResponseMessage Delete([FromBody] PhotoTag pt)
        {
            using (TweetEntities db = new TweetEntities())
            {
                HttpResponseMessage response;
                Photo photo = db.Photo.SingleOrDefault(p => p.Id == pt.PhotoId);
                Tag tag = db.Tag.SingleOrDefault(t => t.Name == pt.TagName);
                if (photo == null || tag == null) 
                {
                    response = Request.CreateResponse(HttpStatusCode.NotFound);
                    return response;
                }
                else
                {
                    photo.Tag.Remove(tag);
                    db.SaveChanges();
                    if (!tag.Photo.Any())         //若沒有關聯的Photo則刪除該Tag
                    {
                        db.Tag.Remove(tag);
                        db.SaveChanges();
                    }
                    response = Request.CreateResponse(HttpStatusCode.OK, $"已將{pt.TagName}標籤移除");
                    return response;
                }

            }
        }
    }
}
