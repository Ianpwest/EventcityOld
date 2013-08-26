using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace MVCEventBench.Classes
{
    public class ImageResult : ActionResult
    {
        public String ContentType { get; set; }

        public byte[] ImageBytes { get; set; }
        public String SourceFilename { get; set; }

        public ImageResult(byte[] sourceStream, String contentType)
        {
            ImageBytes = sourceStream;
            ContentType = contentType;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.Clear();
            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.ContentType = ContentType;

            if (ImageBytes != null)
            {
                var stream = new MemoryStream(ImageBytes);
                stream.WriteTo(response.OutputStream);
                stream.Dispose();
            }
            else
            {
                try
                {
                    response.TransmitFile(SourceFilename);
                }
                catch { }
            }
        }
    }
}