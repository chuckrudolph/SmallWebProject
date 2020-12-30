using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebProject
{
    public class MvcApplication : System.Web.HttpApplication
    {
        string connectionString = "Data Source=JMORGA-LT1\\SQLEXPRESS;Initial Catalog=People;Integrated Security=True"; //ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            SqlDependency.Start(connectionString);
        }

        protected void Application_End()
        {
            SqlDependency.Stop(connectionString);
        }
    }
}

