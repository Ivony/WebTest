using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.Web.Test
{
  public static class EuqalsAsserts
  {


    private const string _message = "断言失败，两个对象不相等";


    public static void AreEqual<T>( this TestAssert assert, T obj1, T obj2, string message = null )
    {

      if ( object.Equals( obj1, obj2 ) )
        return;

      message = message ?? _message;

      assert.Failure( message );

    }

  }
}
