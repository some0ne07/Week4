using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApp3.Models;
using MongoDB.Bson;

namespace WebApp3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ImgServices _img;
        private readonly TextServices _txt;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ImgServices img, TextServices txt)
        {
            _logger = logger;
            _img = img;
            _txt = txt;
        }
        

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile theFile,string theText )
        {

            if (theFile.Length > 0)
            {
                //get the bytes from the content stream of the file
                byte[] thePictureAsBytes = new byte[theFile.Length];
                using var fileStream = theFile.OpenReadStream();
                fileStream.Read(thePictureAsBytes, 0, (int)theFile.Length);

                string id = DateTime.Now.ToString();

                BsonDocument txt = new BsonDocument()
                {
                    { "_id", id },
                    { "txt", theText }
                };
                
                await _img.Addocs(id,thePictureAsBytes);
                await _txt.Addocs(txt);

            }

                return View();
        }

        public async Task<IActionResult> Privacy()
        {

            Dictionary<string, byte[]> pList = new Dictionary<string, byte[]>();
            List<BsonDocument> tList = await _txt.Get(); 
            
            foreach(BsonDocument t in tList)
            {
                pList[(string)t["_id"]] = await _img.Get((string)t["_id"]);
            }
            ViewBag.TextList = tList;
            ViewBag.PicList = pList;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}