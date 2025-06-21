using System;
using System.Web;
using System.IO;

public class Global : HttpApplication
{
    protected void Application_PostMapRequestHandler(object sender, EventArgs e)
    {

    File.AppendAllText("/tmp/echo-log.txt", $"[{DateTime.UtcNow}] Received {Request.HttpMethod} to {Request.Path}\n");

        if (Context.Request.HttpMethod == "POST" &&
            Context.Request.Path.Equals("/download", StringComparison.OrdinalIgnoreCase))
        {
            Context.RemapHandler(new ZoomCharts.ProxyEcho.ProxyHandler());
        }
    }

}
