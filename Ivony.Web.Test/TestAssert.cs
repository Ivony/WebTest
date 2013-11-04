using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.Web.Test
{
  public class TestAssert
  {

    internal TestAssert()
    {

    }

    public void Failure( string format, params object[] args )
    {
      throw new TestAssertFailureException( string.Format( CultureInfo.InvariantCulture, format, args ) );
    }


  }
}
