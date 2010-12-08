using System.IO;
using System.Linq;
using System.Text;
using href.Utils;
using SubtitleTools.Common.EncodingHelper.Model;

namespace SubtitleTools.Common.EncodingHelper
{
    //from: http://www.codeproject.com/KB/recipes/DetectEncoding.aspx
    public class DetectEncoding
    {
        public static EncodingsInf DetectProbableFileCodepages(string filePath)
        {
            var result = new EncodingsInf();
            var fileBytes = File.ReadAllBytes(filePath);
            if (fileBytes.Length == 0) return result;

            var encList = EncodingTools.DetectInputCodepages(fileBytes, maxEncodings: 10);
            if (encList == null || encList.Length == 0)
            {
                foreach (var item in Encoding.GetEncodings())
                {
                    result.Add(new EncodingInf { Name = item.DisplayName, BodyName = item.Name });
                }
                return result;
            }
                        
            foreach (var item in encList.OrderBy(e => e.EncodingName))
            {
                result.Add(new EncodingInf { Name = item.EncodingName, BodyName = item.BodyName });
            }

            return result;
        }
    }
}
