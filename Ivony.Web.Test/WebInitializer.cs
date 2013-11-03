using Ivony.Web.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using Ivony.Fluent;



[assembly: PreApplicationStartMethod( typeof( WebInitializer ), "Initialize" )]
namespace Ivony.Web.Test
{


  public class WebInitializer
  {

    public static void Initialize()
    {
      RouteTable.Routes.Add( new TestRoute() );
    }

  }

  internal class TestRoute : RouteBase
  {
    public override RouteData GetRouteData( HttpContextBase httpContext )
    {
      var path = httpContext.Request.AppRelativeCurrentExecutionFilePath + httpContext.Request.PathInfo;

      if ( VirtualPathUtility.GetExtension( path ).EqualsIgnoreCase( ".test" ) )
        return new RouteData( this, new TestRouteHandler() );


      return null;
    }

    public override VirtualPathData GetVirtualPath( RequestContext requestContext, RouteValueDictionary values )
    {
      return null;
    }
  }

  internal class TestRouteHandler : IRouteHandler
  {
    public IHttpHandler GetHttpHandler( RequestContext requestContext )
    {
      return new TestHandler();
    }
  }



}
