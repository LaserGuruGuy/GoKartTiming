using System;
using System.Web;
using System.Web.UI;

namespace GoKart
{
    sealed class SmsTiming
    {
        public void Initialize()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"< script type = 'text / javascript' >");
            sb.Append(@"function initialize() {");
            sb.Append(@"var baseUrl = 'https://backend.sms-timing.com';");
            sb.Append(@"var auth = url('?key');");
            sb.Append(@"var params = getUrlParams();");
            sb.Append(@"var baseConnection;");
            sb.Append(@"if (params.customCSS != null) {");
            sb.Append(@"addCustomCSS(params.customCSS); }");
            sb.Append(@"can.when(getConnectionInfo(baseUrl, auth)).then(function(connectionInfo) {");
            sb.Append(@"baseConnection = parseConnectionInfo(connectionInfo);");
            sb.Append(@"can.when(getLiveTimingSettings(baseConnection, params)).then(function(settings) {");
            sb.Append(@"initializeLiveTiming(settings, connectionInfo);");
            sb.Append(@"initBranding(settings);});});}");
            sb.Append(@"</ script >");

            Page page = HttpContext.Current.CurrentHandler as Page;
            ScriptManager ScriptManager = new ScriptManager();
            ScriptManager.RegisterStartupScript(page, page.GetType(), Guid.NewGuid().ToString(), sb.ToString(), false);
        }
    }
}