using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;


namespace Restorewindow_position
{
    
    public partial class Form1 : Form
    {
        #region getactivewindow
        protected delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        protected static extern int GetWindowText(IntPtr hWnd, StringBuilder strText, int maxCount);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        protected static extern int GetWindowTextLength(IntPtr hWnd);
        [DllImport("user32.dll")]
        protected static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);
        [DllImport("user32.dll")]
        protected static extern bool IsWindowVisible(IntPtr hWnd);
        public bool EnumTheWindows(IntPtr hWnd, IntPtr lParam)
        {
            int size = GetWindowTextLength(hWnd);
            if (size++ > 0 && IsWindowVisible(hWnd))
            {
                StringBuilder sb = new StringBuilder(size);
                GetWindowText(hWnd, sb, size);
                //Console.WriteLine(sb.ToString());

                RECT rct = new RECT();
                GetWindowRect(hWnd, ref rct);
                //MoveWindow(hWnd, 600, 600, 600, 600, true);  
                textBox1.AppendText(sb.ToString() + rct.Top + " " + rct.Bottom + " " + rct.Left + " " + rct.Right + "\n");
            }
            return true;
        }
        #endregion

        #region coordinates of windows
        // http://msdn.microsoft.com/en-us/library/ms633519(VS.85).aspx
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        // http://msdn.microsoft.com/en-us/library/a5ch4fda(VS.80).aspx
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        #endregion

        #region movewindow?
         [DllImport("user32.dll", SetLastError = true)]
         internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

          
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();

            EnumWindows(new EnumWindowsProc(EnumTheWindows), IntPtr.Zero);
            
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        
    }
}
