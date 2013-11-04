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

  /// <summary>
  /// 产生测试报告的服务
  /// </summary>
  public class TestReportService
  {

    public TestReportService( TextWriter writer )
    {

      Writer = writer;

    }



    private const string resultsPlace = "<!--results-->";

    public void Begin()
    {
      var template = Resources.Report;

      Writer.Write( template.Remove( template.IndexOf( resultsPlace ) ) );
    }


    public void WriteResults( TestResult[] results )
    {

      foreach ( var r in results )
      {
        WriteResult( r );
      }

    }

    private void WriteResult( TestResult result )
    {

      if ( result.IsSuccessed )
        Writer.WriteLine( "<div class='result success'>" );

      else
        Writer.WriteLine( "<div class='result failure'>" );

      Writer.WriteLine( "<span class='name'>{0}</span>", result.TestInfo.Name );
      var success = result as TestResultSuccess;

      if ( success != null )
        Writer.WriteLine( "<span class='summary'>{0} 毫秒</span>", success.Duration.Milliseconds );

      else
      {
        Writer.WriteLine( "<span class='summary'>错误</span>" );
        Writer.WriteLine( "<div class=message>{0}</div>", result.Message );
      }

      Writer.WriteLine( "</div>" );

      Writer.Flush();
    }

    public void End()
    {
      var template = Resources.Report;

      Writer.Write( template.Substring( template.IndexOf( resultsPlace ) + resultsPlace.Length ) );
    }



    protected TextWriter Writer
    {
      get;
      private set;
    }


  }
}
