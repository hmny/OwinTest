﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

// Add Owing
using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using Owin;

namespace OwinConsole
{
    // alias for owin AppFunc
    using AppFunc = Func<IDictionary<string, object>, Task>;


    public class Program
    {
        static void Main(string[] args)
        {
            WebApp.Start<Startup>("http://localhost:8080");
            Console.WriteLine("server started");
            Console.ReadLine();
        }


    }

    // owin startup
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var middleware = new Func<AppFunc, AppFunc>(MyMiddleWare);
            app.Use(middleware);
        }

        // middleware signature: Func<AppFunc, AppFunc>
        public AppFunc MyMiddleWare(AppFunc next)
        {
            AppFunc appFunc = async (IDictionary<string, object> environment) =>
            {
                // do something with incomming request
                var response = environment["owin.ResponseBody"] as Stream;
                using (var writer = new StreamWriter(response))
                {
                    await writer.WriteAsync(("<h1>Hello from middleware</h1>"));
                }
                // Call next middleware in chain
                // middlewares are responsible for calling eachother
                await next.Invoke(environment);
            };
            return appFunc;
        }

    }
}
