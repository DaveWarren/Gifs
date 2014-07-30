namespace Gifs.Models
{
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
}