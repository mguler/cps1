using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace mame
{
    public partial class CPS
    {
        public static Bitmap GetTilemapGDI0_wofcn()
        {
            int i1, i2, iOffset, i3, i4, iOffset3 = 0;
            int rows, cols, width, height;
            int scanheight = 0x100, scanrows;
            int tilewidth, tileheight;
            int gfxset;
            tilewidth = 8;
            tileheight = tilewidth;
            rows = 0x40;
            cols = rows;
            width = tilewidth * cols;
            height = width;
            scanrows = scanheight / tileheight;
            ttmap[0].bChar = false;
            ttmap[0].lsX = new List<int>();
            ttmap[0].lsX2 = new List<int>();
            ttmap[0].lsY = new List<int>();
            ttmap[0].lsCode = new List<int>();
            ttmap[0].lsAttr = new List<int>();
            ttmap[0].lsXinit = new List<int>();
            ttmap[0].lsYinit = new List<int>();
            int iByte;
            int iCode, iAttr;
            int iChar, nChar, xinit, coloffset;
            int offsetx5, offsety5;
            int iColor, iFlag, iGroup;
            int idx = 0;
            int x0 = 0, y0 = 0, dx0 = 0, dy0 = 0, match;
            ushort[] uuVRam0;
            uuVRam0 = new ushort[0x2000];
            for (i1 = 0; i1 < 0x2000; i1++)
            {
                uuVRam0[i1] = (ushort)(gfxram[baseTilemap0G + i1 * 2] * 0x100 + gfxram[baseTilemap0G + i1 * 2 + 1]);
            }
            for (i4 = 0; i4 < rows; i4++)
            {
                for (i3 = 0; i3 < cols; i3++)
                {
                    iOffset3 = (i3 * scanrows + i4 % scanrows) * 2 + i4 / scanrows * 0x1000;
                    iCode = uuVRam0[iOffset3];
                    iAttr = uuVRam0[iOffset3 + 1];
                    iColor = iAttr % 0x20 + 0x20;
                    iFlag = ((iAttr & 0x60) >> 5) & 3;
                    iGroup = (iAttr & 0x0180) >> 7;
                    match = 0;
                    if (ttmap[0].bChar == false && iCode >= 0xe000 && iCode < 0xe9c0)
                    {
                        ttmap[0].bChar = true;
                    }
                    if ((iCode >= 0xe000 && iCode < 0xe9c0) && (iCode & 0x11) == 0)
                    {
                        ttmap[0].lsX.Add(i3);
                        ttmap[0].lsX2.Add(0);
                        ttmap[0].lsY.Add(i4);
                        ttmap[0].lsCode.Add(iCode + 0x2000);
                        ttmap[0].lsAttr.Add(iAttr);
                        if (ttmap[0].lsYinit.IndexOf(i4) < 0)
                        {
                            ttmap[0].lsXinit.Add(i3);
                            ttmap[0].lsYinit.Add(i4);
                        }
                    }
                }
            }
            Color c1 = new Color();
            Bitmap bm1;
            bm1 = new Bitmap(width, height);
            BitmapData bmData;
            bmData = bm1.LockBits(new Rectangle(0, 0, bm1.Width, bm1.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            unsafe
            {
                byte* ptr = (byte*)(bmData.Scan0);
                byte* ptr2 = (byte*)0;
                for (i3 = 0; i3 < cols; i3++)
                {
                    for (i4 = 0; i4 < rows; i4++)
                    {
                        iOffset3 = (i3 * scanrows + i4 % scanrows) * 2 + i4 / scanrows * 0x1000;
                        iCode = uuVRam0[iOffset3];
                        iAttr = uuVRam0[iOffset3 + 1];
                        iColor = iAttr % 0x20 + 0x20;
                        iFlag = ((iAttr & 0x60) >> 5) & 3;
                        iGroup = (iAttr & 0x0180) >> 7;
                        match = 0;
                        if (iCode >= 0xe000 && iCode <= 0xe9c0)
                        {
                            continue;
                        }
                        foreach (gfx_range r in lsRange0)
                        {
                            if (iCode >= r.start && iCode <= r.end)
                            {
                                iCode += r.add;
                                match = 1;
                                break;
                            }
                        }
                        if (match == 0)
                        {
                            continue;
                        }
                        else
                        {
                            if (iFlag == 0)
                            {
                                x0 = tilewidth * i3;
                                y0 = tileheight * i4;
                                dx0 = 1;
                                dy0 = 1;
                            }
                            else if (iFlag == 1)
                            {
                                x0 = tilewidth * i3 + tilewidth - 1;
                                y0 = tileheight * i4;
                                dx0 = -1;
                                dy0 = 1;
                            }
                            else if (iFlag == 2)
                            {
                                x0 = tilewidth * i3;
                                y0 = tileheight * i4 + tileheight - 1;
                                dx0 = 1;
                                dy0 = -1;
                            }
                            else if (iFlag == 3)
                            {
                                x0 = tilewidth * i3 + tilewidth - 1;
                                y0 = tileheight * i4 + tileheight - 1;
                                dx0 = -1;
                                dy0 = -1;
                            }
                            gfxset = i3 & 1;
                            for (i1 = 0; i1 < tilewidth; i1++)
                            {
                                for (i2 = 0; i2 < tileheight; i2++)
                                {
                                    iOffset = iCode * 0x80 + gfxset * 8 + i1 + i2 * 0x10;
                                    iByte = gfx1rom[iOffset];
                                    idx = iColor * 0x10 + iByte;
                                    c1 = cc1G[idx];
                                    ptr2 = ptr + ((y0 + dy0 * i2) * width + x0 + dx0 * i1) * 4;
                                    *ptr2 = c1.B;
                                    *(ptr2 + 1) = c1.G;
                                    *(ptr2 + 2) = c1.R;
                                    *(ptr2 + 3) = c1.A;
                                }
                            }
                        }
                    }
                }
                if (ttmap[0].bChar == true)
                {
                    nChar = ttmap[0].lsCode.Count;
                    for (iChar = 0; iChar < nChar; iChar++)
                    {
                        xinit = ttmap[0].getxint(ttmap[0].lsY[iChar]);
                        if (ttmap[0].lsX[iChar] == xinit)
                        {
                            ttmap[0].lsX2[iChar] = ttmap[0].lsX[iChar];
                        }
                        else
                        {
                            coloffset = ttmap[0].lsX[iChar] - ttmap[0].lsX[iChar - 1];
                            if (coloffset > 0)
                            {
                                ttmap[0].lsX2[iChar] = ttmap[0].lsX2[iChar - 1] + coloffset + 1;
                            }
                        }
                    }
                    for (iChar = 0; iChar < nChar; iChar++)
                    {
                        for (offsetx5 = 0; offsetx5 <= 1; offsetx5++)
                        {
                            for (offsety5 = 0; offsety5 <= 1; offsety5++)
                            {
                                x0 = tilewidth * (ttmap[0].lsX2[iChar] + offsetx5);
                                y0 = tileheight * (ttmap[0].lsY[iChar] + offsety5);
                                iCode = ttmap[0].lsCode[iChar] + offsetx5 + offsety5 * 0x10;
                                iAttr = ttmap[0].lsAttr[iChar];
                                iColor = iAttr % 0x20 + 0x20;
                                for (i1 = 0; i1 < tilewidth; i1++)
                                {
                                    for (i2 = 0; i2 < tileheight; i2++)
                                    {
                                        iOffset = iCode * 0x80 + i1 + i2 * 0x10;
                                        iByte = CPS.gfx1rom[iOffset];
                                        idx = iColor * 0x10 + iByte;                                        
                                        c1 = cc1G[idx];
                                        ptr2 = ptr + ((y0 + dy0 * i2) * width + x0 + dx0 * i1) * 4;
                                        *ptr2 = c1.B;
                                        *(ptr2 + 1) = c1.G;
                                        *(ptr2 + 2) = c1.R;
                                        *(ptr2 + 3) = c1.A;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            bm1.UnlockBits(bmData);
            return bm1;
        }
    }
}
