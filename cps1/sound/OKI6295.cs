﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace mame
{
    public class OKI6295
    {
        public struct adpcm_state
        {
            public int signal;
            public int step;
        };
        public struct ADPCMVoice
        {
            public bool playing;			/* 1 if we are actively playing */
            public uint base_offset;		/* pointer to the base memory location */
            public uint sample;			/* current sample number */
            public uint count;			/* total samples to play */
            public uint volume;			/* output volume */
        };
        public struct okim6295Struct
        {
            public ADPCMVoice[] voice;
            public int command;
            public int bank_offset;
            public sound_stream stream;	/* which stream are we playing on? */
            public uint master_clock;	/* master clock frequency */
        };
        public static byte[] okirom;
        public static okim6295Struct OKI;
        public static adpcm_state[] adpcm;
        private static int[] index_shift = new int[8] { -1, -1, -1, -1, 2, 4, 6, 8 };
        /* lookup table for the precomputed difference */
        private static int[] diff_lookup = new int[49 * 16];
        /* volume lookup table */
        private static uint[] volume_table = new uint[16];
        /* tables computed? */
        private static int tables_computed = 0;
        private static void compute_tables()
        {
            /* nibble to bit map */
            int[,] nbl2bit = new int[16, 4]
	        {
		        { 1, 0, 0, 0}, { 1, 0, 0, 1}, { 1, 0, 1, 0}, { 1, 0, 1, 1},
		        { 1, 1, 0, 0}, { 1, 1, 0, 1}, { 1, 1, 1, 0}, { 1, 1, 1, 1},
		        {-1, 0, 0, 0}, {-1, 0, 0, 1}, {-1, 0, 1, 0}, {-1, 0, 1, 1},
		        {-1, 1, 0, 0}, {-1, 1, 0, 1}, {-1, 1, 1, 0}, {-1, 1, 1, 1}
	        };
            int step, nib;
            /* loop over all possible steps */
            for (step = 0; step <= 48; step++)
            {
                /* compute the step value */
                int stepval = (int)Math.Floor(16.0 * Math.Pow(11.0 / 10.0, (double)step));

                /* loop over all nibbles and compute the difference */
                for (nib = 0; nib < 16; nib++)
                {
                    diff_lookup[step * 16 + nib] = nbl2bit[nib, 0] *
                        (stepval * nbl2bit[nib, 1] +
                         stepval / 2 * nbl2bit[nib, 2] +
                         stepval / 4 * nbl2bit[nib, 3] +
                         stepval / 8);
                }
            }
            /* generate the OKI6295 volume table */
            for (step = 0; step < 16; step++)
            {
                double dout = 256.0;
                int vol = step;
                /* 3dB per step */
                while (vol-- > 0)
                    dout /= 1.412537545;	/* = 10 ^ (3/20) = 3dB */
                volume_table[step] = (uint)dout;
            }
            tables_computed = 1;
        }
        private static void reset_adpcm(int i)
        {
            if (tables_computed == 0)
            {
                compute_tables();
            }
            adpcm[i].signal = -2;
            adpcm[i].step = 0;
        }
        private static short clock_adpcm(int i, byte nibble)
        {
            adpcm[i].signal += diff_lookup[adpcm[i].step * 16 + (nibble & 15)];
            /* clamp to the maximum */
            if (adpcm[i].signal > 2047)
                adpcm[i].signal = 2047;
            else if (adpcm[i].signal < -2048)
                adpcm[i].signal = -2048;
            /* adjust the step size and clamp */
            adpcm[i].step += index_shift[nibble & 7];
            if (adpcm[i].step > 48)
                adpcm[i].step = 48;
            else if (adpcm[i].step < 0)
                adpcm[i].step = 0;
            /* return the signal scaled up to 32767 */
            return (short)(adpcm[i].signal << 4);
        }
        private static void generate_adpcm(int i, short[] buffer, int samples)
        {
            /* if this voice is active */
            int i1 = 0;
            if (OKI.voice[i].playing)
            {
                int bbase = (int)(OKI.bank_offset + OKI.voice[i].base_offset);
                int sample = (int)OKI.voice[i].sample;
                int count = (int)OKI.voice[i].count;
                /* loop while we still have samples to generate */
                while (samples != 0)
                {
                    int nibble = okirom[bbase + sample / 2] >> (((sample & 1) << 2) ^ 4);
                    buffer[i1] = (short)(clock_adpcm(i, (byte)nibble) * OKI.voice[i].volume / 256);
                    i1++;
                    samples--;
                    if (++sample >= count)
                    {
                        OKI.voice[i].playing = false;
                        break;
                    }
                }
                OKI.voice[i].sample = (uint)sample;
            }
            while ((samples--) != 0)
            {
                buffer[i1] = 0;
                i1++;
            }
        }
        public static void okim6295_update(int offsetO, int samples)
        {
            int i;
            for (i = 0; i < samples; i++)
            {
                Sound.okistream.streamoutput[0][offsetO + i] = 0;
            }
            for (i = 0; i < 4; i++)
            {
                short[] sample_data = new short[10000];
                int remaining = samples;
                /* loop while we have samples remaining */
                while (remaining != 0)
                {
                    int samples1 = (remaining > 10000) ? 10000 : remaining;
                    int samp;
                    generate_adpcm(i, sample_data, samples1);
                    for (samp = 0; samp < samples; samp++)
                    {
                        Sound.okistream.streamoutput[0][offsetO + samp] += sample_data[samp];
                    }
                    remaining -= samples1;
                }
            }
        }
        public static void okim6295_start()
        {
            int voice;
            compute_tables();
            OKI.command = -1;
            OKI.bank_offset = 0;
            OKI.master_clock = 1000000;            
            OKI.voice = new ADPCMVoice[4];
            adpcm = new adpcm_state[4];
            for (voice = 0; voice < 4; voice++)
            {
                OKI.voice[voice].volume = 255;
                reset_adpcm(voice);
            }
        }
        public static void okim6295_reset()
        {
            int i;
            Sound.okistream.stream_update();
            for (i = 0; i < 4; i++)
            {
                OKI.voice[i].playing = false;
            }
        }
        public static void okim6295_set_pin7(int pin7)
        {

        }
        public static byte okim6295_status_0_r()
        {
            int i;
            byte result;
            result = 0xf0;
            Sound.okistream.stream_update();
            for (i = 0; i < 4; i++)
            {
                if (OKI.voice[i].playing)
                {
                    result |= (byte)(1 << i);
                }
            }
            return result;
        }
        public static void okim6295_data_0_w(byte data)
        {
            if (OKI.command != -1)
            {
                int temp = data >> 4, i, start, stop;
                int baseoffset;
                Sound.okistream.stream_update();
                for (i = 0; i < 4; i++, temp >>= 1)
                {
                    if ((temp & 1)!=0)
                    {
                        baseoffset = OKI.bank_offset + OKI.command * 8;
                        start = ((okirom[baseoffset + 0] << 16) + (okirom[baseoffset + 1] << 8) + okirom[baseoffset + 2]) & 0x3ffff;
                        stop = ((okirom[baseoffset + 3] << 16) + (okirom[baseoffset + 4] << 8) + okirom[baseoffset + 5]) & 0x3ffff;
                        if (start < stop)
                        {
                            if (!OKI.voice[i].playing)
                            {
                                OKI.voice[i].playing = true;
                                OKI.voice[i].base_offset = (uint)start;
                                OKI.voice[i].sample = 0;
                                OKI.voice[i].count = (uint)(2 * (stop - start + 1));
                                reset_adpcm(i);//OKI.voice[i].adpcm);
                                OKI.voice[i].volume = volume_table[data & 0x0f];
                            }
                            else
                            {

                            }
                        }
                        else
                        {
                            OKI.voice[i].playing = false;
                        }
                    }
                }
                OKI.command = -1;
            }
            else if ((data & 0x80)!=0)
            {
                OKI.command = data & 0x7f;
            }
            else
            {
                int temp = data >> 3, i;
                Sound.okistream.stream_update();
                for (i = 0; i < 4; i++, temp >>= 1)
                {
                    if ((temp & 1)!=0)
                    {
                        OKI.voice[i].playing = false;
                    }
                }
            }
        }
        public static void SaveStateBinary(BinaryWriter writer)
        {
            int i;
            writer.Write(OKI.command);
            writer.Write(OKI.bank_offset);
            for (i = 0; i < 4; i++)
            {
                writer.Write(OKI.voice[i].playing);
                writer.Write(OKI.voice[i].sample);
                writer.Write(OKI.voice[i].count);
                writer.Write(OKI.voice[i].volume);
                writer.Write(OKI.voice[i].base_offset);
                writer.Write(adpcm[i].signal);
                writer.Write(adpcm[i].step);
            }
        }
        public static void LoadStateBinary(BinaryReader reader)
        {
            int i;
            OKI.command = reader.ReadInt32();
            OKI.bank_offset = reader.ReadInt32();
            for (i = 0; i < 4; i++)
            {
                OKI.voice[i].playing = reader.ReadBoolean();
                OKI.voice[i].sample = reader.ReadUInt32();
                OKI.voice[i].count = reader.ReadUInt32();
                OKI.voice[i].volume = reader.ReadUInt32();
                OKI.voice[i].base_offset = reader.ReadUInt32();
                adpcm[i].signal = reader.ReadInt32();
                adpcm[i].step = reader.ReadInt32();
            }
        }
    }
}