using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gifs.Controllers
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Threading;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Split(HttpPostedFileBase gif)
        {
            var foldername = GetRandomString();
            Thread.Sleep(100);
            var imageName = GetRandomString();

            var frames = GetFrames(gif, foldername, imageName).ToList();
            var converter = new ImageConverter();
            //var x = (byte[])converter.ConvertTo(frames.First().Image, typeof(byte[]));
            var model = new SplitGifModel(foldername, imageName, frames.Count());
            return this.View(model);
        }

        

        private IEnumerable<Frame> GetFrames(HttpPostedFileBase gif, string foldername, string imageName)
        {
            var img = Image.FromStream(gif.InputStream);
            var frames = new List<Frame>();

            var frameCount = img.GetFrameCount(FrameDimension.Time);

            //Get the times stored in the gif
            //PropertyTagFrameDelay ((PROPID) 0x5100) comes from gdiplusimaging.h
            //More info on http://msdn.microsoft.com/en-us/library/windows/desktop/ms534416(v=vs.85).aspx
            var times = img.GetPropertyItem(0x5100).Value;

            //Convert the 4bit duration chunk into an int

            for (int i = 0; i < frameCount; i++)
            {
                //convert 4 bit value to integer
                var duration = BitConverter.ToInt32(times, 4 * i);

                //Add a new frame to our list of frames
                frames.Add(new Frame() { Image = new Bitmap(img), Duration = duration });

                //Set the write frame before we save it
                img.SelectActiveFrame(FrameDimension.Time, i);
            }

            //Dispose the image when we're done
            img.Dispose();
            var j = 0;

            string path = string.Format(@"C:\Users\dave\Documents\visual studio 2013\Projects\Gifs\Gifs\Output\{0}", foldername);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            
            foreach (var frame in frames)
            {
                //@"C:\Users\dave\Documents\visual studio 2013\Projects\Gifs\Gifs\Output\
                frame.Image.Save(string.Format(@"C:\Users\dave\Documents\visual studio 2013\Projects\Gifs\Gifs\Output\{0}\{1}_{2}.gif", foldername, imageName, j), ImageFormat.Gif);
                j++;                
            }

            return frames;
        }

        public string GetRandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());
        }
    }

    public class SplitGifModel
    {
        public SplitGifModel(string foldername, string imageName, int count)
        {
            this.Foldername = foldername;
            this.ImageName = imageName;
            this.Count = count;
        }

        public string Foldername { get; set; }
        public string ImageName { get; set; }
        public int Count { get; set; } 
    }

    public class Frame
    {
        public Bitmap Image { get; set; }
        public int Duration { get; set; } 
    }


}