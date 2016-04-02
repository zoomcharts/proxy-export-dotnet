using System;
using System.Web;

namespace ZoomCharts.ProxyEcho
{
    public class ProxyHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get
            {
                return true;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            // data uri format:
            // data:[<media type>][;charset=<character set>][;base64],<data>
            var data = context.Request["data"];
            
            var i = (data == null || !data.StartsWith("data:")) ? -1 : data.IndexOf(',', 5);

            if (i == -1)
            {
                context.Response.StatusCode = 400;
                context.Response.StatusDescription = "Bad request";
                context.Response.Write("The request did not include a valid 'data' parameter which must be a valid data-uri.");
                return;
            }
            
            var j = data.IndexOf(';', 5, i - 5);
            if (j == -1) j = i;

            var type = data.Substring(5, j - 5);

            var mime = type.Length > 0 ? type : "application/octet-stream";
            var name = context.Request["name"] ?? "export";

            context.Response.AppendHeader("Content-Type", mime);
            context.Response.AppendHeader("Content-Disposition", "attachment; filename*=UTF-8''" + Uri.EscapeDataString(name));
            context.Response.AppendHeader("Cache-Control", "private, must-revalidate, max-age=0");

            var base64 = i > 13 && data.Substring(i - 7, 7) == ";base64";

            if (base64)
            {
                var buffer = Convert.FromBase64String(data.Substring(i + 1));
                context.Response.BinaryWrite(buffer);
            }
            else
            {
                var decoded = Uri.UnescapeDataString(data.Substring(i + 1));
                context.Response.Write(decoded);
            }
        }
    }
}
