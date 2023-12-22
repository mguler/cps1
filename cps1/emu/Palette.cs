using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace mame
{
    public class Palette
    {
        public static uint[] entry_color;
        private static uint trans_uint;
        private static int numcolors;
        public static Color trans_color;
        public static void palette_init()
        {
            int index;
            trans_color = Color.Magenta;
            trans_uint = (uint)trans_color.ToArgb();
            switch (Machine.sBoard)
            {
                case "CPS-1":
                case "CPS-1(QSound)":
                    numcolors = 0xc00;
                    break;
            }
            entry_color = new uint[numcolors];
            for (index = 0; index < numcolors; index++)
            {
                palette_entry_set_color(index, make_argb(0xff, pal1bit((byte)(index >> 0)), pal1bit((byte)(index >> 1)), pal1bit((byte)(index >> 2))));
            }
        }
        public static void palette_entry_set_color(int index, uint rgb)
        {
            if (index >= numcolors || entry_color[index] == rgb)
            {
                return;
            }
            if (index % 0x10 == 0x0f && rgb == 0)
            {
                entry_color[index] = trans_uint;
            }
            else
            {
                entry_color[index] = 0xff000000 | rgb;
            }
        }
        public static uint make_rgb(int r, int g, int b)
        {
            return ((((uint)(r) & 0xff) << 16) | (((uint)(g) & 0xff) << 8) | ((uint)(b) & 0xff));
        }
        public static uint make_argb(int a, int r, int g, int b)
        {
            return ((((uint)(a) & 0xff) << 24) | (((uint)(r) & 0xff) << 16) | (((uint)(g) & 0xff) << 8) | ((uint)(b) & 0xff));
        }
        public static byte pal1bit(byte bits)
        {
            return (byte)(((bits & 1) != 0) ? 0xff : 0x00);
        }
    }
}