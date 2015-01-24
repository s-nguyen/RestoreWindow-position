using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restorewindow_position
{
    class window
    {
        private IntPtr hWnd;
        private string name;
        private int xCoordinate;
        private int yCoordinate;
        private int width;
        private int height;

        public window(IntPtr hWnd, string name, int x, int y, int w, int h)
        {
            this.hWnd = hWnd;
            this.name = name;
            xCoordinate = x;
            yCoordinate = y;
            w = width;
            h = height;
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



    }
}
