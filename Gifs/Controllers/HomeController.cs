namespace Gifs.Controllers
{
    #region Using Directives

    using System;
    using System.Drawing;
    using System.Linq;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;

    using Gifs.Models;
    using Gifs.Services;

    #endregion

    public class HomeController : Controller
    {
        #region Constants and Fields

        private readonly IGifService gifService;

        #endregion

        #region Constructors and Destructors

        public HomeController(IGifService gifService)
        {
            this.gifService = gifService;
        }

        #endregion

        #region Public Methods

        public string GetRandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public ActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public ActionResult Split(HttpPostedFileBase gif)
        {
            var foldername = this.GetRandomString();
            Thread.Sleep(100);
            var imageName = this.GetRandomString();

            var frames = this.gifService.GetFrames(gif, foldername, imageName).ToList();
            
            var model = new SplitGifModel(foldername, imageName, frames.Count());
            return this.View(model);
        }

        #endregion
    }
}