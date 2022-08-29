using System;

namespace GoKart
{
    public interface IConfiguration
    {
        Uri Uri { get; set; }
        string baseUrl { get; set; }
        string auth { get; set; }
    }
}