using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ui;
using cpu.m68000;
using cpu.z80;

namespace mame
{
    public partial class CPS
    {
        public static short short1, short2;
        public static sbyte sbyte0, sbyte3;
        public static short short1_old, short2_old;
        public static sbyte sbyte0_old, sbyte3_old;
        public static sbyte MCReadOpByte(int address)
        {
            address &= 0xffffff;
            sbyte result = 0;
            if (address <= 0x3fffff)
            {
                if (address < Memory.mainrom.Length)
                {
                    result= (sbyte)(Memory.mainrom[address]);
                }
                else
                {
                    result= 0;
                }
            }
            return result;
        }
        public static sbyte MCReadByte(int address)
        {
            address &= 0xffffff;
            sbyte result = 0;
            if (address <= 0x3fffff)
            {
                if (address < Memory.mainrom.Length)
                {
                    result= (sbyte)Memory.mainrom[address];
                }
                else
                {
                    result=0;
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
                if (address % 2 == 0)
                {
                    result = (sbyte)(cps1_cps_b_r(offset) >> 8);
                }
                else if (address % 2 == 1)
                {
                    result = (sbyte)cps1_cps_b_r(offset);
                }
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
        public static short MCReadOpWord(int address)
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
        public static short MCReadWord(int address)
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
                result = (short)(((byte)(cps1_dsw_r(offset)) << 8) | (byte)cps1_dsw_r(offset));
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
            else if (address >= 0xff0000 && address + 1 <= 0xffffff)
            {
                result = (short)(Memory.mainram[(address & 0xffff)] * 0x100 + Memory.mainram[(address & 0xffff) + 1]);
            }
            return result;
        }
        public static int MCReadOpLong(int address)
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
        public static int MCReadLong(int address)
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
            else if (address >= 0xff0000 && address + 3 <= 0xffffff)
            {
                result = (int)(Memory.mainram[(address & 0xffff)] * 0x1000000 + Memory.mainram[(address & 0xffff) + 1] * 0x10000 + Memory.mainram[(address & 0xffff) + 2] * 0x100 + Memory.mainram[(address & 0xffff) + 3]);
            }
            return result;
        }
        public static void MCWriteByte(int address, sbyte value)
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
            else if (address >= 0x800180 && address <= 0x800187)
            {
                Sound.soundlatch_w((ushort)value);
            }
            else if (address >= 0x800188 && address <= 0x80018f)
            {
                Sound.soundlatch2_w((ushort)value);
            }
            else if (address >= 0x900000 && address <= 0x92ffff)
            {
                gfxram[(address & 0x3ffff)] = (byte)(value);
                cps1_gfxram_w((address & 0x3ffff) / 2);
            }
            else if (address >= 0xff0000 && address <= 0xffffff)
            {
                /*if (m68000Form.iStatus == 0 && address == 0xff8ffb)
                {
                    m68000Form.iStatus = 1;
                }*/
                Memory.mainram[(address & 0xffff)] = (byte)(value);
            }
            else
            {
                return;
            }
        }
        public static void MCWriteWord(int address, short value)
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
        public static void MCWriteLong(int address, int value)
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
        public static byte ZCReadOp(ushort address)
        {
            byte result = 0;
            if (address <= 0x7fff)
            {
                result = Memory.audiorom[address & 0x7fff];
            }
            else
            {
                result = 0;
            }
            return result;
        }
        public static byte ZCReadMemory(ushort address)
        {
            byte result = 0;
            if (address < 0x8000)
            {
                result = Memory.audiorom[address & 0x7fff];
            }
            else if (address >= 0x8000 && address <= 0xbfff)
            {
                result = Memory.audiorom[basebanksnd + (address & 0x3fff)];
            }
            else if (address >= 0xd000 && address <= 0xd7ff)
            {
                result = Memory.audioram[address & 0x7ff];
            }
            else if (address == 0xf001)
            {
                result = YM2151.ym2151_status_port_0_r();
            }
            else if (address == 0xf002)
            {
                result = OKI6295.okim6295_status_0_r();
            }
            else if (address == 0xf008)
            {
                result = (byte)Sound.soundlatch_r();
            }
            else if (address == 0xf00a)
            {
                result = (byte)Sound.soundlatch2_r();
            }
            else
            {
                result = 0;
            }
            return result;
        }
        public static void ZCWriteMemory(ushort address, byte value)
        {
            if (address >= 0xd000 && address <= 0xd7ff)
            {
                Memory.audioram[address & 0x7ff] = value;
            }
            else if (address == 0xf000)
            {
                YM2151.ym2151_register_port_0_w(value);
            }
            else if (address == 0xf001)
            {
                YM2151.ym2151_data_port_0_w(value);
            }
            else if (address == 0xf002)
            {
                OKI6295.okim6295_data_0_w(value);
            }
            else if (address == 0xf004)
            {
                cps1_snd_bankswitch_w(value);
            }
            else if (address == 0xf006)
            {
                cps1_oki_pin7_w(value);
            }
            else
            {

            }
        }
        public static sbyte MQReadOpByte(int address)
        {
            address &= 0xffffff;
            sbyte result = 0;
            if (address <= 0x3fffff)
            {
                if (address < Memory.mainrom.Length)
                {
                    result = (sbyte)(Memory.mainrom[address]);
                }
                else
                {
                    result = 0;
                }
            }
            else if (address >= 0x900000 && address <= 0x92ffff)
            {
                return (sbyte)gfxram[(address & 0x3ffff)];
            }
            return result;
        }
        public static sbyte MQReadByte(int address)
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
            else if (address >= 0x800140 && address <= 0x80017f)
            {
                int offset = (address - 0x800140) / 2;
                result = (sbyte)cps1_cps_b_r(offset);
            }
            else if (address >= 0x900000 && address <= 0x92ffff)
            {
                result = (sbyte)gfxram[(address & 0x3ffff)];
            }
            else if (address >= 0xf00000 && address <= 0xf0ffff)
            {
                int offset = (address - 0xf00000) / 2;
                if (address % 2 == 0)
                {
                    result = (sbyte)(qsound_rom_r(offset) >> 8);
                }
                else if (address % 2 == 1)
                {
                    result = (sbyte)qsound_rom_r(offset);
                }
            }
            else if (address >= 0xf18000 && address <= 0xf19fff)
            {
                int offset = (address - 0xf18000) / 2;
                result = (sbyte)qsound_sharedram1_r(offset);
            }
            else if (address >= 0xf1c000 && address <= 0xf1c001)
            {
                result = (sbyte)short2;
            }
            else if (address >= 0xf1c002 && address <= 0xf1c003)
            {
                result = sbyte3;
            }
            else if (address >= 0xf1c006 && address <= 0xf1c007)
            {
                result = (sbyte)cps1_eeprom_port_r();
            }
            else if (address >= 0xf1e000 && address <= 0xf1ffff)
            {
                int offset = (address - 0xf1e000) / 2;
                result = (sbyte)qsound_sharedram2_r(offset);
            }
            else if (address >= 0xff0000 && address <= 0xffffff)
            {
                result = (sbyte)Memory.mainram[address & 0xffff];
            }
            return result;
        }
        public static short MQReadOpWord(int address)
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
        public static short MQReadWord(int address)
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
                result = cps1_dsw_r(offset);
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
            else if (address >= 0xf00000 && address + 1 <= 0xf0ffff)
            {
                int offset = (address - 0xf00000) / 2;
                result = qsound_rom_r(offset);
            }
            else if (address >= 0xf18000 && address + 1 <= 0xf19fff)
            {
                int offset = (address - 0xf18000) / 2;
                result = qsound_sharedram1_r(offset);
            }
            else if (address >= 0xf1c000 && address + 1 <= 0xf1c001)
            {
                result = (short)((int)short2 & 0xff);
            }
            else if (address >= 0xf1c002 && address + 1 <= 0xf1c003)
            {
                result = (short)((int)sbyte3 & 0xff);
            }
            else if (address >= 0xf1c006 && address + 1 <= 0xf1c007)
            {
                result = (short)cps1_eeprom_port_r();
            }
            else if (address >= 0xf1e000 && address + 1 <= 0xf1ffff)
            {
                int offset = (address - 0xf1e000) / 2;
                result = qsound_sharedram2_r(offset);
            }
            else if (address >= 0xff0000 && address + 1 <= 0xffffff)
            {
                result = (short)(Memory.mainram[(address & 0xffff)] * 0x100 + Memory.mainram[(address & 0xffff) + 1]);
            }
            return result;
        }
        public static int MQReadOpLong(int address)
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
        public static int MQReadLong(int address)
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
                result = -1;//short1
            }
            else if (address >= 0x800018 && address + 3 <= 0x80001f)
            {
                result = 0;//cps1_dsw_r
            }
            else if (address >= 0x800140 && address + 3 <= 0x80017f)
            {
                result = 0;//cps1_cps_b_r
            }
            else if (address >= 0x900000 && address + 3 <= 0x92ffff)
            {
                result = (int)(gfxram[(address & 0x3ffff)] * 0x1000000 + gfxram[(address & 0x3ffff) + 1] * 0x10000 + gfxram[(address & 0x3ffff) + 2] * 0x100 + gfxram[(address & 0x3ffff) + 3]);
            }
            else if (address >= 0xf00000 && address + 3 <= 0xf0ffff)
            {
                result = 0;//qsound_rom_r
            }
            else if (address >= 0xf18000 && address + 3 <= 0xf19fff)
            {
                result = 0;//qsound_sharedram1_r
            }
            else if (address >= 0xf1c000 && address + 3 <= 0xf1c001)
            {
                result = (int)short2 & 0xff;
            }
            else if (address >= 0xf1c002 && address + 3 <= 0xf1c003)
            {
                result = (int)sbyte3 & 0xff;
            }
            else if (address >= 0xf1c006 && address + 3 <= 0xf1c007)
            {
                result = 0;//cps1_eeprom_port_r();
            }
            else if (address >= 0xf1e000 && address + 3 <= 0xf1ffff)
            {
                result = 0;//qsound_sharedram2_r
            }
            else if (address >= 0xff0000 && address + 3 <= 0xffffff)
            {
                result = (int)(Memory.mainram[(address & 0xffff)] * 0x1000000 + Memory.mainram[(address & 0xffff) + 1] * 0x10000 + Memory.mainram[(address & 0xffff) + 2] * 0x100 + Memory.mainram[(address & 0xffff) + 3]);
            }
            return result;
        }
        public static void MQWriteByte(int address, sbyte value)
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
                    
                }
            }
            else if (address >= 0x800100 && address <= 0x80013f)
            {
                return;//cps1_cps_a_w
            }
            else if (address >= 0x800140 && address <= 0x80017f)
            {
                return;//cps1_cps_b_w
            }
            else if (address >= 0x900000 && address <= 0x92ffff)
            {
                gfxram[(address & 0x3ffff)] = (byte)(value);
                cps1_gfxram_w((address & 0x3ffff) / 2);
            }
            else if (address >= 0xf18000 && address <= 0xf19fff)
            {
                int offset = (address - 0xf18000) / 2;
                if ((address & 1) == 1)
                {
                    qsound_sharedram1_w(offset, (byte)value);
                }
                else
                {
                    return;
                }
            }
            else if (address >= 0xf1c004 && address <= 0xf1c005)
            {
                return;//cpsq_coinctrl2_w
            }
            else if (address >= 0xf1c006 && address <= 0xf1c007)
            {
                if ((address & 1) == 1)
                {
                    cps1_eeprom_port_w(value);
                }
            }
            else if (address >= 0xf1e000 && address <= 0xf1ffff)
            {
                int offset = (address - 0xf1e000) / 2;
                if ((address & 1) == 1)
                {
                    qsound_sharedram2_w(offset, (byte)value);
                }
                else
                {
                    return;
                }
            }
            else if (address >= 0xff0000 && address <= 0xffffff)
            {
                /*if (address == 0xff5d96)
                {
                    int i = MC68000.m1.PPC;
                }*/
                Memory.mainram[(address & 0xffff)] = (byte)(value);
            }
            else
            {
                return;
            }
        }
        public static void MQWriteWord(int address, short value)
        {
            address &= 0xffffff;
            if (address >= 0x800030 && address + 1 <= 0x800037)
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
            else if (address >= 0x800100 && address + 1 <= 0x80013f)
            {
                cps1_cps_a_w((address & 0x3f) / 2, (ushort)value);
            }
            else if (address >= 0x800140 && address + 1 <= 0x80017f)
            {
                cps1_cps_b_w((address & 0x3f) / 2, (ushort)value);
            }
            else if (address >= 0x900000 && address + 1 <= 0x92ffff)
            {
                gfxram[(address & 0x3ffff)] = (byte)(value >> 8);
                gfxram[(address & 0x3ffff) + 1] = (byte)value;
                cps1_gfxram_w((address & 0x3ffff) / 2);
            }
            else if (address >= 0xf18000 && address + 1 <= 0xf19fff)
            {
                qsound_sharedram1_w((address - 0xf18000) >> 1, (byte)value);
            }
            else if (address >= 0xf1c004 && address + 1 <= 0xf1c005)
            {
                cpsq_coinctrl2_w((ushort)value);
            }
            else if (address >= 0xf1c006 && address + 1 <= 0xf1c007)
            {
                cps1_eeprom_port_w(value);
            }
            else if (address >= 0xf1e000 && address + 1 <= 0xf1ffff)
            {
                qsound_sharedram2_w((address - 0xf1e000) >> 1, (byte)value);
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
        public static void MQWriteLong(int address, int value)
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
                return;//cps1_cps_b_w
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
            else if (address >= 0xf18000 && address + 3 <= 0xf19fff)
            {
                return;//qsound_sharedram1_w
            }
            else if (address >= 0xf1c004 && address + 3 <= 0xf1c005)
            {
                return;//cpsq_coinctrl2_w
            }
            else if (address >= 0xf1c006 && address + 3 <= 0xf1c007)
            {
                return;//cps1_eeprom_port_w
            }
            else if (address >= 0xf1e000 && address + 3 <= 0xf1ffff)
            {
                return;//qsound_sharedram2_w
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
        public static byte ZQReadOp(ushort address)
        {
            byte result = 0;
            if (address <= 0x7fff)
            {
                result = audioromop[address & 0x7fff];
            }
            else
            {
                result = 0;
            }
            return result;
        }
        public static byte ZQReadMemory(ushort address)
        {
            byte result = 0;
            if (address <= 0x7fff)
            {
                result = Memory.audiorom[address & 0x7fff];
            }
            else if (address >= 0x8000 && address <= 0xbfff)
            {
                result = Memory.audiorom[basebanksnd + (address & 0x3fff)];
            }
            else if (address >= 0xc000 && address <= 0xcfff)
            {
                result = qsound_sharedram1[address & 0xfff];
            }
            else if (address == 0xd007)
            {
                result = QSound.qsound_status_r();
            }
            else if (address >= 0xf000 && address <= 0xffff)
            {
                result = qsound_sharedram2[address & 0xfff];
            }
            else
            {
                result = 0;
            }
            return result;
        }
        public static void ZQWriteMemory(ushort address, byte value)
        {
            if (address >= 0xc000 && address <= 0xcfff)
            {
                qsound_sharedram1[address & 0xfff] = value;
            }
            else if (address == 0xd000)
            {
                QSound.qsound_data_h_w(value);
            }
            else if (address == 0xd001)
            {
                QSound.qsound_data_l_w(value);
            }
            else if (address == 0xd002)
            {
                QSound.qsound_cmd_w(value);
            }
            else if (address == 0xd003)
            {
                qsound_banksw_w(value);
            }
            else if (address >= 0xf000 && address <= 0xffff)
            {
                qsound_sharedram2[address & 0xfff] = value;
            }
            else
            {
                return;
            }
        }
        public static byte ZCReadHardware(ushort address)
        {
            return 0;
        }
        public static void ZCWriteHardware(ushort address, byte value)
        {

        }
        public static int ZIRQCallback()
        {
            return Cpuint.cpu_irq_callback(Z80A.z1.cpunum, 0);
        }
    }
}
