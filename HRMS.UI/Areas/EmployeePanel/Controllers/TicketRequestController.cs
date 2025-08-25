﻿using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.EmployeePanel.Controllers
{
    [Area("EmployeePanel")]
    public class TicketRequestController : Controller
    {
        private readonly IConfiguration _configuration;

        public TicketRequestController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult TicketOpen()
        {
            ViewBag.BaseUrl = _configuration["UIBaseUrlSettings:baseUrl"];

            return View();
        }
        public IActionResult AddTicket()
        {
            ViewBag.BaseUrl = _configuration["UIBaseUrlSettings:baseUrl"];

            return View();
        }

        public IActionResult TicketClose()
        {
            ViewBag.BaseUrl = _configuration["UIBaseUrlSettings:baseUrl"];

            return View();
        }
        public IActionResult TicketRequest()
        {
            return View();
        }
        public IActionResult TicketApplication()
        {
            return View();
        }
    }
}
