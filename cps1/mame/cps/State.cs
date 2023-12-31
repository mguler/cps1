﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using cpu.m68000;
using cpu.z80;

namespace mame
{
    public partial class CPS
    {
        public static void SaveStateBinaryC(BinaryWriter writer)
        {
            int i;
            writer.Write(dswa);
            writer.Write(dswb);
            writer.Write(dswc);
            writer.Write(basebanksnd);
            for (i = 0; i < 0x20; i++)
            {
                writer.Write(cps_a_regs[i]);
            }
            for (i = 0; i < 0x20; i++)
            {
                writer.Write(cps_b_regs[i]);
            }
            for (i = 0; i < 0xc00; i++)
            {
                writer.Write(Palette.entry_color[i]);
            }
            writer.Write(Memory.mainram, 0, 0x10000);
            writer.Write(gfxram, 0, 0x30000);
            MC68000.m1.SaveStateBinary(writer);
            writer.Write(Memory.audioram, 0, 0x800);
            Z80A.z1.SaveStateBinary(writer);
            Cpuint.SaveStateBinary(writer);
            writer.Write(Timer.global_basetime.seconds);
            writer.Write(Timer.global_basetime.attoseconds);
            writer.Write(Video.screenstate.frame_number);
            writer.Write(Sound.last_update_second);
            for (i = 0; i < 2; i++)
            {
                writer.Write(Cpuexec.cpu[i].suspend);
                writer.Write(Cpuexec.cpu[i].nextsuspend);
                writer.Write(Cpuexec.cpu[i].eatcycles);
                writer.Write(Cpuexec.cpu[i].nexteatcycles);
                writer.Write(Cpuexec.cpu[i].localtime.seconds);
                writer.Write(Cpuexec.cpu[i].localtime.attoseconds);
            }
            Timer.SaveStateBinary(writer);
            YM2151.SaveStateBinary(writer);
            OKI6295.SaveStateBinary(writer);
            for (i = 0; i < 2; i++)
            {
                writer.Write(Sound.latched_value[i]);
            }
            for (i = 0; i < 2; i++)
            {
                writer.Write(Sound.utempdata[i]);
            }
            writer.Write(Sound.ym2151stream.output_sampindex);
            writer.Write(Sound.ym2151stream.output_base_sampindex);
            writer.Write(Sound.okistream.output_sampindex);
            writer.Write(Sound.okistream.output_base_sampindex);
            writer.Write(Sound.mixerstream.output_sampindex);
            writer.Write(Sound.mixerstream.output_base_sampindex);
        }
        public static void SaveStateBinaryQ(BinaryWriter writer)
        {
            int i;
            writer.Write(dswa);
            writer.Write(dswb);
            writer.Write(dswc);
            writer.Write(basebanksnd);
            for (i = 0; i < 0x20; i++)
            {
                writer.Write(cps_a_regs[i]);
            }
            for (i = 0; i < 0x20; i++)
            {
                writer.Write(cps_b_regs[i]);
            }
            for (i = 0; i < 0xc00; i++)
            {
                writer.Write(Palette.entry_color[i]);
            }
            writer.Write(Memory.mainram, 0, 0x10000);
            writer.Write(gfxram, 0, 0x30000);
            MC68000.m1.SaveStateBinary(writer);
            writer.Write(Memory.audioram, 0, 0x800);
            Z80A.z1.SaveStateBinary(writer);
            Cpuint.SaveStateBinary(writer);
            writer.Write(Timer.global_basetime.seconds);
            writer.Write(Timer.global_basetime.attoseconds);
            writer.Write(Video.screenstate.frame_number);
            writer.Write(Sound.last_update_second);
            for (i = 0; i < 2; i++)
            {
                writer.Write(Cpuexec.cpu[i].suspend);
                writer.Write(Cpuexec.cpu[i].nextsuspend);
                writer.Write(Cpuexec.cpu[i].eatcycles);
                writer.Write(Cpuexec.cpu[i].nexteatcycles);
                writer.Write(Cpuexec.cpu[i].localtime.seconds);
                writer.Write(Cpuexec.cpu[i].localtime.attoseconds);
            }
            Timer.SaveStateBinary(writer);
            writer.Write(qsound_sharedram1);
            writer.Write(qsound_sharedram2);
            QSound.SaveStateBinary(writer);
            writer.Write(Sound.qsoundstream.output_sampindex);
            writer.Write(Sound.qsoundstream.output_base_sampindex);
            writer.Write(Sound.mixerstream.output_sampindex);
            writer.Write(Sound.mixerstream.output_base_sampindex);
            writer.Write(Eeprom.eeprom_data);
            writer.Write(Eeprom.serial_buffer);
            writer.Write((int)Eeprom.clock_line);
            writer.Write((int)Eeprom.reset_line);
            writer.Write(Eeprom.locked);
            writer.Write(Eeprom.serial_count);
            writer.Write(Eeprom.latch);
            writer.Write(Eeprom.sending);
            writer.Write(Eeprom.eeprom_data_bits);
        }
        public static void LoadStateBinaryC(BinaryReader reader)
        {
            int i;
            dswa = reader.ReadByte();
            dswb = reader.ReadByte();
            dswc = reader.ReadByte();
            basebanksnd = reader.ReadInt32();
            for (i = 0; i < 0x20; i++)
            {
                cps_a_regs[i] = reader.ReadUInt16();
            }
            for (i = 0; i < 0x20; i++)
            {
                cps_b_regs[i] = reader.ReadUInt16();
            }
            for (i = 0; i < 0xc00; i++)
            {
                Palette.entry_color[i] = reader.ReadUInt32();
            }
            Memory.mainram = reader.ReadBytes(0x10000);
            gfxram = reader.ReadBytes(0x30000);
            MC68000.m1.LoadStateBinary(reader);
            Memory.audioram = reader.ReadBytes(0x800);
            Z80A.z1.LoadStateBinary(reader);
            Cpuint.LoadStateBinary(reader);
            Timer.global_basetime.seconds = reader.ReadInt32();
            Timer.global_basetime.attoseconds = reader.ReadInt64();
            Video.screenstate.frame_number = reader.ReadInt64();
            Sound.last_update_second = reader.ReadInt32();
            for (i = 0; i < 2; i++)
            {
                Cpuexec.cpu[i].suspend = reader.ReadByte();
                Cpuexec.cpu[i].nextsuspend = reader.ReadByte();
                Cpuexec.cpu[i].eatcycles = reader.ReadByte();
                Cpuexec.cpu[i].nexteatcycles = reader.ReadByte();
                Cpuexec.cpu[i].localtime.seconds = reader.ReadInt32();
                Cpuexec.cpu[i].localtime.attoseconds = reader.ReadInt64();
            }
            Timer.LoadStateBinary(reader);
            YM2151.LoadStateBinary(reader);
            OKI6295.LoadStateBinary(reader);
            for (i = 0; i < 2; i++)
            {
                Sound.latched_value[i] = reader.ReadUInt16();
            }
            for (i = 0; i < 2; i++)
            {
                Sound.utempdata[i] = reader.ReadUInt16();
            }
            Sound.ym2151stream.output_sampindex = reader.ReadInt32();
            Sound.ym2151stream.output_base_sampindex = reader.ReadInt32();
            Sound.okistream.output_sampindex = reader.ReadInt32();
            Sound.okistream.output_base_sampindex = reader.ReadInt32();
            Sound.mixerstream.output_sampindex = reader.ReadInt32();
            Sound.mixerstream.output_base_sampindex = reader.ReadInt32();
        }
        public static void LoadStateBinaryQ(BinaryReader reader)
        {
            int i;
            dswa = reader.ReadByte();
            dswb = reader.ReadByte();
            dswc = reader.ReadByte();
            basebanksnd = reader.ReadInt32();
            for (i = 0; i < 0x20; i++)
            {
                cps_a_regs[i] = reader.ReadUInt16();
            }
            for (i = 0; i < 0x20; i++)
            {
                cps_b_regs[i] = reader.ReadUInt16();
            }
            for (i = 0; i < 0xc00; i++)
            {
                Palette.entry_color[i] = reader.ReadUInt32();
            }
            Memory.mainram = reader.ReadBytes(0x10000);
            gfxram = reader.ReadBytes(0x30000);
            MC68000.m1.LoadStateBinary(reader);
            Memory.audioram = reader.ReadBytes(0x800);
            Z80A.z1.LoadStateBinary(reader);
            Cpuint.LoadStateBinary(reader);
            Timer.global_basetime.seconds = reader.ReadInt32();
            Timer.global_basetime.attoseconds = reader.ReadInt64();
            Video.screenstate.frame_number = reader.ReadInt64();
            Sound.last_update_second = reader.ReadInt32();
            for (i = 0; i < 2; i++)
            {
                Cpuexec.cpu[i].suspend = reader.ReadByte();
                Cpuexec.cpu[i].nextsuspend = reader.ReadByte();
                Cpuexec.cpu[i].eatcycles = reader.ReadByte();
                Cpuexec.cpu[i].nexteatcycles = reader.ReadByte();
                Cpuexec.cpu[i].localtime.seconds = reader.ReadInt32();
                Cpuexec.cpu[i].localtime.attoseconds = reader.ReadInt64();
            }
            Timer.LoadStateBinary(reader);
            qsound_sharedram1 = reader.ReadBytes(0x1000);
            qsound_sharedram2 = reader.ReadBytes(0x1000);
            QSound.LoadStateBinary(reader);
            Sound.qsoundstream.output_sampindex = reader.ReadInt32();
            Sound.qsoundstream.output_base_sampindex = reader.ReadInt32();
            Sound.mixerstream.output_sampindex = reader.ReadInt32();
            Sound.mixerstream.output_base_sampindex = reader.ReadInt32();
            Eeprom.eeprom_data = reader.ReadBytes(0x80);
            Eeprom.serial_buffer = reader.ReadBytes(40);
            Eeprom.clock_line = (LineState)reader.ReadInt32();
            Eeprom.reset_line = (LineState)reader.ReadInt32();
            Eeprom.locked = reader.ReadInt32();
            Eeprom.serial_count = reader.ReadInt32();
            Eeprom.latch = reader.ReadInt32();
            Eeprom.sending = reader.ReadInt32();
            Eeprom.eeprom_data_bits = reader.ReadInt32();
        }
    }
}
