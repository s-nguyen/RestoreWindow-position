﻿using System;
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
using System.Collections.Generic;


namespace Restorewindow_position
{
    
    public partial class Form1 : Form
    {

        private List<window> activeWindow;

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
                GetWindowText(hWnd, sb, size); //What is size?

                RECT rct = new RECT();
                GetWindowRect(hWnd, ref rct);
                int wWidth = rct.Right - rct.Left;
                int wHeight = rct.Bottom - rct.Top;
                activeWindow.Add(new window(hWnd, sb.ToString(), rct.Left, rct.Top, wWidth, wHeight));




                //Console.WriteLine(sb.ToString());

                
                //MoveWindow(hWnd, 600, 600, 600, 600, true);  
               // textBox1.AppendText(sb.ToString() + "     " + rct.Top + " " + rct.Bottom + " " + rct.Left + " " + rct.Right +  "   " +hWnd + "\n");
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
           // textBox1.Clear();
            activeWindow = new List<window>(); //Creates new list when this button is clicked
            EnumWindows(new EnumWindowsProc(EnumTheWindows), IntPtr.Zero); //Stores required value in a list
            

            

            
        }
        private void button2_Click(object sender, EventArgs e)
        {
            IntPtr hWnd = (IntPtr)66612;
            MoveWindow(hWnd, 0, 0, 700, 700, true);  
            
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
            
        }

        



        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox1.Clear();
            IntPtr skype = (IntPtr)66612;
            RECT rct = new RECT();
            GetWindowRect(skype, ref rct);


            int size = GetWindowTextLength(skype);
            StringBuilder sb = new StringBuilder(size);

            GetWindowText(skype, sb, size);
            textBox1.AppendText(sb.ToString() + "     " + rct.Top + " " + rct.Bottom + " " + rct.Left + " " + rct.Right + "   " + skype + "\n");
            textBox1.AppendText("X: " + rct.Left + "   Y: " + rct.Top + "\n");
            int wWidth = rct.Right - rct.Left;
            int wHeight = rct.Bottom - rct.Top;
            textBox1.AppendText("Width: " + wWidth + "   Height: " + wHeight);
        }

        
    }
}
