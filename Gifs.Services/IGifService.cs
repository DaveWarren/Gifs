using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gifs.Services
{
    using System.Web;

    public interface IGifService
    {
        IEnumerable<Frame> GetFrames(HttpPostedFileBase gif, string foldername, string imageName);
    }
}
