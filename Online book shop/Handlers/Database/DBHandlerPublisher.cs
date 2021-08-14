using Online_book_shop.Handlers.Business;
using Online_book_shop.Handlers.Search;
using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Database
{
    public class DBHandlerPublisher
    {
        public static Publisher SavePublisher(Publisher publisher)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    ctx.Publishers.Add(publisher);
                    if (ctx.SaveChanges() > 0 && BusinessHandlerConfigurations.GetLUCENE_AUTO_UPDATE())
                    {
                        SearchHandler.UpdateSearchIndex(publisher, ObjectTypes.Publisher);
                    }
                }
                return publisher;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public static List<Publisher> GetPublishers()
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var Publishers = ctx.Publishers.OrderBy(author => author.Name).ToList();
                    foreach (var a in Publishers)
                    {
                        a.ProfileImage = ctx.Medias.Where(media => media.Id == a.ProfilePictureMediaId).FirstOrDefault();
                    }
                    return Publishers;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static Publisher UpdatePublisher(Publisher publisher)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    Publisher a = ctx.Publishers.Where(x => x.Id == publisher.Id).FirstOrDefault();
                    if (a != null)
                    {
                        a.ContactNumber = publisher.ContactNumber;
                        a.Description = publisher.Description;
                        a.Email = publisher.Email;
                        a.ExternalURL = publisher.ExternalURL;
                        a.Name = publisher.Name;
                        a.LocalName = publisher.LocalName;
                        if (publisher.ProfilePictureMediaId > 0)
                        {
                            a.ProfilePictureMediaId = publisher.ProfilePictureMediaId;
                        }
                        a.UpdatedBy = publisher.UpdatedBy;
                        a.UpdatedDate = publisher.UpdatedDate;
                        a.FriendlyName = publisher.FriendlyName;
                        ctx.SaveChanges();
                        if (BusinessHandlerConfigurations.GetLUCENE_AUTO_UPDATE())
                        {
                            SearchHandler.UpdateSearchIndex(a, ObjectTypes.Publisher);
                        }
                    }

                }
                return publisher;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static Publisher GetPublisher(int id)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var author = ctx.Publishers.Where(a => a.Id == id).FirstOrDefault();
                    if (author != null)
                    {
                        author.ProfileImage = ctx.Medias.Where(media => media.Id == author.ProfilePictureMediaId).FirstOrDefault();
                    }
                    return author;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        internal static bool ShowHide(int id, bool option)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    Publisher publisher = ctx.Publishers.Where(x => x.Id == id).FirstOrDefault();
                    if (publisher != null)
                    {
                        publisher.isDeleted = option;
                        publisher.UpdatedDate = DateTime.UtcNow;
                        publisher.UpdatedBy = BusinessHandlerAuthor.GetLoginUserId();
                        if (ctx.SaveChanges() > 0)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}