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
using System.Collections.Generic;

/* Pro tip RECT gets the border pixil location*/

namespace Restore_Window_Position
{
    
    public partial class Form1 : Form
    {

        private List<window> activeWindow;


        #region ShowWindowAsync
        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
        #endregion

        //https://msdn.microsoft.com/en-us/library/windows/desktop/ms633545(v=vs.85).aspx
        [DllImport("User32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

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


 
         #region FORM stuff

         

         public Form1()
        {
            InitializeComponent();
        }
        http://stackoverflow.com/questions/4453998/c-sharp-run-application-minimized-at-windows-startup
        private void Form1_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear(); 
            textBox1.AppendText("Saved Programs: \n");
            activeWindow = new List<window>(); //Creates new list when this button is clicked
            EnumWindows(new EnumWindowsProc(EnumTheWindows), IntPtr.Zero); //Stores required value in a list
           
        
        }
        private void button2_Click(object sender, EventArgs e)
        {
           // IntPtr hWnd = (IntPtr)66612;
           // MoveWindow(hWnd, 0, 0, 700, 700, true);  

            foreach(window w in activeWindow){

                if (w.getPosition() == 1)
                {
                    MoveWindow(w.gethWnd(), w.getX(), w.getY(), w.getWidth(), w.getHeight(), true);
                    ShowWindowAsync(w.gethWnd(), w.getPosition());
                }
                else if (w.getPosition() == 2)
                {
                    ShowWindowAsync(w.gethWnd(), w.getPosition());
                }
                else
                {
                   SetWindowPos(w.gethWnd(), (IntPtr)0, w.getX(), w.getY(), 0, 0, 0x0001);
                   ShowWindowAsync(w.gethWnd(), w.getPosition());
                }
                
                
            }
            
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
            
        }


        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
         
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
                
            }
            else if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
                
                notifyIcon1.ShowBalloonTip(300);
                
            }
            
        }
        //Handles resizing to notification area
        //http://stackoverflow.com/questions/16140627/minimize-to-tray
        //http://alperguc.blogspot.com/2008/11/c-system-tray-minimize-to-tray-with.html
        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
               // notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(300);
                this.Hide();
            }
           // else if (FormWindowState.Normal == this.WindowState)
           // {
           //     notifyIcon1.Visible = false;
           // }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           // textBox1.Clear();


            /* Debug
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
            textBox1.AppendText("Width: " + wWidth + "   Height: " + wHeight + "\n");

            window skypee = new window(skype, "skype", rct.Left, rct.Top, wWidth, wHeight);
            
            textBox1.AppendText("" + skypee.getPosition());
             * 
             */
        }
        #endregion

        public bool EnumTheWindows(IntPtr hWnd, IntPtr lParam)
        {
            
            int size = GetWindowTextLength(hWnd);
            if (size++ > 0 && IsWindowVisible(hWnd))
            {
                StringBuilder sb = new StringBuilder(size);
                GetWindowText(hWnd, sb, size); //What is size?

                RECT rct = new RECT();
                GetWindowRect(hWnd, ref rct);
                int wWidth = rct.Right - rct.Left; //Calculates the width
                int wHeight = rct.Bottom - rct.Top; //Calculates the height of a window
                activeWindow.Add(new window(hWnd, sb.ToString(), rct.Left, rct.Top, wWidth, wHeight));

                textBox1.AppendText(sb.ToString() + "\n");


                //Console.WriteLine(sb.ToString());


                //MoveWindow(hWnd, 600, 600, 600, 600, true);  
                // textBox1.AppendText(sb.ToString() + "     " + rct.Top + " " + rct.Bottom + " " + rct.Left + " " + rct.Right +  "   " +hWnd + "\n");
            }
            return true;
        }

        //http://stackoverflow.com/questions/12437751/add-a-function-to-contextmenu-item-at-notifyicon
        public void ExitApplication(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void notifyIcon1_MouseClick(object sender, EventArgs e)
        {

        }

        

        
        
    }
}
//http://stackoverflow.com/questions/1003073/how-to-check-whether-another-app-is-minimized-or-not
//http://stackoverflow.com/questions/18652162/how-to-minimize-maximize-opened-applications