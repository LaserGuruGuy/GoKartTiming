using System;
using System.Web;
using System.Web.UI;

namespace GoKart
{
    sealed class WebSocket
    {
        public void Initialize()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            Page page = HttpContext.Current.CurrentHandler as Page;
            ScriptManager ScriptManager = new ScriptManager();
            ScriptManager.RegisterStartupScript(page, page.GetType(), Guid.NewGuid().ToString(), sb.ToString(), false);
        }
    }
}