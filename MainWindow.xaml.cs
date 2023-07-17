using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CenterApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CenterWindows(object sender, RoutedEventArgs e)
        {
            List<IntPtr> handles = new List<IntPtr>();
            List<String> titles = new List<String>();

            foreach (Process p in Process.GetProcesses())
            {
                if (!String.IsNullOrEmpty(p.MainWindowTitle))
                {
                    handles.Add(p.MainWindowHandle);
                    titles.Add(p.MainWindowTitle);
                }
            }

            int screenWidth = (int)SystemParameters.WorkArea.Width;
            int screenHeight = (int)SystemParameters.WorkArea.Height;

            foreach (IntPtr hWnd in handles)
            {
                GetWindowRect(hWnd, out RECT appRect);

                int appWidth = appRect.Right - appRect.Left;
                int appHeight = appRect.Bottom - appRect.Top;

                SetWindowPos(hWnd, IntPtr.Zero, (screenWidth - appWidth) / 2, (screenHeight - appHeight) / 2, 0, 0, 0x0001);
            }
        }
    }
}
