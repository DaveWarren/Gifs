namespace Gifs.Services
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Web;

    public class GifService : IGifService
    {
        public IEnumerable<Frame> GetFrames(HttpPostedFileBase gif, string foldername, string imageName)
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
                //if (j % 2 == 0)
                //{
                //    // frame.Image.RotateFlip(RotateFlipType.Rotate180FlipY);
                //    // frame.Image.MakeTransparent(Color.Aqua);
                //    this.DrawImage(frame.Image);
                //}


                switch (j)
                {
                    case 10:
                    case 11:
                    case 12:
                    case 13:
                        this.DrawDark(frame.Image);
                        break;
                    case 14:
                    case 15:
                    case 16:
                    case 17:
                    case 18:
                    case 19:
                    case 20:
                    case 21:
                    case 22:
                    case 23:
                        this.DrawLight(frame.Image);
                        break;
                }
                
                frame.Image.Save(string.Format(@"C:\Users\dave\Documents\visual studio 2013\Projects\Gifs\Gifs\Output\{0}\{1}_{2}.gif", foldername, imageName, j), ImageFormat.Gif);
                j++;
            }

            return frames;
        }


        private void DrawDark(Bitmap bmp)
        {
            using (var graphics = Graphics.FromImage(bmp))
            {
                //graphics.DrawLine(blackPen, x1, y1, x2, y2);
                var image = Image.FromFile(@"C:\Users\dave\Documents\visual studio 2013\Projects\Gifs\Gifs.Services\Overlays\DarkRed.png");
                graphics.DrawImage(image, 1, 1);
            }
        }

        private void DrawLight(Bitmap bmp)
        {
            using (var graphics = Graphics.FromImage(bmp))
            {
                //graphics.DrawLine(blackPen, x1, y1, x2, y2);
                var image = Image.FromFile(@"C:\Users\dave\Documents\visual studio 2013\Projects\Gifs\Gifs.Services\Overlays\LightRed.png");
                graphics.DrawImage(image, 1, 1);
            }
        }

        private void DrawImage(Bitmap bmp)
        {
            Pen blackPen = new Pen(Color.Black, 3);

            int x1 = 100;
            int y1 = 100;
            int x2 = 500;
            int y2 = 100;
            // Draw line to screen.
            using (var graphics = Graphics.FromImage(bmp))
            {
                //graphics.DrawLine(blackPen, x1, y1, x2, y2);
                var image = Image.FromFile(@"C:\Users\dave\Documents\visual studio 2013\Projects\Gifs\Gifs.Services\wasted.png");
                graphics.DrawImage(image, 1,1);
            }
        }
    }
}