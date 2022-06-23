using Microsoft.AspNetCore.Mvc;

namespace APIlearn.Controllers
{
    public class test1Controller : Controller
    {
        [HttpGet]
        public int Get()
        {
            return 2;
        }
    }
}
