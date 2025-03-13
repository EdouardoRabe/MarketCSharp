using System;
using System.Windows.Forms;
using MarketCSharp.app;

namespace MarketCSharp.Main
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MarketApp());
        }
    }
}