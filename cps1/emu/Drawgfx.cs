using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mame
{
    public class Drawgfx
    {
        private static void setpixelcolorC(int offsety, int offsetx, int n, uint pmask)
        {
            if (((1 << (Tilemap.priority_bitmap[offsety, offsetx] & 0x1f)) & pmask) == 0)
            {
                Video.bitmapbase[Video.curbitmap][offsety * 0x200 + offsetx] = (ushort)n;
            }
            Tilemap.priority_bitmap[offsety, offsetx] = (byte)((Tilemap.priority_bitmap[offsety, offsetx] & 0x7f) | 0x1f);
        }
        private static void blockmove_4toN_transpen_pri16(int code, int srcmodulo, int leftskip, int topskip, int flipx, int flipy, int dstwidth, int dstheight, int colorbase, int offsety, int offsetx,uint primask)
        {
            int ydir;
            int offset = code * 0x80;
            if (flipy != 0)
            {
                offsety += (dstheight - 1);
                offset += (0x10 - dstheight - topskip) * srcmodulo;
                ydir = -1;
            }
            else
            {
                offset += topskip * srcmodulo;
                ydir = 1;
            }
            if (flipx != 0)
            {
                offsetx += dstwidth - 1;
                offset += (0x10 - dstwidth - leftskip) / 2;
                leftskip = (0x10 - dstwidth - leftskip) & 1;
            }
            else
            {
                offset += leftskip / 2;
                leftskip &= 1;
            }
            srcmodulo -= (dstwidth + leftskip) / 2;
            if (flipx != 0)
            {
                int endoffset;
                while (dstheight != 0)
                {
                    int col;
                    endoffset = offsety * 0x200 + offsetx - dstwidth;
                    if (leftskip != 0)
                    {
                        col = CPS.gfx1rom[offset * 2 + 1];
                        offset++;
                        if (col != 0x0f)
                            setpixelcolorC(offsety, offsetx, colorbase + col,primask);
                        offsetx += (-1);
                    }
                    while (offsety * 0x200 + offsetx > endoffset)
                    {
                        col = CPS.gfx1rom[offset * 2];
                        if (col != 0x0f)
                            setpixelcolorC(offsety, offsetx, colorbase + col, primask);
                        offsetx += (-1);
                        if (offsety * 0x200 + offsetx > endoffset)
                        {
                            col = CPS.gfx1rom[offset * 2 + 1];
                            offset++;
                            if (col != 0x0f)
                                setpixelcolorC(offsety, offsetx, colorbase + col, primask);
                            offsetx += (-1);
                        }
                    }
                    offset += srcmodulo;
                    offsety += ydir;
                    offsetx += dstwidth;
                    dstheight--;
                }
            }
            else
            {
                int endoffset;
                while (dstheight != 0)
                {
                    int col;
                    endoffset = offsety * 0x200 + offsetx + dstwidth;
                    if (leftskip!=0)
                    {
                        col = CPS.gfx1rom[offset * 2 + 1];
                        offset++;
                        if (col != 0x0f)
                            setpixelcolorC(offsety, offsetx, colorbase + col, primask);
                        offsetx++;

                    }
                    while (offsety * 0x200 + offsetx < endoffset)
                    {
                        col = CPS.gfx1rom[offset * 2];
                        if (col != 0x0f)
                            setpixelcolorC(offsety, offsetx, colorbase + col, primask);
                        offsetx++;
                        if (offsety * 0x200 + offsetx < endoffset)
                        {
                            col = CPS.gfx1rom[offset * 2 + 1];
                            offset++;
                            if (col != 0x0f)
                                setpixelcolorC(offsety, offsetx, colorbase + col, primask);
                            offsetx++;
                        }
                    }
                    offset += srcmodulo;
                    offsety += ydir;
                    offsetx += (-dstwidth);
                    dstheight--;
                }
            }
        }        
        public static void common_drawgfx_c(int code, int color, int flipx, int flipy, int sx, int sy, uint primask)
        {
            int ox;
            int oy;
            int ex;
            int ey;
            int width, height;

            width = Tilemap.videovisarea.max_x + 1;
            height = Tilemap.videovisarea.max_y + 1;

            /* check bounds */
            ox = sx;
            oy = sy;

            ex = sx + 0x0f;
            if (sx < 0)
                sx = 0;
            if (sx < Tilemap.videovisarea.min_x)
                sx = Tilemap.videovisarea.min_x;
            if (ex >= width)
                ex = width - 1;
            if (ex > Tilemap.videovisarea.max_x)
                ex = Tilemap.videovisarea.max_x;
            if (sx > ex)
                return;

            ey = sy + 0x0f;
            if (sy < 0)
                sy = 0;
            if (sy < Tilemap.videovisarea.min_y)
                sy = Tilemap.videovisarea.min_y;
            if (ey >= height)
                ey = height - 1;
            if (ey > Tilemap.videovisarea.max_y)
                ey = Tilemap.videovisarea.max_y;
            if (sy > ey)
                return;

            int ls = sx - ox;											/* left skip */
            int ts = sy - oy;											/* top skip */
            int dw = ex - sx + 1;										/* dest width */
            int dh = ey - sy + 1;										/* dest height */
            int colorbase = 0x10 * color;
            blockmove_4toN_transpen_pri16(code, 0x08, ls, ts, flipx, flipy, dw, dh, colorbase, sy, sx,primask);
        }
    }
}