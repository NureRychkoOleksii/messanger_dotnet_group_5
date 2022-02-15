using Microsoft.AspNetCore.Mvc;
using Web_MSL.Data;
using Web_MSL.Models;

namespace Web_MSL.Controllers;

public class RegisterController : Controller
{
    private readonly WebMSLContext _context;

    public RegisterController(WebMSLContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(User user)
    {
        await _context.AddAsync(user);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}