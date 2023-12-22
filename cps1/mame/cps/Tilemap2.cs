using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mame
{
    public partial class Tmap
    {
        public int getxint(int row)
        {
            int i, i1 = 0;
            for (i = 0; i < lsYinit.Count; i++)
            {
                if (lsYinit[i] == row)
                {
                    i1 = lsXinit[i];
                    break;
                }
            }
            return i1;
        }
        public void tilemap_draw_primask10_wofcn(RECT cliprect, int flags, byte _priority)
        {
            int xpos, ypos, coloffset;
            if (!enable)
                return;
            if (all_tiles_dirty)
            {
                Array.Copy(Tilemap.bbFF, tileflags, 0x1000);
                all_tiles_dirty = false;
            }
            bChar = false;
            lsCode = new List<int>();
            lsAttr = new List<int>();
            lsX = new List<int>();
            lsX2 = new List<int>();
            lsY = new List<int>();
            lsXinit = new List<int>();
            lsYinit = new List<int>();
            int scrollx = (CPS.scrollxoff - CPS.scroll1x) & 0x1ff;
            int scrolly = (CPS.scrollyoff - CPS.scroll1y) & 0x1ff;
            for (ypos = scrolly - 0x200; ypos <= cliprect.max_y; ypos += 0x200)
                for (xpos = scrollx - 0x200; xpos <= cliprect.max_x; xpos += 0x200)
                    tilemap_draw_instance10_wofcn(cliprect, xpos, ypos);
            if (bChar == true)
            {
                nChar = lsCode.Count;
                for (iChar = 0; iChar < nChar; iChar++)
                {
                    xinit = getxint(lsY[iChar]);
                    if (lsX[iChar] == xinit)
                    {
                        lsX2[iChar] = lsX[iChar];
                    }
                    else
                    {
                        coloffset = lsX[iChar] - lsX[iChar - 1];
                        lsX2[iChar] = lsX2[iChar - 1] + coloffset + 1;
                    }
                }
                for (iChar = 0; iChar < nChar; iChar++)
                {
                    tile_update02(lsX2[iChar], lsY[iChar], lsCode[iChar], lsAttr[iChar]);
                }
            }
        }
        private void tilemap_draw_instance10_wofcn(RECT cliprect, int xpos, int ypos)
        {
            int mincol, maxcol;
            int x1, y1, x2, y2;
            int y, nexty;
            int offsety1;
            x1 = Math.Max(xpos, cliprect.min_x);
            x2 = Math.Min(xpos + 0x200, cliprect.max_x + 1);
            y1 = Math.Max(ypos, cliprect.min_y);
            y2 = Math.Min(ypos + 0x200, cliprect.max_y + 1);
            if (x1 >= x2 || y1 >= y2)
                return;
            x1 -= xpos;
            y1 -= ypos;
            x2 -= xpos;
            y2 -= ypos;
            offsety1 = y1;
            mincol = x1 / 8;
            maxcol = (x2 + 8 - 1) / 8;
            y = y1;
            nexty = 8 * (y1 / 8) + 8;
            nexty = Math.Min(nexty, y2);
            for (; ; )
            {
                int row = y / 8;
                trans_t prev_trans = trans_t.WHOLLY_TRANSPARENT;
                trans_t cur_trans;
                int x_start = x1;
                int column;
                for (column = mincol; column <= maxcol; column++)
                {
                    int x_end;
                    if (column == maxcol)
                    {
                        cur_trans = trans_t.WHOLLY_TRANSPARENT;
                    }
                    else
                    {
                        if (tileflags[row, column] == Tilemap.TILE_FLAG_DIRTY)
                        {
                            tile_update01(column, row);
                        }
                        if ((tileflags[row, column] & 0x2f) != 0)
                        {
                            cur_trans = trans_t.MASKED;
                        }
                        else
                        {
                            cur_trans = ((flagsmap[offsety1, column * 0x08] & 0x2f) == 0x20) ? trans_t.WHOLLY_OPAQUE : trans_t.WHOLLY_TRANSPARENT;
                        }
                    }
                    if (cur_trans == prev_trans)
                        continue;
                    x_end = column * 8;
                    x_end = Math.Max(x_end, x1);
                    x_end = Math.Min(x_end, x2);
                    if (prev_trans != trans_t.WHOLLY_TRANSPARENT)
                    {
                        int offsety2 = offsety1;
                        int cury;
                        if (prev_trans == trans_t.WHOLLY_OPAQUE)
                        {
                            for (cury = y; cury < nexty; cury++)
                            {
                                Array.Copy(pixmap, offsety2 * 0x200 + x_start, Video.bitmapbase[Video.curbitmap], (offsety2 + ypos) * 0x200 + xpos + x_start, x_end - x_start);
                                offsety2++;
                            }
                        }
                        else if (prev_trans == trans_t.MASKED)
                        {
                            for (cury = y; cury < nexty; cury++)
                            {
                                int i;
                                for (i = 0; i < x_end - x_start; i++)
                                {
                                    if ((flagsmap[offsety2, x_start + i] & 0x2f) == 0x20)
                                    {
                                        Video.bitmapbase[Video.curbitmap][(offsety2 + ypos) * 0x200 + xpos + x_start + i] = pixmap[offsety2 * 0x200 + x_start + i];
                                    }
                                }
                                offsety2++;
                            }
                        }
                    }
                    x_start = x_end;
                    prev_trans = cur_trans;
                }
                if (nexty == y2)
                    break;
                offsety1 += (nexty - y);
                y = nexty;
                nexty += 8;
                nexty = Math.Min(nexty, y2);
            }
        }      
        private void tile_update01(int col, int row)
        {
            byte group0, flags0;
            int x0 = 0x08 * col;
            int y0 = 0x08 * row;
            int palette_base0;
            int code, attr;
            int memindex;
            int gfxset;
            int match;
            int i, j;
            memindex = (row & 0x1f) + ((col & 0x3f) << 5) + ((row & 0x20) << 6);
            {
                code = CPS.gfxram[(CPS.scroll1 + 2 * memindex) * 2] * 0x100 + CPS.gfxram[(CPS.scroll1 + 2 * memindex) * 2 + 1];
                gfxset = (memindex & 0x20) >> 5;
                attr = CPS.gfxram[(CPS.scroll1 + 2 * memindex + 1) * 2] * 0x100 + CPS.gfxram[(CPS.scroll1 + 2 * memindex + 1) * 2 + 1];
                match = 0;
                if (bChar == false && code >= 0xe000 && code <= 0xe9c0)
                {
                    bChar = true;
                }
                if (code >= 0xe000 && code < 0xe9c0)
                {
                    if ((code & 0x11) == 0)
                    {
                        lsX.Add(col);
                        lsX2.Add(0);
                        lsY.Add(row);
                        lsCode.Add(code + 0x2000);
                        lsAttr.Add(attr);
                        if (lsYinit.IndexOf(row) < 0)
                        {
                            lsXinit.Add(col);
                            lsYinit.Add(row);
                        }
                    }
                    return;
                }
                foreach (CPS.gfx_range r in CPS.lsRange0)
                {
                    if (code >= r.start && code <= r.end)
                    {
                        code += r.add;
                        match = 1;
                        break;
                    }
                }
                {
                    if (match == 0)
                    {
                        Array.Copy(Tilemap.empty_tile, 0, pen_data, 0, 0x40);
                    }
                    else
                    {
                        for (j = 0; j < 0x08; j++)
                        {
                            Array.Copy(CPS.gfx1rom, code * 0x80 + gfxset * 8 + j * 16, pen_data, j * 8, 8);
                        }
                    }
                    palette_base0 = 0x10 * ((attr & 0x1f) + 0x20);
                    flags0 = (byte)(((attr & 0x60) >> 5) & 3);
                }
                group0 = (byte)((attr & 0x0180) >> 7);
            }
            {
                int offset = 0;
                byte andmask = 0xff, ormask = 0;
                int dx0 = 1, dy0 = 1;
                int tx, ty;
                if ((flags0 & Tilemap.TILE_FLIPY) != 0)
                {
                    y0 += 0x07;
                    dy0 = -1;
                }
                if ((flags0 & Tilemap.TILE_FLIPX) != 0)
                {
                    x0 += 0x07;
                    dx0 = -1;
                }
                for (ty = 0; ty < 0x08; ty++)
                {
                    int offsetx1 = x0;
                    int offsety1 = y0;
                    int xoffs = 0;
                    y0 += dy0;
                    for (tx = 0; tx < 0x08; tx++)
                    {
                        byte pen, map;
                        pen = pen_data[offset];
                        map = pen_to_flags[group0, pen];
                        pixmap[offsety1 * 0x200 + offsetx1 + xoffs] = (ushort)(palette_base0 + pen);
                        flagsmap[offsety1, offsetx1 + xoffs] = map;
                        andmask &= map;
                        ormask |= map;
                        xoffs += dx0;
                        offset++;
                    }
                }
                tileflags[row, col] = (byte)(andmask ^ ormask);
            }
        }
        private void tile_update02(int col, int row, int code, int attr)
        {
            byte group0, flags0;
            int palette_base0;
            int i, j;
            int x1, y1, offsetx5, offsety5, code1, row1, col1;
            for (offsetx5 = 0; offsetx5 <= 1; offsetx5++)
            {
                for (offsety5 = 0; offsety5 <= 1; offsety5++)
                {
                    code1 = code + offsetx5 + offsety5 * 0x10;
                    col1 = col + offsetx5;
                    row1 = row + offsety5;
                    x1 = col1 * 8;
                    y1 = row1 * 8;
                    for (j = 0; j < 0x08; j++)
                    {
                        Array.Copy(CPS.gfx1rom, code1 * 0x80 + j * 0x10, pen_data, j * 8, 8);
                    }
                    palette_base0 = 0x10 * ((attr & 0x1f) + 0x20);
                    flags0 = (byte)(((attr & 0x60) >> 5) & 3);
                    group0 = (byte)((attr & 0x0180) >> 7);
                    int offset = 0;
                    byte andmask = 0xff, ormask = 0;
                    int dx0 = 1, dy0 = 1;
                    int tx, ty;
                    if ((flags0 & Tilemap.TILE_FLIPY) != 0)
                    {
                        y1 += 0x07;
                        dy0 = -1;
                    }
                    if ((flags0 & Tilemap.TILE_FLIPX) != 0)
                    {
                        x1 += 0x07;
                        dx0 = -1;
                    }
                    for (ty = 0; ty < 0x08; ty++)
                    {
                        int offsetx1 = x1;
                        int offsety1 = y1;
                        int xoffs = 0;
                        y1 += dy0;
                        for (tx = 0; tx < 0x08; tx++)
                        {
                            byte pen, map;
                            pen = pen_data[offset];
                            map = pen_to_flags[group0, pen];
                            pixmap[offsety1 * 0x200 + offsetx1 + xoffs] = (ushort)(palette_base0 + pen);
                            flagsmap[offsety1, offsetx1 + xoffs] = map;
                            andmask &= map;
                            ormask |= map;
                            xoffs += dx0;
                            offset++;
                        }
                        for (i = 0; i < 8; i++)
                        {
                            if ((flagsmap[offsety1, offsetx1 + i] & 0x2f) == 0x20)
                            {
                                Video.bitmapbase[Video.curbitmap][offsety1 * 0x200 + 0x40 + offsetx1 + i] = pixmap[offsety1 * 0x200 + offsetx1 + i];
                            }
                        }
                    }
                    tileflags[row1, col1] = 0xff;
                }
            }
        }
    }
}
