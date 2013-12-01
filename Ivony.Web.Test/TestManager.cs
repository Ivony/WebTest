using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Compilation;

namespace Ivony.Web.Test
{
  public static class TestManager
  {


    private static readonly object _sync = new object();

    private static Type[] _testClasses;


    public static Type[] FindTestClasses()
    {
      lock ( _sync )
      {

        if ( _testClasses == null )
        {
          _testClasses = BuildManager.GetReferencedAssemblies().Cast<Assembly>().AsParallel()
            .SelectMany( assembly => assembly.GetTypes() )
            .Where( type => type.IsSubclassOf( typeof( TestClass ) ) )
            .ToArray();
        }

        return _testClasses;
      }
    }


    public static TestResult[] RunTest( Type type, HttpContext context )
    {
      if ( type == null )
        throw new ArgumentNullException( "type" );

      if ( !type.IsSubclassOf( typeof( TestClass ) ) )
        throw new InvalidOperationException();

      return RunTest( Activator.CreateInstance( type ) as TestClass, context );

    }



    public static TestResult[] RunTest( TestClass instance, HttpContext context )
    {

      instance.Initialize( context );


      var results = new List<TestResult>();

      foreach ( var method in GetTestMethods( instance.GetType() ) )
      {

        instance.MethodInitialize();

        results.Add( RunTest( instance, method ) );

        instance.MethodCleanup();


      }

      instance.Cleanup();

      return results.ToArray();
    }


    public static TestInfo GetTestInfo( MethodInfo method )
    {
      return new TestInfo()
      {
        Name = method.Name
      };
    }


    public static TestResult RunTest( TestClass instance, MethodInfo method )
    {

      var info = GetTestInfo( method );
      var invoker = CreateInvoker( instance, method );
      try
      {
        var watch = Stopwatch.StartNew();
        invoker( instance );
        watch.Stop();

        return Success( info, watch.Elapsed );
      }
      catch ( TestAssertFailureException exception )
      {
        return Failure( info, exception );
      }

      catch ( Exception exception )
      {
        return Exception( info, exception );
      }
    }


    private static TestResult Success( TestInfo info, TimeSpan duration )
    {
      return new TestResultSuccess( info, duration );
    }

    private static TestResult Failure( TestInfo info, TestAssertFailureException exception )
    {
      return new TestResultFailure( info, exception );
    }

    private static TestResult Exception( TestInfo info, Exception exception )
    {
      return new TestResultException( info, exception );
    }




    private static Hashtable invokersCache = new Hashtable();

    private static Action<object> CreateInvoker( TestClass instance, MethodInfo method )
    {
      lock ( _sync )
      {
        var invoker = invokersCache[method] as Action<object>;
        if ( invoker == null )
        {

          var instanceParameter = Expression.Parameter( typeof( object ), "obj" );
          var callExpression = Expression.Call( Expression.Convert( instanceParameter, method.DeclaringType ), method );
          var expression = Expression.Lambda<Action<object>>( callExpression, instanceParameter );

          invoker = expression.Compile();
        }

        return invoker;
      }
    }




    private static Hashtable methodsCache = new Hashtable();


    private static MethodInfo[] GetTestMethods( Type testClass )
    {
      lock ( _sync )
      {
        var methods = methodsCache[testClass] as MethodInfo[];

        if ( methods == null )
        {
          methods = testClass.GetMethods( BindingFlags.Public | BindingFlags.Instance )
            .Where( m => !m.GetParameters().Any() )
            .Where( m => m.ReturnType == typeof( void ) )
            .Where( m => m.DeclaringType != typeof( object ) )
            .Where( m => m.DeclaringType != typeof( TestClass ) )
            .Where( m => m.IsVirtual == false )
            .ToArray();
        }

        return methods;
      }
    }



  }
}
