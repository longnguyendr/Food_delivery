using Food_delivery.Models.Data;
using Food_delivery.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Food_delivery.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManagePagesController : Controller
    {
       
        // GET: Admin/ManagePages
        public ActionResult Index()
        {
            //Declare list of PageVM
            List<PageVM> pagesList;
            //Init the list
            using (Food db = new Food())
            {
                pagesList = db.ManagePages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();
            }
                return View(pagesList);
        }

        //GET: Admin/ManagePages/AddPage
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }
        //POST: Admin/ManagePages/AddPage
        [HttpPost]
        public ActionResult AddPage(PageVM Model)
        {
            //Check Model state
            if(!ModelState.IsValid)
            {
                return View(Model);
            }
            using (Food db = new Food())
            {
                //Declare slug
                string slug;
                //Init pageDTO
                PageDTO dto= new PageDTO();
                // DTO Title
                dto.Title = Model.Title;
                //Check for and set slug if need be
                if (string.IsNullOrWhiteSpace(Model.Slug))
                {
                    slug = Model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = Model.Slug.Replace(" ", "-").ToLower();
                }
                //Ensure Title and Slug are unique
                if(db.ManagePages.Any(x => x.Title == Model.Title) || db.ManagePages.Any(x=>x.Slug == slug))
                {
                    ModelState.AddModelError("", "Title or slug already exists");
                    return View(Model);
                }
                //Data Transfer Objects to rest
                dto.Slug = slug;
                dto.Body = Model.Body;
                dto.HasSidebar = Model.HasSidebar;
                dto.Sorting = 100;
                //Save DTO
                db.ManagePages.Add(dto);
                db.SaveChanges();
            }

            //Set TempData message
            TempData["SM"] = "Successful Add a new Page!";
            //Redirect

            return RedirectToAction("AddPage");
        }

        //GET: Admin/ManagePages/EditPage/id
        [HttpGet]
        public ActionResult EditPage(int id)
        {
            //Declare PageVM
            PageVM Model;
            using (Food db = new Food())
            {
                //Get the page
                PageDTO dto = db.ManagePages.Find(id);
                //Confirm page exists
                if (dto == null)
                {
                    return Content("Page does not exists!!");
                }
                //init pageVM
                Model = new PageVM(dto);
            }
            return View(Model);
        }

        //POST: Admin/ManagePages/EditPage/id
        [HttpPost]
        public ActionResult EditPage(PageVM Model)
        {
            //Check model state
            if (!ModelState.IsValid)
            {
                return View(Model);
            }

            using (Food db = new Food())
            {
                //Get ID 
                int id = Model.Id;
                //Declare slug
                string slug = "Home";
                //Get the Page
                PageDTO dto = db.ManagePages.Find(id);
                //DTO the title
                dto.Title = Model.Title;
                //Check for slug and set if it need 
                if (Model.Slug != "Home")
                {
                    if(string.IsNullOrWhiteSpace(Model.Slug))
                    {
                        slug = Model.Title.Replace(" ", "-").ToLower();
                    }
                    else
                    {
                        slug = Model.Slug.Replace(" ", "-").ToLower();
                    }
                }
                //Ensure title and slug are unique
                if (db.ManagePages.Where(x => x.Id != id).Any(x => x.Title == Model.Title) ||  
                    db.ManagePages.Where(x => x.Id != id).Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "Title or Slug already exists!!!");
                    return View(Model);
                }
                //DTO the rest
                dto.Slug = slug;
                dto.Body = Model.Body;
                dto.HasSidebar = Model.HasSidebar;
                //save to DTO
                db.SaveChanges();
            }
            //Set Tempdata message
            TempData["SM"] = "Successfully Edit the Page";
            //Redirect
            return RedirectToAction("EditPage");
        }

        //GET: Admin/ManagePages/PageDetail
        public ActionResult PageDetail(int id)
        {
            //Declare PageVM
            PageVM Model;

            using (Food db = new Food())
            {
                //Get the Page
                PageDTO dto = db.ManagePages.Find(id);
                
                //Confirm the page exists
                if(dto == null)
                {
                    return Content("The Page does not exists!!");
                }

                //init PageVM
                Model = new PageVM(dto);
            }

            return View(Model);
        }

        //GET: Admin/ManagePages/DeletePage
        public ActionResult DeletePage(int id)
        {
            using (Food db = new Food())
            {
                //Get the page 
                PageDTO dto = db.ManagePages.Find(id);
                //Remove the page
                db.ManagePages.Remove(dto);
                //Save
                db.SaveChanges();
            }
            //Redirect
            return RedirectToAction("Index");
        }

        //POST: Admin/ManagePages/ReorderPages
        [HttpPost]
        public void ReorderPages(int[] id)
        {
            using (Food db = new Food())
            {
                // Set initial count
                int count = 1;

                // Declare PageDTO
                PageDTO dto;

                // Set sorting for each page
                foreach (var pageId in id)
                {
                    dto = db.ManagePages.Find(pageId);
                    dto.Sorting = count;

                    db.SaveChanges();

                    count++;
                }
            }

        }

        // GET: Admin/Pages/EditSidebar
        [HttpGet]
        public ActionResult EditSidebar()
        {
            // Declare model
            SidebarVM model;

            using (Food db = new Food())
            {
                // Get the DTO
                SidebarDTO dto = db.Sidebar.Find(1);

                // Init model
                model = new SidebarVM(dto);
            }

            // Return view with model
            return View(model);
        }

        // POST: Admin/Pages/EditSidebar
        [HttpPost]
        public ActionResult EditSidebar(SidebarVM model)
        {
            using (Food db = new Food())
            {
                // Get the DTO
                SidebarDTO dto = db.Sidebar.Find(1);

                // DTO the body
                dto.Body = model.Body;

                // Save
                db.SaveChanges();
            }

            // Set TempData message
            TempData["SM"] = "You have edited the sidebar!";

            // Redirect
            return RedirectToAction("EditSidebar");
        }

    }
}