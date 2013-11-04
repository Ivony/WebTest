using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.Web.Test
{
  public static class SetAsserts
  {


    public static void HasAny<T>( this TestAssert assert, IEnumerable<T> set, string message = null )
    {
      if ( set.Any() )
        return;

      assert.Failure( message ?? "断言失败，集合没有任何元素" );
    }

    public static void HasAny<T>( this TestAssert assert, IEnumerable<T> set, Func<T, bool> predicate, string message = null )
    {
      if ( set.Any( predicate ) )
        return;

      assert.Failure( message ?? "断言失败，集合没有满足指定条件的元素" );
    }

    public static void Contains<T>( this TestAssert assert, IEnumerable<T> set, T element, string message = null )
    {
      if ( set.Contains( element ) )
        return;

      assert.Failure( message ?? "断言失败，集合不包含指定的元素" );
    }


    public static void IsSubsetOf<T>( this TestAssert assert, IEnumerable<T> set, IEnumerable<T> sub, string message = null )
    {
      if ( set.Intersect( sub ).Count() == sub.Count() )
        return;

      assert.Failure( message ?? "断言失败，验证的集合不是指定集合的子集" );
    }

  }
}
