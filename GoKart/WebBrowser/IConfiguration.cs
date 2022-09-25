using System;

namespace GoKart.WebBrowser
{
    public interface IConfiguration
    {
        Uri Uri { get; set; }
        string auth { get; set; }
    }
}