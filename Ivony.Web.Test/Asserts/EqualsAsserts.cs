using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.Web.Test
{
  public static class EuqalsAsserts
  {

    public static void AreEqual<T>( this TestAssert assert, T obj1, T obj2, string message = null )
    {

      if ( EqualityComparer<T>.Default.Equals( obj1, obj2 ) )
        return;

      assert.Failure( message ?? "断言失败，期望两个对象相等，但两个对象不相等" );

    }


    public static void AreNotEqual<T>( this TestAssert assert, T obj1, T obj2, string message = null )
    {
      if ( !EqualityComparer<T>.Default.Equals( obj1, obj2 ) )
        return;

      assert.Failure( message ?? "断言失败，期望两个对象不相等，但两个对象相等" );
    }


    public static void IsNull<T>( this TestAssert assert, T obj, string message = null ) where T : class
    {
      if ( obj == null )
        return;

      assert.Failure( message ?? "断言失败，期望对象是 null，但对象不是 null" );
    }


    public static void IsNull<T>( this TestAssert assert, T? obj, string message = null ) where T : struct
    {
      if ( obj == null )
        return;

      assert.Failure( message ?? "断言失败，期望对象是 null，但对象有值" );
    }


    public static void NotNull<T>( this TestAssert assert, T obj, string message = null ) where T : class
    {
      if ( obj != null )
        return;

      assert.Failure( message ?? "断言失败，期望对象不为 null，但对象是 null" );
    }

    public static void NotNull<T>( this TestAssert assert, T? obj, string message = null ) where T : struct
    {
      if ( obj.HasValue )
        return;

      assert.Failure( message ?? "断言失败，期望对象有值，但对象是 null" );
    }

    public static void NotDbNull( this TestAssert assert, object obj, string message = null )
    {
      if ( obj == null )
        assert.Failure( message ?? "断言失败，期望对象不为 null 或 DbNull，但对象是 null" );

      if ( Convert.IsDBNull( obj ) )
        assert.Failure( message ?? "断言失败，期望对象不为 null 或 DbNull，但对象是 DbNull" );
    }


    public static void IsTrue( this TestAssert assert, bool value, string message = null )
    {
      if ( value )
        return;

      assert.Failure( message ?? "断言失败，期望结果是 true ，但结果是 false" );
    }


    public static void IsFalse( this TestAssert assert, bool value, string message = null )
    {
      if ( !value )
        return;

      assert.Failure( message ?? "断言失败，期望结果是 false ，但结果是 true" );
    }


    public static void IsTypeOf( this TestAssert assert, object obj, Type type, string message = null )
    {
      if ( type.IsAssignableFrom( obj.GetType() ) )
        return;

      assert.Failure( message ?? "断言失败，期望对象的类型与 '{0}' 相容，但对象类型为 '{1}'", type.AssemblyQualifiedName, obj.GetType().AssemblyQualifiedName );
    }


    public static void IsTypeIs( this TestAssert assert, object obj, Type type, string message = null )
    {
      if ( type.Equals( obj.GetType() ) )
        return;

      assert.Failure( message ?? "断言失败，期望对象的类型是 '{0}'，但对象类型为 '{1}'", type.AssemblyQualifiedName, obj.GetType().AssemblyQualifiedName );
    }



  }
}
