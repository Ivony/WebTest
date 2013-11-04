using Ivony.Html;
using Ivony.Html.Parser;
using Ivony.Web.Test.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ivony.Web.Test
{
  public class TestHandler : IHttpHandler, IHttpAsyncHandler
  {
    public bool IsReusable
    {
      get { return false; }
    }

    public void ProcessRequest( HttpContext context )
    {
      throw new InvalidOperationException();
    }

    public IAsyncResult BeginProcessRequest( HttpContext context, AsyncCallback callback, object extraData )
    {


      var tcs = new TaskCompletionSource<object>();

      var task = ProcessRequestCore( context );
      task.ContinueWith( t =>
      {
        if ( t.IsFaulted )
          tcs.TrySetException( t.Exception.InnerException );
        else if ( t.IsCanceled )
          tcs.TrySetCanceled();
        else
          tcs.TrySetResult( null );

        if ( callback != null ) callback( tcs.Task );
      }, TaskScheduler.Default );

      return tcs.Task;
    }


    public void EndProcessRequest( IAsyncResult result )
    {
      var task = (Task) result;
      if ( task.Status == TaskStatus.Faulted )
        throw task.Exception;
    }

    protected async Task ProcessRequestCore( HttpContext context )
    {

      var types = TestManager.FindTestClasses();

      var document = new JumonyParser().Parse( Resources.Report );
      var report = new TestReportService( context.Response.Output );


      report.Begin();
      try
      {

        foreach ( var testType in types )
        {
          var results = await Task.Run( () => TestManager.RunTest( testType ) );
          report.WriteResults( results );
        }
      }
      finally
      {
        report.End();
      }

    }

    private void GenerateReport( IHtmlContainer container, TestResult[] results )
    {
      foreach ( var result in results )
      {
        var resultElement = container.AddElement( "div" );
        resultElement.Class( "result" );

        if ( result.IsSuccessed )
          resultElement.Class( "successed" );


        resultElement.AddElement( "span" ).Class( "name" ).InnerText( result.TestInfo.Name );
        resultElement.AddElement( "span" ).Class( "message" ).InnerText( result.Message );

      }
    }
  }
}
