using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SitesChecker.App
{
    public class Program
    {
	    public static void Main(string[] args)
	    {
		    var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
		    var pathToContentRoot = Path.GetDirectoryName(pathToExe);
		    var host = WebHost.CreateDefaultBuilder(args)
			    .UseContentRoot(pathToContentRoot)
			    .UseStartup<Startup>()
			    .Build();
		    host.RunAsService();
		    Console.ReadLine();
	    }
    }
}
