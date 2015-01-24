using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Restore_Window_Position
{
    class window
    {

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        private struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public int showCmd;
            public System.Drawing.Point ptMinPosition;
            public System.Drawing.Point ptMaxPosition;
            public System.Drawing.Rectangle rcNormalPosition;
        }

        private IntPtr hWnd;
        private string name;
        private int xCoordinate;
        private int yCoordinate;
        private int width;
        private int height;
        private int position;

        public window(IntPtr hWnd, string name)
        {
            this.hWnd = hWnd;
            this.name = name;
        }

        public window(IntPtr hWnd, string name, int x, int y, int w, int h)
        {
            
            this.hWnd = hWnd;
            this.name = name;
            this.xCoordinate = x;
            this.yCoordinate = y;
            this.width = w;
            this.height = h;
            this.setPosition(w, h);
            
        }

        public IntPtr gethWnd()
        {
            return hWnd;
        }
        public int getX()
        {
            return xCoordinate;
        }
        public int getY()
        {
            return yCoordinate;
        }
        public int getWidth()
        {
            return width;
        }
        public int getHeight()
        {
            return height;
        }

        public void setPosition(int w, int h)
        {
            //Minimize Normal or Maximize
            WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
            GetWindowPlacement(hWnd, ref placement);

            position = placement.showCmd;
            
        }

        public int getPosition()
        {
            return this.position;
        }



    }
}
