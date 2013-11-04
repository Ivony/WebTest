using Ivony.Web.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// MyTest 的摘要说明
/// </summary>
public class MyTest : TestClass
{


  public void Test1()
  {

    throw new Exception( "ABC" );

  }

  public void Test2()
  {
    Assert.AreEqual( 1, 2 );
  }

}