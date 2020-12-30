using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebProject.Models;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace WebProject.Controllers
{
    public class DatabaseController : Controller
    {
        Database database;
        string cacheList;
        public DatabaseController()
        {
            this.database = new Models.Database();
            this.cacheList = "personList";
        }
        public ActionResult Database()
        {
            return View();
        }

        [HttpPost]
        //Creates a database object, calls the getPersonList method to get all the data from the Person table in the people database,
        //the method returns a list of person objects
        public JsonResult GetPeople()
        {
            string jsonString;
            try
            {
                jsonString = this.database.getPersonList();
            }
            catch (InvalidOperationException e)
            {
                return Json(new { result = false, jsonStr = $"InvalidOperationException: {e.Message}" }, JsonRequestBehavior.AllowGet);
            }
            catch (ArgumentException e)
            {
                return Json(new { result = false, jsonStr = $"Argument Exception: {e.Message}" }, JsonRequestBehavior.AllowGet);
            }
            catch (SqlException e)
            {   
                return Json(new { result = false, jsonStr = $"AqlException: {e.Message}" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result = false, jsonStr = $"Exception: {e.Message}" }, JsonRequestBehavior.AllowGet);
            }

            HttpContext.Cache[cacheList] = JsonConvert.DeserializeObject<List<Person>>(jsonString);

            return Json( new {result = true, jsonStr = jsonString }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //It checks if any of the parameters are null. If they're not, a Database and Person object will be created and the Person table from the 
        //people databased will be updated with the new information of a changed person
        public JsonResult ChangePeople(string id, string firstName, string lastName)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                return Json(new { result = false, message = "Error: An input field is null or empty" }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                this.database.changePersonData(new Person(Int32.Parse(id), firstName, lastName));
            }
            catch (ArgumentException e)
            {
                return Json(new { result = false, message = $"Argument Exception: {e.Message}" }, JsonRequestBehavior.AllowGet);
            }
            catch (SqlException e)
            {
                return Json(new { result = false, message = $"AqlException: {e.Message}" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result = false, message = $"Exception: {e.Message}" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = true, message = "Data updated successfully" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //It compares the Person list from the database and the list from the cache. It returns two JSON obects: one contains a list of Persons
        //that need to be updated and/or added to the list and the other contains a list of Persons to be removed.
        public JsonResult GetUpdate(/*string list*/)
        {
            Person temp = null;

            List<Person> newList = JsonConvert.DeserializeObject<List<Person>>(this.database.getPersonList());
            List<Person> currentList = (List<Person>)HttpContext.Cache[cacheList];
            List<Person> toUpdate = new List<Person>();
            List<Person> toRemove = new List<Person>();

            foreach(Person person in newList)
            {
                temp = currentList.Find(x => x.id == person.id);

                if (temp != null)
                {
                    if (!temp.Equals(person))
                    {
                        toUpdate.Add(person);
                    }
                }
                else
                {
                    toUpdate.Add(person);
                }
            }

            foreach(Person person in currentList)
            {
                temp = newList.Find(x => x.id == person.id);
                if (temp == null)
                    toRemove.Add(person);
            }

            HttpContext.Cache[cacheList] = newList;

            return Json(new { update = JsonConvert.SerializeObject(toUpdate), delete = JsonConvert.SerializeObject(toRemove) }, JsonRequestBehavior.AllowGet);
        }
    }
}