using System.Windows;
using Spring.Context.Support;

namespace Topifier
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var ctx = ContextRegistry.GetContext();
            var shell = ctx.GetObject("MainWindow") as Window;
            
            if (shell != null)
            {
                shell.Show();
            }
        }
    }
}
