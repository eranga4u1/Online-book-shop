using Newtonsoft.Json;
using Online_book_shop.Handlers.Business;
using Online_book_shop.Models;
using Online_book_shop.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Online_book_shop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ConfigurationController : Controller
    {

        // GET: Admin/Configuration
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UpdateBannerImages()
        {
            List<Media> banners = new List<Media>();
            banners = BusinessHandlerMedia.GetHomeBanners();
            if (Request.Files != null)
            {
                foreach (string fileName in Request.Files.Keys)
                {
                    var image = Request.Files[fileName];

                    if (image.ContentLength > 0 && Path.GetExtension(image.FileName).ToLower() == ".jpg" || Path.GetExtension(image.FileName).ToLower() == ".png")
                    {
                        Media banner = BusinessHandlerMedia.CreateNewMediaEntry(image, MediaCategory.CoverImage);
                        if (banner != null)
                        {
                            if (fileName == "Banner_1")
                            {
                                if (banners.Count > 0 && banners[0] != null)
                                {
                                    banners[0] = banner;
                                }
                                else
                                {
                                    banners.Add(banner);
                                }
                            }
                            else if (banners.Count > 1 && fileName == "Banner_2")
                            {
                                if (banners[1] != null)
                                {
                                    banners[1] = banner;
                                }
                                else
                                {
                                    banners.Add(banner);
                                }
                            }
                            else if (banners.Count > 2 && fileName == "Banner_3")
                            {
                                if (banners[2] != null)
                                {
                                    banners[2] = banner;
                                }
                                else
                                {
                                    banners.Add(banner);
                                }
                            }
                            else if (banners.Count > 3 && fileName == "Banner_4")
                            {
                                if (banners[3] != null)
                                {
                                    banners[3] = banner;
                                }
                                else
                                {
                                    banners.Add(banner);
                                }
                            }
                            else
                            {
                                banners.Add(banner);
                            }

                        }
                    }
                }
                if (banners.Count > 0)
                {
                    BusinessHandlerConfigurations.AddOrUpdate(new Configuration { Key = "HOME_BANNER_IMAGES", Value = JsonConvert.SerializeObject(banners) });
                }
            }
            return RedirectToAction("Home", "Dashboard");
        }
        public ActionResult UpdateSocialNetworkURLs(SocialNetworkURLs model)
        {
            BusinessHandlerConfigurations.AddOrUpdate(new Configuration { Key = "SOCAIL_NETWORK_URLS", Value = JsonConvert.SerializeObject(model) });
            return RedirectToAction("Home", "Dashboard");
        }
        //public ActionResult AddRootMenu(MenuVM model)
        //{
        //    List<MenuVM> menus = BusinessHandlerConfigurations.GetHomeNavigationMenus();
        //    menus.Add(model);
        //    BusinessHandlerConfigurations.AddOrUpdate(new Configuration { Key = "HOME_NAVIGATION", Value = JsonConvert.SerializeObject(menus) });
        //    return RedirectToAction("Home", "Dashboard");
        //}
        //public ActionResult AddSubMenu(MenuVM model)
        //{
        //    List<MenuVM> menus = BusinessHandlerConfigurations.GetHomeNavigationMenus();
        //    foreach (MenuVM menu in menus)
        //    {
        //        if (menu.Title == model.Parent)
        //        {
        //            MenuVM menuVM = model;
        //            if (menu.SubMenus != null)
        //            {
        //                menu.SubMenus.Add(menuVM);
        //            }
        //            else
        //            {
        //                List<MenuVM> submenus = new List<MenuVM>();
        //                submenus.Add(menuVM);
        //                menu.SubMenus = submenus;
        //            }
        //        }
        //    }
        //        BusinessHandlerConfigurations.AddOrUpdate(new Configuration { Key = "HOME_NAVIGATION", Value = JsonConvert.SerializeObject(menus) });

        //        return RedirectToAction("Home", "Dashboard");
        //}
        //public ActionResult DeleteRootMenu(string id)
        //{
        //    List<MenuVM> menus = BusinessHandlerConfigurations.GetHomeNavigationMenus();
        //    var items= menus.Where(m => m.Title != id.Trim());
        //    BusinessHandlerConfigurations.AddOrUpdate(new Configuration { Key = "HOME_NAVIGATION", Value = JsonConvert.SerializeObject(items !=null?items:null) });
        //    return RedirectToAction("Home", "Dashboard");
        //}
        //public ActionResult DeleteSubMenu(string id)
        //{
        //    List<MenuVM> menus = BusinessHandlerConfigurations.GetHomeNavigationMenus();
        //    if(Request.QueryString["parent"] != null)
        //    {
        //        string parent = Request.QueryString["parent"];
        //        foreach (MenuVM menu in menus)
        //        {
        //            if (menu.Title == parent)
        //            {
        //                var items = menu.SubMenus.Where(m => m.Title != id.Trim()).ToList();
        //                if(items !=null && items.Count > 0)
        //                {
        //                    menu.SubMenus = items;
        //                }
        //                else
        //                {
        //                    menu.SubMenus = null;
        //                }                        
        //            }
        //        }
        //    }          
        //    BusinessHandlerConfigurations.AddOrUpdate(new Configuration { Key = "HOME_NAVIGATION", Value = JsonConvert.SerializeObject(menus != null ? menus : null) });
        //    return RedirectToAction("Home", "Dashboard");
        //}

        //public ActionResult AddFooterMenu(MenuVM model)
        //{
        //    List<MenuVM> menus = BusinessHandlerConfigurations.GetHomeFooterMenus();
        //    menus.Add(model);
        //    BusinessHandlerConfigurations.AddOrUpdate(new Configuration { Key = "HOME_FOOTER_NAVIGATION", Value = JsonConvert.SerializeObject(menus) });
        //    return RedirectToAction("Home", "Dashboard");
        //}
        //public ActionResult AddSubFooterMenu(MenuVM model)
        //{
        //    List<MenuVM> menus = BusinessHandlerConfigurations.GetHomeFooterMenus();
        //    foreach (MenuVM menu in menus)
        //    {
        //        if (menu.Title == model.Parent)
        //        {
        //            MenuVM menuVM = model;
        //            if (menu.SubMenus != null)
        //            {
        //                menu.SubMenus.Add(menuVM);
        //            }
        //            else
        //            {
        //                List<MenuVM> submenus = new List<MenuVM>();
        //                submenus.Add(menuVM);
        //                menu.SubMenus = submenus;
        //            }
        //        }
        //    }
        //    BusinessHandlerConfigurations.AddOrUpdate(new Configuration { Key = "HOME_FOOTER_NAVIGATION", Value = JsonConvert.SerializeObject(menus) });

        //    return RedirectToAction("Home", "Dashboard");
        //}
        //public ActionResult DeleteFooterMenu(string id)
        //{
        //    List<MenuVM> menus = BusinessHandlerConfigurations.GetHomeFooterMenus();
        //    var items = menus.Where(m => m.Title != id.Trim());
        //    BusinessHandlerConfigurations.AddOrUpdate(new Configuration { Key = "HOME_FOOTER_NAVIGATION", Value = JsonConvert.SerializeObject(items != null ? items : null) });
        //    return RedirectToAction("Home", "Dashboard");
        //}
        //public ActionResult DeleteFooterSubMenu(string id)
        //{
        //    List<MenuVM> menus = BusinessHandlerConfigurations.GetHomeFooterMenus();
        //    if (Request.QueryString["parent"] != null)
        //    {
        //        string parent = Request.QueryString["parent"];
        //        foreach (MenuVM menu in menus)
        //        {
        //            if (menu.Title == parent)
        //            {
        //                var items = menu.SubMenus.Where(m => m.Title != id.Trim()).ToList();
        //                if (items != null && items.Count > 0)
        //                {
        //                    menu.SubMenus = items;
        //                }
        //                else
        //                {
        //                    menu.SubMenus = null;
        //                }
        //            }
        //        }
        //    }
        //    BusinessHandlerConfigurations.AddOrUpdate(new Configuration { Key = "HOME_FOOTER_NAVIGATION", Value = JsonConvert.SerializeObject(menus != null ? menus : null) });
        //    return RedirectToAction("Home", "Dashboard");
        //}

        public ActionResult UpdateNavigation(JsonMenu model)
        {
            BusinessHandlerConfigurations.AddOrUpdate(new Configuration { Key = "HOME_TOP_NAVIGATION", Value = model.jsonString });
            return Redirect("/Admin/Menu");
        }
        public ActionResult UpdateFooterNavigation(JsonMenu model)
        {
            BusinessHandlerConfigurations.AddOrUpdate(new Configuration { Key = "HOME_FOOTER_NAVIGATION", Value = model.jsonString });
            return Redirect("/Admin/Menu/Footer");
        }
        public ActionResult UpdateUpdateContactUs(ContactVM model)
        {
            BusinessHandlerConfigurations.AddOrUpdate(new Configuration { Key = "CONTACTUS-PAGE", Value = JsonConvert.SerializeObject(model) });
            return Redirect("/Admin/Dashboard/Home");
        }
        public ActionResult UpdateAboutUs(AboutUs model)
        {
            BusinessHandlerConfigurations.AddOrUpdate(new Configuration { Key = "ABOUT-US", Value = model.Summary });
            return Redirect("/Admin/Dashboard/Home");
        }

        public ActionResult Localization()
        {
            List<ContactNumber> Obj__contactNumbers = new List<ContactNumber>();
            List<EmailAddress> Obj__emailAddress = new List<EmailAddress>();
            List<MusesAddress> Obj_address = new List<MusesAddress>();
            List<PaymentMethod> Obj_payment_option = new List<PaymentMethod>();
            InstantMessage Obj_instant_message = new InstantMessage();
            List<ItemSaleType> Obj_sales_types = new List<ItemSaleType>();
            List<DeliveryMethod> Obj_delivery_types = new List<DeliveryMethod>();
            List<string> Obj_book_property_types = new List<string>();
            double Obj_clientTimeOffSet = 0;

            Configuration conf_contactNumbers = BusinessHandlerConfigurations.GetConfigByKey("CONTACT_NUMBERS");
            Configuration conf_emailAddress = BusinessHandlerConfigurations.GetConfigByKey("EMAIL_ADDRESS");
            Configuration conf_address = BusinessHandlerConfigurations.GetConfigByKey("ADDRESS");
            Configuration conf_payment_option = BusinessHandlerConfigurations.GetConfigByKey("PAYMENT_METHODS");

            Configuration instrant_message = BusinessHandlerConfigurations.GetConfigByKey("INSTANT_MESSAGE");

            Configuration sales_types = BusinessHandlerConfigurations.GetConfigByKey("SALES_TYPES");

            Configuration delivery_types = BusinessHandlerConfigurations.GetConfigByKey("DELIVERY_TYPES");

            Configuration book_property_types = BusinessHandlerConfigurations.GetConfigByKey("BOOK_PROPERTY_TYPES");
            Configuration clientTimeOffSet = BusinessHandlerConfigurations.GetConfigByKey("TIME_OFFSET");

            if (clientTimeOffSet != null && !string.IsNullOrEmpty(clientTimeOffSet.Value))
            {
                Obj_clientTimeOffSet = JsonConvert.DeserializeObject<double>(clientTimeOffSet.Value);
            }
            else
            {
                Obj_clientTimeOffSet = 5.5;
                BusinessHandlerConfigurations.AddOrUpdate(new Configuration { Key = "TIME_OFFSET", Value = JsonConvert.SerializeObject(Obj_clientTimeOffSet) });
            }

            if (book_property_types != null && !string.IsNullOrEmpty(book_property_types.Value))
            {
                Obj_book_property_types = JsonConvert.DeserializeObject<List<string>>(book_property_types.Value);
            }
            else
            {
                Obj_book_property_types.Add("Normal Cover");
                Obj_book_property_types.Add("Hard Bind Cover");
                Obj_book_property_types.Add("Other");
                BusinessHandlerConfigurations.AddOrUpdate(new Configuration { Key = "BOOK_PROPERTY_TYPES", Value = JsonConvert.SerializeObject(Obj_book_property_types) });
            }
            if (conf_contactNumbers != null && !string.IsNullOrEmpty(conf_contactNumbers.Value))
            {
                Obj__contactNumbers = JsonConvert.DeserializeObject<List<ContactNumber>>(conf_contactNumbers.Value);
            }
            if (conf_emailAddress != null && !string.IsNullOrEmpty(conf_emailAddress.Value))
            {
                Obj__emailAddress = JsonConvert.DeserializeObject<List<EmailAddress>>(conf_emailAddress.Value);
            }
            if (conf_address != null && !string.IsNullOrEmpty(conf_address.Value))
            {
                Obj_address = JsonConvert.DeserializeObject<List<MusesAddress>>(conf_address.Value);
            }
            if (instrant_message != null && !string.IsNullOrEmpty(instrant_message.Value))
            {
                Obj_instant_message = JsonConvert.DeserializeObject<InstantMessage>(instrant_message.Value);
            }
            if (conf_payment_option != null && !string.IsNullOrEmpty(conf_payment_option.Value))
            {
                Obj_payment_option = JsonConvert.DeserializeObject<List<PaymentMethod>>(conf_payment_option.Value);
            }
            else
            {
                PaymentMethod p1 = new PaymentMethod { EnumId = 0, Title = "Cash_On_Delivery", Message = "", isEnable = true };
                PaymentMethod p2 = new PaymentMethod { EnumId = 1, Title = "Online_Payment", Message = "", isEnable = true };
                PaymentMethod p3 = new PaymentMethod { EnumId = 2, Title = "Bank_Deposit", Message = "", isEnable = true };
                PaymentMethod p4 = new PaymentMethod { EnumId = 3, Title = "Ez_cash", Message = "", isEnable = true };
                PaymentMethod p5 = new PaymentMethod { EnumId = 4, Title = "In_store_payment", Message = "", isEnable = true };
                Obj_payment_option.Add(p1);
                Obj_payment_option.Add(p2);
                Obj_payment_option.Add(p3);
                Obj_payment_option.Add(p4);
                Obj_payment_option.Add(p5);
                BusinessHandlerConfigurations.AddOrUpdate(new Configuration { Key = "PAYMENT_METHODS", Value = JsonConvert.SerializeObject(Obj_payment_option) });
            }
            if (sales_types != null && !string.IsNullOrEmpty(sales_types.Value))
            {
                Obj_sales_types = JsonConvert.DeserializeObject<List<ItemSaleType>>(sales_types.Value);
            }
            else
            {
                ItemSaleType sl1 = new ItemSaleType { EnumId = 0, Title = "PreOrder", Color=""};
                ItemSaleType sl2 = new ItemSaleType { EnumId = 1, Title = "NormalSale", Color = "" };
                ItemSaleType sl3 = new ItemSaleType { EnumId = 2, Title = "OutOfPrint", Color = "" };
                //Models.ViewModel.SaleType p4 = new Models.ViewModel.SaleType { EnumId = 3, Title = "In2Days", Message = "", isEnable = true };
                Obj_sales_types.Add(sl1);
                Obj_sales_types.Add(sl2);
                Obj_sales_types.Add(sl3);

                BusinessHandlerConfigurations.AddOrUpdate(new Configuration { Key = "SALES_TYPES", Value = JsonConvert.SerializeObject(Obj_sales_types) });
            }
            if (delivery_types != null && !string.IsNullOrEmpty(delivery_types.Value))
            {
                Obj_delivery_types = JsonConvert.DeserializeObject<List<DeliveryMethod>>(delivery_types.Value);
            }
            else
            {
                DeliveryMethod dm1 = new DeliveryMethod { Id = (int)DeliveryTypes.Postal_Service, Title = "Post Service", isEnable = true };
                DeliveryMethod dm2 = new DeliveryMethod { Id = (int)DeliveryTypes.Currier_Service, Title = "Courier Service", isEnable = true };
                DeliveryMethod dm3 = new DeliveryMethod { Id = (int)DeliveryTypes.Foreign_Airmail, Title = "Foregn Air mail", isEnable = true };
                DeliveryMethod dm4 = new DeliveryMethod { Id = (int)DeliveryTypes.EMS, Title = "EMS", isEnable = true };
                DeliveryMethod dm5 = new DeliveryMethod { Id = (int)DeliveryTypes.In_Store_Pickup, Title = "In Stock Pickup", isEnable = true };
                Obj_delivery_types.Add(dm1);
                Obj_delivery_types.Add(dm2);
                Obj_delivery_types.Add(dm3);
                Obj_delivery_types.Add(dm4);
                Obj_delivery_types.Add(dm5);

                BusinessHandlerConfigurations.AddOrUpdate(new Configuration
                {
                    Key = "DELIVERY_TYPES",
                    Value = JsonConvert.SerializeObject(Obj_delivery_types)
                });
            }

            ViewBag.ContactNumbers = Obj__contactNumbers;
            ViewBag.EmailAddress = Obj__emailAddress;
            ViewBag.Address = Obj_address;
            ViewBag.PaymentMethods = Obj_payment_option;
            ViewBag.InstantMessage = Obj_instant_message;
            ViewBag.Sale_Types = Obj_sales_types;
            ViewBag.Delivery_Types = Obj_delivery_types;
            ViewBag.BookPropertyTypes= Obj_book_property_types;

            return View();
        }
        public ActionResult UpdateDeliveryMethod(int Id, bool isEnable)
        {
            List<DeliveryMethod> Obj_delivery_types = new List<DeliveryMethod>();
            Configuration delivery_types = BusinessHandlerConfigurations.GetConfigByKey("DELIVERY_TYPES");
            if (delivery_types != null && !string.IsNullOrEmpty(delivery_types.Value))
            {
                Obj_delivery_types = JsonConvert.DeserializeObject<List<DeliveryMethod>>(delivery_types.Value);
                var item = Obj_delivery_types.Where(x => x.Id == Id).FirstOrDefault();
                Obj_delivery_types.Remove(item);
                if (item != null)
                {
                    item.isEnable = isEnable;
                }
                Obj_delivery_types.Add(item);
                BusinessHandlerConfigurations.AddOrUpdate(new Configuration
                {
                    Key = "DELIVERY_TYPES",
                    Value = JsonConvert.SerializeObject(Obj_delivery_types)

                });
            }
            return Redirect("/Admin/Configuration/Localization#delivery_methods"); 
        }

        [HttpPost]
        public JsonResult AddContactNumber(ContactNumber model)
        {
            JsonResult jr = new JsonResult();
            List<ContactNumber> Obj__contactNumbers = new List<ContactNumber>();
            try
            {
                Configuration conf_contactNumbers = BusinessHandlerConfigurations.GetConfigByKey("CONTACT_NUMBERS");
                if (conf_contactNumbers != null && !string.IsNullOrEmpty(conf_contactNumbers.Value))
                {
                    Obj__contactNumbers = JsonConvert.DeserializeObject<List<ContactNumber>>(conf_contactNumbers.Value);
                }
                if (Obj__contactNumbers != null)
                {
                    Obj__contactNumbers.Add(model);
                }
                else
                {
                    Obj__contactNumbers = new List<ContactNumber>();
                    Obj__contactNumbers.Add(model);
                }

                jr.Data = BusinessHandlerConfigurations.AddOrUpdate(new Configuration { Key = "CONTACT_NUMBERS", Value = JsonConvert.SerializeObject(Obj__contactNumbers) });

            }
            catch (Exception ex)
            {

            }
            return jr;
        }
        [HttpPost]
        public JsonResult AddEmailAddress(EmailAddress model)
        {
            JsonResult jr = new JsonResult();
            List<EmailAddress> Obj__emailAddress = new List<EmailAddress>();

            try
            {
                Configuration conf__emailAddress = BusinessHandlerConfigurations.GetConfigByKey("EMAIL_ADDRESS");
                if (conf__emailAddress != null && !string.IsNullOrEmpty(conf__emailAddress.Value))
                {
                    Obj__emailAddress = JsonConvert.DeserializeObject<List<EmailAddress>>(conf__emailAddress.Value);
                }
                if (Obj__emailAddress != null)
                {
                    Obj__emailAddress.Add(model);
                }
                else
                {
                    Obj__emailAddress = new List<EmailAddress>();
                    Obj__emailAddress.Add(model);
                }

                jr.Data = BusinessHandlerConfigurations.AddOrUpdate(new Configuration { Key = "EMAIL_ADDRESS", Value = JsonConvert.SerializeObject(Obj__emailAddress) });
            }
            catch (Exception ex)
            {

            }
            return jr;
        }
        [HttpPost]
        public JsonResult AddContactAddress(MusesAddress model)
        {
            JsonResult jr = new JsonResult();
            List<MusesAddress> Obj_address = new List<MusesAddress>();
            try
            {
                Configuration conf_contactAddresss = BusinessHandlerConfigurations.GetConfigByKey("ADDRESS");
                if (conf_contactAddresss != null && !string.IsNullOrEmpty(conf_contactAddresss.Value))
                {
                    Obj_address = JsonConvert.DeserializeObject<List<MusesAddress>>(conf_contactAddresss.Value);
                }
                if (Obj_address != null)
                {
                    Obj_address.Add(model);
                }
                else
                {
                    Obj_address = new List<MusesAddress>();
                    Obj_address.Add(model);
                }

                jr.Data = BusinessHandlerConfigurations.AddOrUpdate(new Configuration { Key = "ADDRESS", Value = JsonConvert.SerializeObject(Obj_address) });
            }
            catch (Exception ex)
            {

            }
            return jr;
        }

        [HttpPost]
        public JsonResult RemoveContactNumber(ContactNumber model)
        {
            JsonResult jr = new JsonResult();
            List<ContactNumber> Obj__contactNumbers = new List<ContactNumber>();
            try
            {
                Configuration conf_contactNumbers = BusinessHandlerConfigurations.GetConfigByKey("CONTACT_NUMBERS");
                if (conf_contactNumbers != null && !string.IsNullOrEmpty(conf_contactNumbers.Value))
                {
                    Obj__contactNumbers = JsonConvert.DeserializeObject<List<ContactNumber>>(conf_contactNumbers.Value);
                }
                if (Obj__contactNumbers != null)
                {
                    var item = Obj__contactNumbers.Where(x => x.NumberValue == model.NumberValue).FirstOrDefault();
                    Obj__contactNumbers.Remove(item);
                }


                jr.Data = BusinessHandlerConfigurations.AddOrUpdate(new Configuration { Key = "CONTACT_NUMBERS", Value = JsonConvert.SerializeObject(Obj__contactNumbers) });

            }
            catch (Exception ex)
            {

            }
            return jr;
        }
        [HttpPost]
        public JsonResult RemoveEmailAddress(EmailAddress model)
        {
            JsonResult jr = new JsonResult();
            List<EmailAddress> Obj__emailAddress = new List<EmailAddress>();

            try
            {
                Configuration conf__emailAddress = BusinessHandlerConfigurations.GetConfigByKey("EMAIL_ADDRESS");
                if (conf__emailAddress != null && !string.IsNullOrEmpty(conf__emailAddress.Value))
                {
                    Obj__emailAddress = JsonConvert.DeserializeObject<List<EmailAddress>>(conf__emailAddress.Value);
                }
                if (Obj__emailAddress != null)
                {
                    var item = Obj__emailAddress.Where(x => x.EmailValue == model.EmailValue).FirstOrDefault();
                    Obj__emailAddress.Remove(item);
                }

                jr.Data = BusinessHandlerConfigurations.AddOrUpdate(new Configuration { Key = "EMAIL_ADDRESS", Value = JsonConvert.SerializeObject(Obj__emailAddress) });
            }
            catch (Exception ex)
            {

            }
            return jr;
        }
        [HttpPost]
        public JsonResult RemoveContactAddress(MusesAddress model)
        {
            JsonResult jr = new JsonResult();
            List<MusesAddress> Obj_address = new List<MusesAddress>();
            try
            {
                Configuration conf_contactAddresss = BusinessHandlerConfigurations.GetConfigByKey("ADDRESS");
                if (conf_contactAddresss != null && !string.IsNullOrEmpty(conf_contactAddresss.Value))
                {
                    Obj_address = JsonConvert.DeserializeObject<List<MusesAddress>>(conf_contactAddresss.Value);
                }
                if (Obj_address != null)
                {
                    var item = Obj_address.Where(x => x.AddressValue == model.AddressValue).FirstOrDefault();
                    Obj_address.Remove(item);
                }

                jr.Data = BusinessHandlerConfigurations.AddOrUpdate(new Configuration { Key = "ADDRESS", Value = JsonConvert.SerializeObject(Obj_address) });
            }
            catch (Exception ex)
            {

            }

            return jr;
        }

        [HttpPost]

        public JsonResult UpdatePaymentMethod(PaymentMethod model)
        {
            JsonResult jr = new JsonResult();
            List<PaymentMethod> Obj_payment_option = new List<PaymentMethod>();
            Configuration conf_payment_option = BusinessHandlerConfigurations.GetConfigByKey("PAYMENT_METHODS");
            if (conf_payment_option != null && !string.IsNullOrEmpty(conf_payment_option.Value))
            {
                Obj_payment_option = JsonConvert.DeserializeObject<List<PaymentMethod>>(conf_payment_option.Value);
                Obj_payment_option.Remove(Obj_payment_option.Where(x => x.EnumId == model.EnumId).FirstOrDefault());
                Obj_payment_option.Add(model);
                jr.Data = BusinessHandlerConfigurations.AddOrUpdate(new Configuration
                {
                    Key = "PAYMENT_METHODS",
                    Value = JsonConvert.SerializeObject(Obj_payment_option)
                });
            }
            return jr;
        }

        public ActionResult UpdateDeliveryCharges()
        {
            Configuration conf_district = BusinessHandlerConfigurations.GetConfigByKey("DISTRICT_SRILANKA");
            Configuration conf_countries = BusinessHandlerConfigurations.GetConfigByKey("COUNTRIES");
            ViewBag.District= JsonConvert.DeserializeObject<List<string>>(conf_district.Value).OrderBy(d => d).ToList();
            ViewBag.Countries = JsonConvert.DeserializeObject<List<string>>(conf_countries.Value).OrderBy(d => d).ToList();

            return View();
        }

        [HttpPost]
        public JsonResult UpdateSriLankaPostalCharges(DeliveryCharge model)
        {
            JsonResult jr = new JsonResult();
            if(model != null)
            {
               jr.Data= BusinessHandlerDeliveryCharges.Add(model);
            }
            return jr;
        }

        [HttpPost]
        public JsonResult RemoveDeliveryChargesCharges(DeliveryCharge model)
        {
            JsonResult jr = new JsonResult();
            if (model != null)
            {
                jr.Data = BusinessHandlerDeliveryCharges.remove(model);
            }
            return jr;
        }


        
        public JsonResult UpdateInstantMessage(InstantMessage model)
        {
            JsonResult jr = new JsonResult();
            if(BusinessHandlerConfigurations.AddOrUpdate(new Configuration
            {
                Key = "INSTANT_MESSAGE",
                Value = JsonConvert.SerializeObject(model)
            }) != null)
            {
                jr.Data = "success";
            }
            else
            {
                jr.Data = "failed";
            }
            return jr;
        }

        public ActionResult District()
        {
            Configuration conf_district = BusinessHandlerConfigurations.GetConfigByKey("DISTRICT_SRILANKA");
            ViewBag.District = JsonConvert.DeserializeObject<List<string>>(conf_district.Value).OrderBy(d=>d).ToList();
            return View();
        }

        public JsonResult AddDistrict(District model)
        {
            JsonResult jr = new JsonResult();
            Dictionary<string, string> responce = new Dictionary<string, string>();

            Configuration conf_district = BusinessHandlerConfigurations.GetConfigByKey("DISTRICT_SRILANKA");
            List<string> currentList = JsonConvert.DeserializeObject<List<string>>(conf_district.Value).OrderBy(d => d).ToList();
            if(currentList.Where(x=> x.ToLower().Trim()== model.NewName.ToLower().Trim()).Count() > 0)
            {
                responce.Add("message", "Item already exits.");
            }
            else
            {
                currentList.Add(model.NewName);
                if(BusinessHandlerConfigurations.AddOrUpdate(new Configuration
                {
                    Key = "DISTRICT_SRILANKA",
                    Value = JsonConvert.SerializeObject(currentList)
                }) !=null)
                {
                    responce.Add("message","Successfull");
                }
                else
                {
                    responce.Add("message", "Failed");
                }
                
            }
            jr.Data = responce;
            return jr;
        }

        public JsonResult RemoveDistrict(District model)
        {
            JsonResult jr = new JsonResult();
            Dictionary<string, string> responce = new Dictionary<string, string>();

            Configuration conf_district = BusinessHandlerConfigurations.GetConfigByKey("DISTRICT_SRILANKA");
            List<string> currentList = JsonConvert.DeserializeObject<List<string>>(conf_district.Value).OrderBy(d => d).ToList();
            string Obj = currentList.Where(x => x.ToLower().Trim() == model.OldName.ToLower().Trim()).FirstOrDefault();
            if (Obj != null)
            {
                currentList.Remove(Obj);
                if (BusinessHandlerConfigurations.AddOrUpdate(new Configuration
                {
                    Key = "DISTRICT_SRILANKA",
                    Value = JsonConvert.SerializeObject(currentList)
                }) !=null)
                {
                    responce.Add("message", "Successfull");
                }
                else
                {
                    responce.Add("message", "Failed");
                }
            }
            else
            {
                responce.Add("message", "Item not found.");
            }



            jr.Data = responce;
            return jr;
        }

        public JsonResult UpdateDistrict(District model)
        {
            JsonResult jr = new JsonResult();
            Dictionary<string, string> responce = new Dictionary<string, string>();

            Configuration conf_district = BusinessHandlerConfigurations.GetConfigByKey("DISTRICT_SRILANKA");
            List<string> currentList = JsonConvert.DeserializeObject<List<string>>(conf_district.Value).OrderBy(d => d).ToList();
            string Obj = currentList.Where(x => x.ToLower().Trim() == model.OldName.ToLower().Trim()).FirstOrDefault();
            if (Obj != null)
            {
                currentList.Remove(Obj);
                currentList.Add(model.NewName);
                if (BusinessHandlerConfigurations.AddOrUpdate(new Configuration
                {
                    Key = "DISTRICT_SRILANKA",
                    Value = JsonConvert.SerializeObject(currentList)
                }) !=null)
                {
                    responce.Add("message", "Successfull");
                }
                else
                {
                    responce.Add("message", "Failed");
                }
            }
            else
            {
                responce.Add("message", "Item not found.");
            }



            jr.Data = responce;
            return jr;
        }

        public ActionResult AddBookPropertyType()
        {
            string title = Request.QueryString["title"];
            //JsonResult jr = new JsonResult();
            //Dictionary<string, string> responce = new Dictionary<string, string>();
            List<string> Obj_book_property_types = new List<string>();
            Configuration book_property_types = BusinessHandlerConfigurations.GetConfigByKey("BOOK_PROPERTY_TYPES");
            if (book_property_types != null && !string.IsNullOrEmpty(book_property_types.Value))
            {
                Obj_book_property_types = JsonConvert.DeserializeObject<List<string>>(book_property_types.Value);
            }
            Obj_book_property_types.Add(title);
            BusinessHandlerConfigurations.AddOrUpdate(new Configuration { Key = "BOOK_PROPERTY_TYPES", Value = JsonConvert.SerializeObject(Obj_book_property_types) });
            //responce.Add("status","done");
            //jr.Data = responce;
            //return jr;
            return Redirect("/Admin/Configuration/Localization#property_types");
        }
        public ActionResult RemoveBookPropertyType()
        {
            string title = Request.QueryString["title"];
            List<string> Obj_book_property_types = new List<string>();
            Configuration book_property_types = BusinessHandlerConfigurations.GetConfigByKey("BOOK_PROPERTY_TYPES");
            if (book_property_types != null && !string.IsNullOrEmpty(book_property_types.Value))
            {
                Obj_book_property_types = JsonConvert.DeserializeObject<List<string>>(book_property_types.Value);
            }
            Obj_book_property_types.Remove(title);
            BusinessHandlerConfigurations.AddOrUpdate(new Configuration { Key = "BOOK_PROPERTY_TYPES", Value = JsonConvert.SerializeObject(Obj_book_property_types) });
            return Redirect("/Admin/Configuration/Localization#property_types");
        }
    }
}