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
    public class PhotoController : ApiController
    {
        public HttpResponseMessage Get()
        {
            using (TweetEntities db = new TweetEntities())
            {
                var query = db.Photo.Select(p => new PhotoData
                {
                    Id = p.Id,
                    Artist = p.Artist.Name,
                    Source = p.Source,
                    Tweet = p.Tweet,
                    Tag = p.Tag.Select(t => t.Name).ToList()
                }).ToList();
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, query);
                return response;
            }
        }
        
        public HttpResponseMessage Post([FromBody]InputData data)
        {
            using (TweetEntities db = new TweetEntities())
            {
                HttpResponseMessage response;
                if (db.Photo.Any(p => p.Source == data.Source))     //檢查重複內容
                {
                    response = Request.CreateResponse(HttpStatusCode.Conflict, "重複內容");
                    return response;
                }
                data.Analyze();
                Photo photo = new Photo
                {
                    Source = data.Source,
                    Tweet = data.Tweet,
                    Date = DateTime.Parse(data.Date_8601)
                };
                Artist artist = db.Artist.SingleOrDefault(a => a.Name == data.Artist);
                if (artist != null)    //檢查Artist是否存在,否則新增
                {
                    photo.Artist = artist;
                }
                else
                {
                    Artist newArtist = new Artist { Name = data.Artist };
                    db.Artist.Add(newArtist);
                    photo.Artist = newArtist;
                }
                foreach(string s in data.Tag)
                {
                    Tag tag = db.Tag.SingleOrDefault(t => t.Name == s);
                    if (tag != null)       //檢查Tag是否存在,否則新增
                    {
                        photo.Tag.Add(tag);
                    }
                    else
                    {
                        Tag newTag = new Tag { Name = s };
                        db.Tag.Add(newTag);
                        photo.Tag.Add(newTag);
                    }
                }
                db.Photo.Add(photo);
                db.SaveChanges();
                response = Request.CreateResponse(HttpStatusCode.OK, "加入完成");
                return response;
            }
        }
                
        public HttpResponseMessage Delete(int id)
        {
            using (TweetEntities db = new TweetEntities())
            {
                HttpResponseMessage response;
                Photo photo = db.Photo.SingleOrDefault(p => p.Id == id);
                if (photo != null)          //檢查ID是否存在
                {
                    Artist artist = photo.Artist;
                    List<Tag> tags = photo.Tag.ToList();
                    db.Photo.Remove(photo);
                    db.SaveChanges();
                    if (!artist.Photo.Any())       //若沒有關聯的Photo則刪除該Artist
                    {
                        db.Artist.Remove(artist);
                    }
                    foreach(Tag tag in tags)
                    {
                        if (!tag.Photo.Any())      //若沒有關聯的Photo則刪除該Tag
                        {
                            db.Tag.Remove(tag);
                        }
                    }
                    db.SaveChanges();
                    response = Request.CreateResponse(HttpStatusCode.OK, "刪除成功");
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
