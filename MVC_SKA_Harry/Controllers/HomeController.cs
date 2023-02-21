using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net.Sockets;
using Microsoft.AspNetCore.Mvc;
using MVC_SKA_Harry.Models;

namespace MVC_SKA_Harry.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger; 
    private readonly KeepAccountList _accountList;

    public HomeController(ILogger<HomeController> logger, KeepAccountList keepAccountList)
    {
        _accountList = keepAccountList;
        _logger = logger;
    }
    //預設進入
    public IActionResult Index()
    {
        return View("index", _accountList);
    }
    //當按下Sumit時進入，如果所有輸入符合就加進KeepAccountList
    public IActionResult Create(KeepAccount keepAccount)
    {
        if (AllInputIsValid())
        {
            AddKeepAccountlist(keepAccount);
        }
        return View("Index",  _accountList);
    }
    //進入此route會回傳儲存在KeepAccountList的Json檔
    [HttpGet("api/records")]
    public IActionResult Privacy()
    {
        return KeepAccountListJson();
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    private void AddKeepAccountlist(KeepAccount keepAccount)
    {
        _accountList.Add(keepAccount.Amount, keepAccount.Date, keepAccount.Remark);
    }
    private bool AllInputIsValid()
    {
        return ModelState.IsValid;
    }
    private JsonResult KeepAccountListJson()
    {
        return Json(_accountList.keepAccounts);
    }
}

public class KeepAccount
{
    [Required]
    public int Amount { get; set; }
    [Required]
    public DateTime Date { get; set; }
    [Required]
    public String Remark { get; set; }
}

public class KeepAccountList
{
    public List<KeepAccount> keepAccounts = new List<KeepAccount>();
    public KeepAccount keepAccount;
    public void Add(int amount, DateTime date, String remark)
    {
        keepAccounts.Add(new KeepAccount(){Amount = amount, Date = date, Remark = remark});
    }
}