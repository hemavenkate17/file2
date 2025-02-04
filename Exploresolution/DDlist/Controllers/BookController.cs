﻿using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using DDlist.Models;

namespace DDlist.Controllers
{
    public class BookController : Controller
    {
        // GET: Book
        public ActionResult Index()
        {
            BookModel bkm = new BookModel();
            bkm.Books = PopulateBooks();
            return View(bkm);
        }
        [HttpPost]
        public ActionResult Index(BookModel bkm)
        {
            bkm.Books = PopulateBooks();
            var selectedItem = bkm.Books.Find(p => p.Value == bkm.BookId.ToString());
            if (selectedItem != null)
            {
                selectedItem.Selected = true;
                ViewBag.Message = "Title: " + selectedItem.Text;
                ViewBag.Message += "\\nQuantity " + bkm.Quantity;
            }
            return View(bkm);
        }
        private static List<SelectListItem> PopulateBooks()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string constr = "data source = LAPTOP-FUHQ3D30\\SQLEXPRESS; database = BookDb; integrated security = true";
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "select Title , BookID from tbl_Books";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            items.Add(new SelectListItem
                            {
                                Text = sdr["Title"].ToString(),
                                Value = sdr["BookID"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            return items;
        }
    }
}