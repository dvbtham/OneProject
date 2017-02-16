using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CRUDCore.Controllers
{
    public class BaseController : Controller
    {
        protected void SetAlert(string message, string type)
        {
            TempData["AlertMessage"] = message;
            if (type == "success")
            {
                TempData["AlertType"] = "alert-success";
            }
            else
                if (type == "warning")
            {
                TempData["AlertType"] = "alert-warning";
            }
            else
                    if (type == "error")
            {
                TempData["AlertType"] = "alert-error";
            }
        }
    }
}