using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Ivony.Web.Test
{
  public abstract class TestClass
  {

    protected TestClass()
    {
      Assert = new TestAssert();
    }


    protected HttpContext HttpContext
    {
      get;
      private set;
    }


    protected TestAssert Assert
    {
      get;
      private set;
    }


    public virtual void Initialize( HttpContext context )
    {
      HttpContext = context;
    }

    public virtual void Cleanup()
    {
    
    }


    public virtual void MethodInitialize()
    {

    }


    public virtual void MethodCleanup()
    {

    }

  }
}
