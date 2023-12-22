using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ui;

namespace mame
{
    public partial class CPS
    {
        public static sbyte MCReadByte_forgottn(int address)
        {
            address &= 0xffffff;
            sbyte result = 0;
            if (address >= 0x800052 && address <= 0x800055)
            {
                int offset=(address-0x800052)/2;
                result = (sbyte)((Inptport.input_port_read_direct(Inptport.analog_p0) - dial0) >> (8 * offset));
            }
            else if (address >= 0x80005a && address <= 0x80005d)
            {
                int offset = (address - 0x80005a) / 2;
                result = (sbyte)((Inptport.input_port_read_direct(Inptport.analog_p1) - dial1) >> (8 * offset));
            }
            else
            {
                result = MCReadByte(address);
            }
            return result;
        }
        public static short MCReadWord_forgottn(int address)
        {
            address &= 0xffffff;
            short result = 0;
            if (address >= 0x800052 && address <= 0x800055)
            {
                int offset = (address - 0x800052) / 2;
                result = (short)(((Inptport.input_port_read_direct(Inptport.analog_p0) - dial0) >> (8 * offset)) & 0xff);
            }
            else if (address >= 0x80005a && address <= 0x80005d)
            {
                int offset = (address - 0x80005a) / 2;
                result = (short)(((Inptport.input_port_read_direct(Inptport.analog_p1) - dial1) >> (8 * offset)) & 0xff);
            }
            else
            {
                result = MCReadWord(address);
            }
            return result;
        }
        public static int MCReadLong_forgottn(int address)
        {
            address &= 0xffffff;
            int result = 0;
            if (address >= 0x800052 && address + 3 <= 0x800055)
            {
                result = (int)(((Inptport.input_port_read_direct(Inptport.analog_p0) - dial0) & 0xff) * 0x10000 + (((Inptport.input_port_read_direct(Inptport.analog_p0) - dial0) >> 8) & 0xff));
            }
            else if (address >= 0x80005a && address + 3 <= 0x80005d)
            {
                result = (int)(((Inptport.input_port_read_direct(Inptport.analog_p1) - dial1) & 0xff) * 0x10000 + (((Inptport.input_port_read_direct(Inptport.analog_p1) - dial1) >> 8) & 0xff));
            }
            else
            {
                result = MCReadLong(address);
            }
            return result;
        }
        public static void MCWriteByte_forgottn(int address, sbyte value)
        {
            address &= 0xffffff;
            if (address >= 0x800040 && address <= 0x800041)
            {
                dial0 = (int)Inptport.input_port_read_direct(Inptport.analog_p0);
            }
            else if (address >= 0x800048 && address <= 0x800049)
            {
                dial1 = (int)Inptport.input_port_read_direct(Inptport.analog_p1);
            }
            else
            {
                MCWriteByte(address, value);
            }
        }
        public static void MCWriteWord_forgottn(int address, short value)
        {
            address &= 0xffffff;
            if (address >= 0x800040 && address <= 0x800041)
            {
                dial0 = (int)Inptport.input_port_read_direct(Inptport.analog_p0);
            }
            else if (address >= 0x800048 && address <= 0x800049)
            {
                dial1 = (int)Inptport.input_port_read_direct(Inptport.analog_p1);
            }
            else
            {
                MCWriteWord(address, value);
            }
        }
        public static void MCWriteLong_forgottn(int address, int value)
        {
            address &= 0xffffff;
            if (address >= 0x800040 && address <= 0x800041)
            {
                return;
            }
            else if (address >= 0x800048 && address <= 0x800049)
            {
                return;
            }
            else
            {
                MCWriteLong(address, value);
            }
        }
        public static sbyte MCReadByte_sf2thndr(int address)
        {
            address &= 0xffffff;
            sbyte result = 0;
            if (address >= 0x8001c0 && address <= 0x8001ff)
            {
                int offset = (address - 0x8001c0) / 2;
                result = (sbyte)cps1_cps_b_r(offset);
            }
            else
            {
                result = MCReadByte(address);
            }
            return result;
        }
        public static short MCReadWord_sf2thndr(int address)
        {
            address &= 0xffffff;
            short result = 0;
            if (address >= 0x8001c0 && address + 1 <= 0x8001ff)
            {
                int offset = (address - 0x8001c0) / 2;
                result = (short)cps1_cps_b_r(offset);
            }
            else
            {
                result = MCReadWord(address);
            }
            return result;
        }
        public static void MCWriteWord_sf2thndr(int address, short value)
        {
            address &= 0xffffff;
            if (address >= 0x8001c0 && address + 1 <= 0x8001ff)
            {
                cps1_cps_b_w((address & 0x3f) / 2, (ushort)value);
            }
            else
            {
                MCWriteWord(address, value);
            }
        }
        public static short MCReadWord_sf2ceblp(int address)
        {
            address &= 0xffffff;
            short result = 0;
            if (address >= 0x57a2b0 && address + 1 <= 0x57a2b1)
            {
                if (sf2ceblp_prot == 0x0)
                {
                    result = 0x1992;
                }
                else if (sf2ceblp_prot == 0x04)
                {
                    result = 0x0408;
                }
                else
                {
                    result = -1;
                }
            }
            else
            {
                result = MCReadWord(address);
            }
            return result;
        }
        public static void MCWriteWord_sf2ceblp(int address, short value)
        {
            address &= 0xffffff;
            if (address >= 0x5762b0 && address + 1 <= 0x5762b1)
            {
                sf2ceblp_prot = value;
            }
            else
            {
                MCWriteWord(address,value);
            }
        }
        public static sbyte MCReadByte_sf2m3(int address)
        {
            address &= 0xffffff;
            sbyte result = 0;
            if (address <= 0x3fffff)
            {
                if (address < Memory.mainrom.Length)
                {
                    result = (sbyte)Memory.mainrom[address];
                }
                else
                {
                    result = 0;
                }
            }
            else if (address >= 0x800010 && address <= 0x800011)
            {
                if (address == 0x800010)
                {
                    result = (sbyte)(short1 >> 8);
                }
                else if (address == 0x800011)
                {
                    result = (sbyte)(short1);
                }
            }
            else if (address >= 0x800028 && address <= 0x80002f)
            {
                int offset = (address - 0x800028) / 2;
                result = (sbyte)cps1_dsw_r(offset);
            }
            else if (address >= 0x800140 && address <= 0x80017f)
            {
                int offset = (address - 0x800140) / 2;
                result = (sbyte)cps1_cps_b_r(offset);
            }
            else if (address >= 0x800186 && address <= 0x800187)
            {
                result = (sbyte)short2;
            }
            else if (address >= 0x900000 && address <= 0x92ffff)
            {
                result = (sbyte)gfxram[(address & 0x3ffff)];
            }
            else if (address >= 0xff0000 && address <= 0xffffff)
            {
                result = (sbyte)Memory.mainram[address & 0xffff];
            }
            return result;
        }
        public static short MCReadWord_sf2m3(int address)
        {
            address &= 0xffffff;
            short result = 0;
            if (address <= 0x3fffff)
            {
                if (address + 1 < Memory.mainrom.Length)
                {
                    result = (short)(Memory.mainrom[address] * 0x100 + Memory.mainrom[address + 1]);
                }
                else
                {
                    result = 0;
                }
            }
            else if (address >= 0x800010 && address + 1 <= 0x800011)
            {
                result = short1;
            }
            else if (address >= 0x800028 && address + 1 <= 0x80002f)
            {
                int offset = (address - 0x800028) / 2;
                result = (short)(((byte)(cps1_dsw_r(offset)) << 8) | (byte)cps1_dsw_r(offset));
            }
            else if (address >= 0x800140 && address + 1 <= 0x80017f)
            {
                int offset = (address - 0x800140) / 2;
                result = (short)cps1_cps_b_r(offset);
            }
            else if (address >= 0x800186 && address + 1 <= 0x800187)
            {
                result = (short)((short2 << 8) | (byte)short2);
            }
            else if (address >= 0x900000 && address + 1 <= 0x92ffff)
            {
                result = (short)(gfxram[(address & 0x3ffff)] * 0x100 + gfxram[(address & 0x3ffff) + 1]);
            }
            else if (address >= 0xff0000 && address + 1 <= 0xffffff)
            {
                result = (short)(Memory.mainram[(address & 0xffff)] * 0x100 + Memory.mainram[(address & 0xffff) + 1]);
            }
            return result;
        }
        public static int MCReadLong_sf2m3(int address)
        {
            address &= 0xffffff;
            int result = 0;
            if (address <= 0x3fffff)
            {
                if (address + 3 < Memory.mainrom.Length)
                {
                    result = (int)(Memory.mainrom[address] * 0x1000000 + Memory.mainrom[address + 1] * 0x10000 + Memory.mainrom[address + 2] * 0x100 + Memory.mainrom[address + 3]);
                }
                else
                {
                    result = 0;
                }
            }
            else if (address >= 0x800010 && address + 3 <= 0x800011)
            {
                result = 0;
            }
            else if (address >= 0x800028 && address + 3 <= 0x80002f)
            {
                result = 0;
            }
            else if (address >= 0x800140 && address + 3 <= 0x80017f)
            {
                int offset = (address - 0x800140) / 2;
                result = cps1_cps_b_r(offset) * 0x10000 + cps1_cps_b_r(offset + 1);
            }
            else if (address >= 0x800186 && address + 3 <= 0x800187)
            {
                result = 0;
            }
            else if (address >= 0x900000 && address + 3 <= 0x92ffff)
            {
                result = (int)(gfxram[(address & 0x3ffff)] * 0x1000000 + gfxram[(address & 0x3ffff) + 1] * 0x10000 + gfxram[(address & 0x3ffff) + 2] * 0x100 + gfxram[(address & 0x3ffff) + 3]);
            }
            else if (address >= 0xff0000 && address + 3 <= 0xffffff)
            {
                result = (int)(Memory.mainram[(address & 0xffff)] * 0x1000000 + Memory.mainram[(address & 0xffff) + 1] * 0x10000 + Memory.mainram[(address & 0xffff) + 2] * 0x100 + Memory.mainram[(address & 0xffff) + 3]);
            }
            return result;
        }
        public static void MCWriteByte_sf2m3(int address, sbyte value)
        {
            address &= 0xffffff;
            if (address >= 0x800030 && address <= 0x800037)
            {
                if (address % 2 == 0)
                {
                    cps1_coinctrl_w((ushort)(value * 0x100));
                }
                else
                {
                    return;
                }
            }
            else if (address >= 0x800100 && address <= 0x80013f)
            {
                return;
            }
            else if (address >= 0x800140 && address <= 0x80017f)
            {
                return;
            }
            else if (address >= 0x800190 && address <= 0x800197)
            {
                Sound.soundlatch_w((ushort)value);
            }
            else if (address >= 0x800198 && address <= 0x80019f)
            {
                Sound.soundlatch2_w((ushort)value);
            }
            else if (address >= 0x8001a0 && address <= 0x8001c3)
            {
                return;
            }
            else if (address >= 0x8001c4 && address <= 0x8001c5)
            {
                return;
            }
            else if (address >= 0x900000 && address <= 0x92ffff)
            {
                gfxram[(address & 0x3ffff)] = (byte)(value);
                cps1_gfxram_w((address & 0x3ffff) / 2);
            }
            else if (address >= 0xff0000 && address <= 0xffffff)
            {
                Memory.mainram[(address & 0xffff)] = (byte)(value);
            }
            else
            {
                return;
            }
        }
        public static void MCWriteWord_sf2m3(int address, short value)
        {
            address &= 0xffffff;
            if (address >= 0x800030 && address + 1 <= 0x800037)
            {
                return;
            }
            else if (address >= 0x800100 && address + 1 <= 0x80013f)
            {
                cps1_cps_a_w((address & 0x3f) / 2, (ushort)value);
            }
            else if (address >= 0x800140 && address + 1 <= 0x80017f)
            {
                cps1_cps_b_w((address & 0x3f) / 2, (ushort)value);
            }
            else if (address >= 0x800190 && address + 1 <= 0x800197)
            {
                Sound.soundlatch_w((ushort)value);
            }
            else if (address >= 0x800198 && address + 1 <= 0x80019f)
            {
                Sound.soundlatch2_w((ushort)value);
            }
            else if (address >= 0x8001a0 && address + 1 <= 0x8001c3)
            {
                cps1_cps_a_w((address - 0x8001a0) / 2, (ushort)value);
            }
            else if (address >= 0x8001c4 && address + 1 <= 0x8001c5)
            {
                sf2m3_layer_w((ushort)value);
            }
            else if (address >= 0x900000 && address + 1 <= 0x92ffff)
            {
                gfxram[(address & 0x3ffff)] = (byte)(value >> 8);
                gfxram[(address & 0x3ffff) + 1] = (byte)value;
                cps1_gfxram_w((address & 0x3ffff) / 2);
            }
            else if (address >= 0xff0000 && address + 1 <= 0xffffff)
            {
                Memory.mainram[(address & 0xffff)] = (byte)(value >> 8);
                Memory.mainram[(address & 0xffff) + 1] = (byte)(value);
            }
            else
            {
                return;
            }
        }
        public static void MCWriteLong_sf2m3(int address, int value)
        {
            address &= 0xffffff;
            if (address >= 0x800030 && address + 3 <= 0x800037)
            {
                return;
            }
            else if (address >= 0x800100 && address + 3 <= 0x80013f)
            {
                cps1_cps_a_w((address & 0x3f) / 2, (ushort)(value >> 16));
                cps1_cps_a_w(((address + 2) & 0x3f) / 2, (ushort)value);
            }
            else if (address >= 0x800140 && address + 3 <= 0x80017f)
            {
                cps1_cps_b_w((address & 0x3f) / 2, (ushort)(value >> 16));
                cps1_cps_b_w(((address + 2) & 0x3f) / 2, (ushort)value);
            }
            else if (address >= 0x800190 && address + 3 <= 0x800197)
            {
                return;
            }
            else if (address >= 0x800198 && address + 3 <= 0x80019f)
            {
                return;
            }
            else if (address >= 0x8001a0 && address + 3 <= 0x8001c3)
            {
                cps1_cps_a_w((address - 0x8001a0) / 2, (ushort)(value >> 16));
                cps1_cps_a_w((address + 2 - 0x8001a0) / 2, (ushort)value);
            }
            else if (address >= 0x8001c4 && address + 3 <= 0x8001c5)
            {
                return;
            }
            else if (address >= 0x900000 && address + 3 <= 0x92ffff)
            {
                gfxram[(address & 0x3ffff)] = (byte)(value >> 24);
                gfxram[(address & 0x3ffff) + 1] = (byte)(value >> 16);
                gfxram[(address & 0x3ffff) + 2] = (byte)(value >> 8);
                gfxram[(address & 0x3ffff) + 3] = (byte)(value);
                cps1_gfxram_w((address & 0x3ffff) / 2);
                cps1_gfxram_w(((address + 2) & 0x3ffff) / 2);
            }
            else if (address >= 0xff0000 && address + 3 <= 0xffffff)
            {
                Memory.mainram[(address & 0xffff)] = (byte)(value >> 24);
                Memory.mainram[(address & 0xffff) + 1] = (byte)(value >> 16);
                Memory.mainram[(address & 0xffff) + 2] = (byte)(value >> 8);
                Memory.mainram[(address & 0xffff) + 3] = (byte)(value);
            }
            else
            {
                return;
            }
        }
        public static sbyte MCReadByte_sf2m10(int address)
        {
            address &= 0xffffff;
            sbyte result = 0;
            if (address <= 0x3fffff)
            {
                if (address < Memory.mainrom.Length)
                {
                    result = (sbyte)Memory.mainrom[address];
                }
                else
                {
                    result = 0;
                }
            }
            else if (address >= 0x800000 && address <= 0x800007)
            {
                if (address == 0x800000)
                {
                    result = (sbyte)(short1 >> 8);
                }
                else if (address == 0x800001)
                {
                    result = (sbyte)(short1);
                }
                else
                {
                    result = -1;
                }
            }
            else if (address >= 0x800018 && address <= 0x80001f)
            {
                int offset = (address - 0x800018) / 2;
                result = (sbyte)cps1_dsw_r(offset);
            }
            else if (address >= 0x800020 && address <= 0x800021)
            {
                result = 0;
            }
            else if (address >= 0x800140 && address <= 0x80017f)
            {
                int offset = (address - 0x800140) / 2;
                result = (sbyte)cps1_cps_b_r(offset);
            }
            else if (address >= 0x900000 && address <= 0x92ffff)
            {
                result = (sbyte)gfxram[(address & 0x3ffff)];
            }
            else if (address >= 0xe00000 && address <= 0xefffff)
            {
                result = (sbyte)mainram2[address & 0xfffff];
            }
            else if (address >= 0xf1c000 && address <= 0xf1c001)
            {
                result = (sbyte)short2;
            }
            else if (address >= 0xfeff00 && address <= 0xfeffff)
            {
                result = (sbyte)mainram3[address & 0xff];
            }
            else if (address >= 0xff0000 && address <= 0xffffff)
            {
                result = (sbyte)Memory.mainram[address & 0xffff];
            }
            return result;
        }
        public static short MCReadWord_sf2m10(int address)
        {
            address &= 0xffffff;
            short result = 0;
            if (address <= 0x3fffff)
            {
                if (address + 1 < Memory.mainrom.Length)
                {
                    result = (short)(Memory.mainrom[address] * 0x100 + Memory.mainrom[address + 1]);
                }
                else
                {
                    result = 0;
                }
            }
            else if (address >= 0x800000 && address + 1 <= 0x800007)
            {
                result = short1;
            }
            else if (address >= 0x800018 && address + 1 <= 0x80001f)
            {
                int offset = (address - 0x800018) / 2;
                result = (short)(((ushort)(cps1_dsw_r(offset)) << 8) | (ushort)cps1_dsw_r(offset));
            }
            else if (address >= 0x800020 && address + 1 <= 0x800021)
            {
                result = 0;
            }
            else if (address >= 0x800140 && address + 1 <= 0x80017f)
            {
                int offset = (address - 0x800140) / 2;
                result = (short)cps1_cps_b_r(offset);
            }
            else if (address >= 0x900000 && address + 1 <= 0x92ffff)
            {
                result = (short)(gfxram[(address & 0x3ffff)] * 0x100 + gfxram[(address & 0x3ffff) + 1]);
            }
            else if (address >= 0xe00000 && address + 1 <= 0xefffff)
            {
                result = (short)(mainram2[(address & 0xfffff)] * 0x100 + mainram2[(address & 0xfffff) + 1]);
            }
            else if (address >= 0xf1c000 && address + 1 <= 0xf1c001)
            {
                result = (short)((short2 << 8) | (byte)short2);
            }
            else if (address >= 0xfeff00 && address + 1 <= 0xfeffff)
            {
                result = (short)(mainram3[(address & 0xff)] * 0x100 + mainram3[(address & 0xff) + 1]);
            }
            else if (address >= 0xff0000 && address + 1 <= 0xffffff)
            {
                result = (short)(Memory.mainram[(address & 0xffff)] * 0x100 + Memory.mainram[(address & 0xffff) + 1]);
            }
            return result;
        }
        public static int MCReadLong_sf2m10(int address)
        {
            address &= 0xffffff;
            int result = 0;
            if (address <= 0x3fffff)
            {
                if (address + 3 < Memory.mainrom.Length)
                {
                    result = (int)(Memory.mainrom[address] * 0x1000000 + Memory.mainrom[address + 1] * 0x10000 + Memory.mainrom[address + 2] * 0x100 + Memory.mainrom[address + 3]);
                }
                else
                {
                    result = 0;
                }
            }
            else if (address >= 0x800000 && address + 3 <= 0x800007)
            {
                result = 0;
            }
            else if (address >= 0x800018 && address + 3 <= 0x80001f)
            {
                result = 0;
            }
            else if (address >= 0x800020 && address + 3 <= 0x800021)
            {
                result = 0;
            }
            else if (address >= 0x800140 && address + 3 <= 0x80017f)
            {
                int offset = (address - 0x800140) / 2;
                result = cps1_cps_b_r(offset) * 0x10000 + cps1_cps_b_r(offset + 1);
            }            
            else if (address >= 0x900000 && address + 3 <= 0x92ffff)
            {
                result = (int)(gfxram[(address & 0x3ffff)] * 0x1000000 + gfxram[(address & 0x3ffff) + 1] * 0x10000 + gfxram[(address & 0x3ffff) + 2] * 0x100 + gfxram[(address & 0x3ffff) + 3]);
            }
            else if (address >= 0xe00000 && address + 3 <= 0xefffff)
            {
                result = (int)(mainram2[(address & 0xfffff)] * 0x1000000 + mainram2[(address & 0xfffff) + 1] * 0x10000 + mainram2[(address & 0xfffff) + 2] * 0x100 + mainram2[(address & 0xfffff) + 3]);
            }
            else if (address >= 0xf1c000 && address + 3 <= 0xf1c001)
            {
                result = 0;
            }
            else if (address >= 0xfeff00 && address + 3 <= 0xfeffff)
            {
                result = (int)(mainram3[(address & 0xff)] * 0x1000000 + mainram3[(address & 0xff) + 1] * 0x10000 + mainram3[(address & 0xff) + 2] * 0x100 + mainram3[(address & 0xff) + 3]);
            }
            else if (address >= 0xff0000 && address + 3 <= 0xffffff)
            {
                result = (int)(Memory.mainram[(address & 0xffff)] * 0x1000000 + Memory.mainram[(address & 0xffff) + 1] * 0x10000 + Memory.mainram[(address & 0xffff) + 2] * 0x100 + Memory.mainram[(address & 0xffff) + 3]);
            }
            return result;
        }
        public static void MCWriteByte_sf2m10(int address, sbyte value)
        {
            address &= 0xffffff;
            if (address >= 0x800030 && address <= 0x800037)
            {
                if (address % 2 == 0)
                {
                    cps1_coinctrl_w((ushort)(value * 0x100));
                }
                else
                {
                    return;
                }
            }
            else if (address >= 0x800100 && address <= 0x80013f)
            {
                cps1_cps_a_w(address & 0x3f, (byte)value);
            }
            else if (address >= 0x800140 && address <= 0x80017f)
            {
                return;
            }
            else if (address >= 0x800180 && address <= 0x800187)
            {
                Sound.soundlatch_w((ushort)value);
            }
            else if (address >= 0x800188 && address <= 0x80018f)
            {
                Sound.soundlatch2_w((ushort)value);
            }
            else if (address >= 0x8001a2 && address <= 0x8001b3)
            {
                cps1_cps_a_w(address - 0x8001a2, (byte)value);
            }
            else if (address >= 0x8001fe && address <= 0x8001ff)
            {
                return;
            }
            else if (address >= 0x900000 && address <= 0x92ffff)
            {
                gfxram[(address & 0x3ffff)] = (byte)(value);
                cps1_gfxram_w((address & 0x3ffff) / 2);
            }
            else if (address >= 0xe00000 && address <= 0xefffff)
            {
                mainram2[(address & 0xfffff)] = (byte)(value);
            }
            else if (address >= 0xfeff00 && address <= 0xfeffff)
            {
                mainram3[(address & 0xff)] = (byte)(value);
            }
            else if (address >= 0xff0000 && address <= 0xffffff)
            {
                Memory.mainram[(address & 0xffff)] = (byte)(value);
            }
            else
            {
                return;
            }
        }
        public static void MCWriteWord_sf2m10(int address, short value)
        {
            address &= 0xffffff;
            if (address >= 0x800030 && address + 1 <= 0x800037)
            {
                return;
            }
            else if (address >= 0x800100 && address + 1 <= 0x80013f)
            {
                cps1_cps_a_w((address & 0x3f) / 2, (ushort)value);
            }
            else if (address >= 0x800140 && address + 1 <= 0x80017f)
            {
                cps1_cps_b_w((address & 0x3f) / 2, (ushort)value);
            }
            else if (address >= 0x800180 && address + 1 <= 0x800187)
            {
                Sound.soundlatch_w((ushort)value);
            }
            else if (address >= 0x800188 && address + 1 <= 0x80018f)
            {
                Sound.soundlatch2_w((ushort)value);
            }
            else if (address >= 0x8001a2 && address + 1 <= 0x8001b3)
            {
                cps1_cps_a_w((address - 0x8001a2) / 2, (ushort)value);
            }
            else if (address >= 0x8001fe && address + 1 <= 0x8001ff)
            {
                return;
            }
            else if (address >= 0x900000 && address + 1 <= 0x92ffff)
            {
                gfxram[(address & 0x3ffff)] = (byte)(value >> 8);
                gfxram[(address & 0x3ffff) + 1] = (byte)value;
                cps1_gfxram_w((address & 0x3ffff) / 2);
            }
            else if (address >= 0xe00000 && address + 1 <= 0xefffff)
            {
                mainram2[(address & 0xfffff)] = (byte)(value >> 8);
                mainram2[(address & 0xfffff) + 1] = (byte)(value);
            }
            else if (address >= 0xfeff00 && address + 1 <= 0xfeffff)
            {
                mainram3[(address & 0xff)] = (byte)(value >> 8);
                mainram3[(address & 0xff) + 1] = (byte)(value);
            }
            else if (address >= 0xff0000 && address + 1 <= 0xffffff)
            {
                Memory.mainram[(address & 0xffff)] = (byte)(value >> 8);
                Memory.mainram[(address & 0xffff) + 1] = (byte)(value);
            }
            else
            {
                return;
            }
        }
        public static void MCWriteLong_sf2m10(int address, int value)
        {
            address &= 0xffffff;
            if (address >= 0x800030 && address + 3 <= 0x800037)
            {
                return;
            }
            else if (address >= 0x800100 && address + 3 <= 0x80013f)
            {
                cps1_cps_a_w((address & 0x3f) / 2, (ushort)(value >> 16));
                cps1_cps_a_w(((address + 2) & 0x3f) / 2, (ushort)value);
            }
            else if (address >= 0x800140 && address + 3 <= 0x80017f)
            {
                cps1_cps_b_w((address & 0x3f) / 2, (ushort)(value >> 16));
                cps1_cps_b_w(((address + 2) & 0x3f) / 2, (ushort)value);
            }
            else if (address >= 0x800180 && address + 3 <= 0x800187)
            {
                return;
            }
            else if (address >= 0x800188 && address + 3 <= 0x80018f)
            {
                return;
            }
            else if (address >= 0x8001a2 && address + 3 <= 0x8001b3)
            {
                cps1_cps_a_w((address - 0x8001a2) / 2, (ushort)(value >> 16));
                cps1_cps_a_w((address + 2 - 0x8001a2) / 2, (ushort)value);
            }
            else if (address >= 0x8001fe && address + 3 <= 0x8001ff)
            {
                return;
            }
            else if (address >= 0x900000 && address + 3 <= 0x92ffff)
            {
                gfxram[(address & 0x3ffff)] = (byte)(value >> 24);
                gfxram[(address & 0x3ffff) + 1] = (byte)(value >> 16);
                gfxram[(address & 0x3ffff) + 2] = (byte)(value >> 8);
                gfxram[(address & 0x3ffff) + 3] = (byte)(value);
                cps1_gfxram_w((address & 0x3ffff) / 2);
                cps1_gfxram_w(((address + 2) & 0x3ffff) / 2);
            }
            else if (address >= 0xe00000 && address + 3 <= 0xefffff)
            {
                mainram2[(address & 0xfffff)] = (byte)(value >> 24);
                mainram2[(address & 0xfffff) + 1] = (byte)(value >> 16);
                mainram2[(address & 0xfffff) + 2] = (byte)(value >> 8);
                mainram2[(address & 0xfffff) + 3] = (byte)(value);
            }
            else if (address >= 0xfeff00 && address + 3 <= 0xfeffff)
            {
                mainram3[(address & 0xff)] = (byte)(value >> 24);
                mainram3[(address & 0xff) + 1] = (byte)(value >> 16);
                mainram3[(address & 0xff) + 2] = (byte)(value >> 8);
                mainram3[(address & 0xff) + 3] = (byte)(value);
            }
            else if (address >= 0xff0000 && address + 3 <= 0xffffff)
            {
                Memory.mainram[(address & 0xffff)] = (byte)(value >> 24);
                Memory.mainram[(address & 0xffff) + 1] = (byte)(value >> 16);
                Memory.mainram[(address & 0xffff) + 2] = (byte)(value >> 8);
                Memory.mainram[(address & 0xffff) + 3] = (byte)(value);
            }
            else
            {
                return;
            }
        }
        public static short MCReadWord_sf2dongb(int address)
        {
            address &= 0xffffff;
            short result = 0;
            if (address == 0x180000 || address == 0x1f7040)
            {
                result = 0x0210;
            }
            else
            {
                result = MCReadWord(address);
            }
            return result;
        }
        public static sbyte MCReadByte_dinohunt(int address)
        {
            address &= 0xffffff;
            sbyte result = 0;
            if (address >= 0xf18000 && address <= 0xf19fff)
            {
                result = -1;
            }
            else if (address >= 0xfc0000 && address <= 0xfc0001)
            {
                result = (sbyte)short2;
            }
            else
            {
                result = MCReadByte(address);
            }
            return result;
        }
        public static sbyte MCReadByte_pang3(int address)
        {
            address &= 0xffffff;
            sbyte result = 0;
            if (address >= 0x80017a && address <= 0x80017b)
            {
                result = (sbyte)cps1_eeprom_port_r();
            }
            else
            {
                result = MCReadByte(address);
            }
            return result;
        }
        public static short MCReadWord_pang3(int address)
        {
            address &= 0xffffff;
            short result = 0;
            if (address >= 0x80017a && address + 1 <= 0x80017b)
            {
                result = (short)cps1_eeprom_port_r();
            }
            else
            {
                result = MCReadWord(address);
            }
            return result;
        }
        public static void MCWriteByte_pang3(int address, sbyte value)
        {
            address &= 0xffffff;
            if (address >= 0x80017a && address <= 0x80017b)
            {
                if ((address & 1) == 1)
                {
                    cps1_eeprom_port_w(value);
                }
            }
            else
            {
                MCWriteByte(address, value);
            }
        }
        public static void MCWriteWord_pang3(int address, short value)
        {
            address &= 0xffffff;
            if (address >= 0x80017a && address + 1 <= 0x80017b)
            {
                cps1_eeprom_port_w(value);
            }
            else
            {
                MCWriteWord(address,value);
            }
        }
        public static sbyte MCReadByte_wofsj(int address)
        {
            address &= 0xffffff;
            sbyte result = 0;
            if (address <= 0x3fffff)
            {
                if (address < Memory.mainrom.Length)
                {
                    result = (sbyte)Memory.mainrom[address];
                }
                else
                {
                    result = 0;
                }
            }
            else if (address >= 0x800140 && address <= 0x80017f)
            {
                int offset = (address - 0x800140) / 2;
                result = (sbyte)cps1_cps_b_r(offset);
            }
            else if (address >= 0x880000 && address <= 0x880001)
            {
                result = 0;
            }
            else if (address >= 0x880008 && address <= 0x88000f)
            {
                int offset = (address - 0x880008) / 2;
                result = (sbyte)cps1_dsw_r(offset);
            }
            else if (address >= 0x900000 && address <= 0x92ffff)
            {
                result = (sbyte)gfxram[(address & 0x3ffff)];
            }
            else if (address >= 0xff0000 && address <= 0xffffff)
            {
                result = (sbyte)Memory.mainram[address & 0xffff];
            }
            return result;
        }
        public static short MCReadWord_wofsj(int address)
        {
            address &= 0xffffff;
            short result = 0;
            if (address <= 0x3fffff)
            {
                if (address + 1 < Memory.mainrom.Length)
                {
                    result = (short)(Memory.mainrom[address] * 0x100 + Memory.mainrom[address + 1]);
                }
                else
                {
                    result = 0;
                }
            }
            else if (address >= 0x800140 && address + 1 <= 0x80017f)
            {
                int offset = (address - 0x800140) / 2;
                result = (short)cps1_cps_b_r(offset);
            }
            else if (address >= 0x880000 && address + 1 <= 0x880001)
            {
                result = short1;
            }
            else if (address >= 0x880008 && address + 1 <= 0x88000f)
            {
                int offset = (address - 0x880008) / 2;
                result = (short)(cps1_dsw_r(offset));
            }
            else if (address >= 0x900000 && address + 1 <= 0x92ffff)
            {
                result = (short)(gfxram[(address & 0x3ffff)] * 0x100 + gfxram[(address & 0x3ffff) + 1]);
            }
            else if (address >= 0xff0000 && address + 1 <= 0xffffff)
            {
                result = (short)(Memory.mainram[(address & 0xffff)] * 0x100 + Memory.mainram[(address & 0xffff) + 1]);
            }
            return result;
        }
        public static void MCWriteWord_wofsj(int address, short value)
        {
            address &= 0xffffff;
            if (address == 0x800030)
            {
                cps1_coinctrl_w((ushort)value);
            }
            else if (address >= 0x800100 && address + 1 <= 0x80013f)
            {
                cps1_cps_a_w((address & 0x3f) / 2, (ushort)value);
            }
            else if (address >= 0x800140 && address + 1 <= 0x80017f)
            {
                cps1_cps_b_w((address & 0x3f) / 2, (ushort)value);
            }
            else if (address >= 0x880006 && address + 1 <= 0x880007)
            {
                Sound.soundlatch_w((ushort)value);
            }
            else if (address >= 0x890000 && address + 1 <= 0x890001)
            {
                Sound.soundlatch2_w((ushort)value);
            }
            else if (address >= 0x900000 && address + 1 <= 0x92ffff)
            {
                gfxram[(address & 0x3ffff)] = (byte)(value >> 8);
                gfxram[(address & 0x3ffff) + 1] = (byte)value;
                cps1_gfxram_w((address & 0x3ffff) / 2);
            }
            else if (address >= 0xff0000 && address + 1 <= 0xffffff)
            {
                Memory.mainram[(address & 0xffff)] = (byte)(value >> 8);
                Memory.mainram[(address & 0xffff) + 1] = (byte)(value);
            }
        }
        public static sbyte MCReadByte_sgyxz(int address)
        {
            address &= 0xffffff;
            sbyte result = 0;
            if (address <= 0x3fffff)
            {
                if (address < Memory.mainrom.Length)
                {
                    result = (sbyte)Memory.mainrom[address];
                }
                else
                {
                    result = 0;
                }
            }
            else if (address >= 0x800100 && address <= 0x80013f)
            {
                int offset = (address - 0x800100) / 2;
                if (address % 2 == 0)
                {
                    result = (sbyte)(cps_a_regs[offset] >> 8);
                }
                else
                {
                    result = (sbyte)cps_a_regs[offset];
                }
            }
            else if (address >= 0x800140 && address <= 0x80017f)
            {
                int offset = (address - 0x800140) / 2;
                result = (sbyte)cps1_cps_b_r(offset);
            }
            else if (address >= 0x880000 && address <= 0x880001)
            {
                result = 0;//short1
            }
            else if (address >= 0x880006 && address <= 0x88000d)
            {
                int offset = (address - 0x880006) / 2;
                result = (sbyte)cps1_dsw_r(offset);
            }
            else if (address >= 0x880e78 && address <= 0x880e79)
            {
                result = (sbyte)short2;
            }
            else if (address >= 0x900000 && address <= 0x92ffff)
            {
                result = (sbyte)gfxram[(address & 0x3ffff)];
            }
            else if (address >= 0xf1c006 && address <= 0xf1c007)
            {
                result = (sbyte)cps1_eeprom_port_r();//EEPROMIN
            }
            else if (address >= 0xff0000 && address <= 0xffffff)
            {
                result = (sbyte)Memory.mainram[address & 0xffff];
            }
            return result;
        }
        public static short MCReadWord_sgyxz(int address)
        {
            address &= 0xffffff;
            short result = 0;
            if (address <= 0x3fffff)
            {
                if (address + 1 < Memory.mainrom.Length)
                {
                    result = (short)(Memory.mainrom[address] * 0x100 + Memory.mainrom[address + 1]);
                }
                else
                {
                    result = 0;
                }
            }
            else if (address >= 0x800100 && address + 1 <= 0x80013f)
            {
                int offset = (address - 0x800100) / 2;
                result = (short)cps_a_regs[offset];
            }
            else if (address >= 0x800140 && address + 1 <= 0x80017f)
            {
                int offset = (address - 0x800140) / 2;
                result = (short)cps1_cps_b_r(offset);
            }
            else if (address >= 0x880000 && address + 1 <= 0x880001)
            {
                result = short1;
            }
            else if (address >= 0x880006 && address + 1 <= 0x88000d)
            {
                int offset = (address - 0x880006) / 2;
                result = (short)cps1_dsw_r(offset);
            }
            else if (address >= 0x880e78 && address + 1 <= 0x880e79)
            {
                result = short2;
            }
            else if (address >= 0x900000 && address + 1 <= 0x92ffff)
            {
                result = (short)(gfxram[(address & 0x3ffff)] * 0x100 + gfxram[(address & 0x3ffff) + 1]);
            }
            else if (address >= 0xf1c006 && address + 1 <= 0xf1c007)
            {
                result = (short)cps1_eeprom_port_r();//EEPROMIN
            }
            else if (address >= 0xff0000 && address + 1 <= 0xffffff)
            {
                result = (short)(Memory.mainram[(address & 0xffff)] * 0x100 + Memory.mainram[(address & 0xffff) + 1]);
            }
            return result;
        }
        public static void MCWriteByte_sgyxz(int address, sbyte value)
        {
            address &= 0xffffff;
            if (address >= 0x800030 && address <= 0x800037)
            {
                if (address % 2 == 0)
                {
                    cps1_coinctrl_w((ushort)(value * 0x100));
                }
                else
                {
                    return;
                }
            }
            else if (address >= 0x800100 && address <= 0x80013f)
            {
                cps1_cps_a_w(address & 0x3f, (byte)value);
            }
            else if (address >= 0x800140 && address <= 0x80017f)
            {
                return;
            }
            else if (address >= 0x88000e && address <= 0x88000f)
            {
                if (address == 0x88000e)
                {
                    Sound.soundlatch_w((ushort)value);
                }
                else
                {
                    return;
                }
            }
            else if (address >= 0x890000 && address <= 0x890001)
            {
                if (address == 0x88000e)
                {
                    Sound.soundlatch2_w((ushort)value);
                }
                else
                {
                    return;
                }
            }
            else if (address >= 0x900000 && address <= 0x92ffff)
            {
                gfxram[(address & 0x3ffff)] = (byte)(value);
                cps1_gfxram_w((address & 0x3ffff) / 2);
            }
            else if (address >= 0xf1c004 && address <= 0xf1c005)
            {
                return;//cpsq_coinctrl2_w
            }
            else if (address >= 0xf1c006 && address <= 0xf1c007)
            {
                if ((address % 2) == 1)
                {
                    cps1_eeprom_port_w(value);
                }
                else
                {
                    return;
                }
            }
            else if (address >= 0xff0000 && address <= 0xffffff)
            {
                Memory.mainram[(address & 0xffff)] = (byte)(value);
            }
            else
            {
                return;
            }
        }
        public static void MCWriteWord_sgyxz(int address, short value)
        {
            address &= 0xffffff;
            if (address == 0x800030)
            {
                cps1_coinctrl_w((ushort)value);
            }
            else if (address >= 0x800100 && address + 1 <= 0x80013f)
            {
                cps1_cps_a_w((address & 0x3f) / 2, (ushort)value);
            }
            else if (address >= 0x800140 && address + 1 <= 0x80017f)
            {
                cps1_cps_b_w((address & 0x3f) / 2, (ushort)value);
            }
            else if (address >= 0x88000e && address + 1 <= 0x88000f)
            {
                Sound.soundlatch_w((ushort)value);
            }
            else if (address >= 0x890000 && address + 1 <= 0x890001)
            {
                Sound.soundlatch2_w((ushort)value);
            }
            else if (address >= 0x900000 && address + 1 <= 0x92ffff)
            {
                gfxram[(address & 0x3ffff)] = (byte)(value >> 8);
                gfxram[(address & 0x3ffff) + 1] = (byte)value;
                cps1_gfxram_w((address & 0x3ffff) / 2);
            }
            else if (address >= 0xf1c004 && address + 1 <= 0xf1c005)
            {
                return;//cpsq_coinctrl2_w
            }
            else if (address >= 0xf1c006 && address + 1 <= 0xf1c007)
            {
                cps1_eeprom_port_w(value);
            }
            else if (address >= 0xff0000 && address + 1 <= 0xffffff)
            {
                Memory.mainram[(address & 0xffff)] = (byte)(value >> 8);
                Memory.mainram[(address & 0xffff) + 1] = (byte)(value);
            }
            else
            {
                return;
            }
        }
    }
}
