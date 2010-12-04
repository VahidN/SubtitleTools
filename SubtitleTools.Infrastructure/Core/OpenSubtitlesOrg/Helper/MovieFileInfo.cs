using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using SubtitleTools.Common.Compression;

namespace SubtitleTools.Infrastructure.Core.OpenSubtitlesOrg.Helper
{
    public class MovieFileInfo
    {
        #region Fields (2)

        readonly string _movieFileName;
        readonly string _subFileName;

        #endregion Fields

        #region Constructors (1)

        public MovieFileInfo(string movieFileName, string subFileName)
        {
            _movieFileName = movieFileName;
            _subFileName = subFileName;
            MovieFileLength = new FileInfo(movieFileName).Length;
        }

        #endregion Constructors

        #region Properties (7)

        public long MovieFileLength { set; get; }

        public string MovieFileName
        {
            get
            {
                return Path.GetFileName(_movieFileName);
            }
        }

        public string MovieHash
        {
            get
            {
                return CalculateOsdbHash(_movieFileName);
            }
        }

        public string MovieReleaseName
        {
            get
            {
                return Path.GetFileNameWithoutExtension(_movieFileName);
            }
        }

        public string SubContentToUpload
        {
            get
            {
                var subBytes = File.ReadAllBytes(_subFileName);
                var subBytesCompressed = Compression.CompressZlib(subBytes);
                return Convert.ToBase64String(subBytesCompressed);
            }
        }

        public string SubFileName
        {
            get
            {
                return Path.GetFileName(_subFileName);
            }
        }

        public string SubtitleHash
        {
            get
            {
                return FileMd5(_subFileName);
            }
        }

        #endregion Properties

        #region Methods (2)

        // Public Methods (2) 

        //from: http://trac.opensubtitles.org/projects/opensubtitles/wiki/HashSourceCodes
        public static string CalculateOsdbHash(string fileName)
        {
            using (Stream input = File.OpenRead(fileName))
            {
                var streamsize = input.Length;
                var lhash = streamsize;

                long i = 0;
                var buffer = new byte[sizeof(long)];
                while (i < 65536 / sizeof(long) && (input.Read(buffer, 0, sizeof(long)) > 0))
                {
                    i++;
                    //turn overflow checking option off
                    unchecked { lhash += BitConverter.ToInt64(buffer, 0); }
                }

                input.Position = Math.Max(0, streamsize - 65536);
                i = 0;
                while (i < 65536 / sizeof(long) && (input.Read(buffer, 0, sizeof(long)) > 0))
                {
                    i++;
                    //turn overflow checking option off
                    unchecked { lhash += BitConverter.ToInt64(buffer, 0); }
                }
                input.Close();

                byte[] result = BitConverter.GetBytes(lhash);
                Array.Reverse(result);

                var hexBuilder = new StringBuilder();
                for (i = 0; i < result.Length; i++)
                {
                    hexBuilder.Append(result[i].ToString("x2"));
                }
                return hexBuilder.ToString();
            }
        }

        public static string FileMd5(string filename)
        {
            var md5 = MD5.Create();
            var sb = new StringBuilder();

            using (var fs = File.Open(filename, FileMode.Open))
            {
                foreach (var b in md5.ComputeHash(fs))
                    sb.Append(b.ToString("x2").ToLower());
            }

            return sb.ToString();
        }

        #endregion Methods
    }
}
