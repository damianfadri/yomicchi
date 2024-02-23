using System.Runtime.CompilerServices;

namespace Yomicchi.Desktop
{
    internal class Program
    {
        [STAThread]
        static void Main()
        {
            // TODO Whatever you want to do before starting
            // the WPF application and loading all WPF dlls
            RunApp();
        }

        // Ensure the method is not inlined, so you don't
        // need to load any WPF dll in the Main method
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        static void RunApp()
        {
            var app = new App();
            app.Run();
        }
    }
}
