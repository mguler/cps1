﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace mame
{
    public class sound_stream
    {
        public int sample_rate;
        public long attoseconds_per_sample;
        public int max_samples_per_update;
        public int output_sampindex;		/* current position within each output buffer */
        public int output_base_sampindex;	/* sample at base of buffer, relative to the current emulated second */
        public int[][] streaminput,streamoutput;
        private updatedelegate updatecallback;
        public delegate void updatedelegate(int offset, int length);
        private static long update_attoseconds = Attotime.ATTOSECONDS_PER_SECOND / 50;
        public sound_stream(int _sample_rate, int inputs, int outputs, updatedelegate callback)
        {
            int i;
            sample_rate = _sample_rate;
            attoseconds_per_sample = Attotime.ATTOSECONDS_PER_SECOND / sample_rate;
            max_samples_per_update = (int)((update_attoseconds + attoseconds_per_sample - 1) / attoseconds_per_sample);
            output_base_sampindex = -max_samples_per_update;
            streaminput = new int[inputs][];
            for (i = 0; i < inputs; i++)
            {
                streaminput[i] = new int[max_samples_per_update];
            }
            streamoutput = new int[outputs][];
            for (i = 0; i < outputs; i++)
            {
                streamoutput[i] = new int[5 * max_samples_per_update];
            }
            updatecallback = callback;
        }
        public void stream_update()
        {
            int update_sampindex = time_to_sampindex(Timer.get_current_time());
            int offset, samples;
            samples = update_sampindex - output_sampindex;
            if (samples > 0)
            {
                offset = output_sampindex - output_base_sampindex;
                updatecallback(offset, samples);
            }
            output_sampindex = update_sampindex;
        }
        public void adjuststream(bool second_tick)
        {
            int i, j;
            int output_bufindex = output_sampindex - output_base_sampindex;
            if (second_tick)
            {
                output_sampindex -= sample_rate;
                output_base_sampindex -= sample_rate;
            }
            if (output_bufindex > 3 * max_samples_per_update)
            {
                int samples_to_lose = output_bufindex - max_samples_per_update;
                for (i = 0; i < streamoutput.Length; i++)
                {
                    for (j = 0; j < max_samples_per_update; j++)
                    {
                        streamoutput[i][j] = streamoutput[i][samples_to_lose + j];
                    }
                }
                output_base_sampindex += samples_to_lose;
            }
        }
        private int time_to_sampindex(Atime time)
        {
            int sample = (int)(time.attoseconds / attoseconds_per_sample);
            if (time.seconds > Sound.last_update_second)
            {
                sample += sample_rate;
            }
            if (time.seconds < Sound.last_update_second)
            {
                sample -= sample_rate;
            }
            return sample;
        }
    };
    public partial class Sound
    {
        public static int last_update_second;        
        public static sound_stream ym2151stream, okistream, mixerstream;
        public static sound_stream qsoundstream;
        private static void generate_resampled_dataY5(int gain)
        {            
            int offset;
            int sample0,sample1;
            long basetime;
            int basesample;
            uint basefrac;
            uint step;
            int sampindex;
            basetime = mixerstream.output_sampindex * mixerstream.attoseconds_per_sample - mixerstream.attoseconds_per_sample;
            if (basetime >= 0)
                basesample = (int)(basetime / ym2151stream.attoseconds_per_sample);
            else
                basesample = (int)(-(-basetime / ym2151stream.attoseconds_per_sample) - 1);
            offset = basesample - ym2151stream.output_base_sampindex;
            basefrac = (uint)((basetime - basesample * ym2151stream.attoseconds_per_sample) / (Attotime.ATTOSECONDS_PER_SECOND >> 22));
            step = (uint)(((ulong)ym2151stream.sample_rate << 22) / 48000);
            if (step > 0x400000)
            {
                int smallstep = (int)(step >> (14));
                for (sampindex = 0; sampindex < 0x3c0; sampindex++)
                {
                    int remainder = smallstep;
                    int tpos = 0;
                    int scale;
                    scale = (int)((0x400000 - basefrac) >> (14));
                    sample0 = ym2151stream.streamoutput[0][offset + tpos] * scale;
                    sample1 = ym2151stream.streamoutput[1][offset + tpos] * scale;
                    tpos++;
                    remainder -= scale;
                    while (remainder > 0x100)
                    {
                        sample0 += ym2151stream.streamoutput[0][offset + tpos] * 0x100;
                        sample1 += ym2151stream.streamoutput[1][offset + tpos] * 0x100;
                        tpos++;
                        remainder -= 0x100;
                    }
                    sample0 += ym2151stream.streamoutput[0][offset + tpos] * remainder;
                    sample1 += ym2151stream.streamoutput[1][offset + tpos] * remainder;
                    sample0 /= smallstep;
                    sample1 /= smallstep;
                    mixerstream.streaminput[0][sampindex] = (sample0 * gain) >> 8;
                    mixerstream.streaminput[1][sampindex] = (sample1 * gain) >> 8;
                    basefrac += step;
                    offset += (int)(basefrac >> 22);
                    basefrac &= 0x3fffff;
                }
            }
        }
        private static void generate_resampled_dataO(int gain, int minput)
        {
            int offset;
            int sample;
            long basetime;
            int basesample;
            uint basefrac;
            uint step;
            int sampindex;
            basetime = mixerstream.output_sampindex * mixerstream.attoseconds_per_sample - okistream.attoseconds_per_sample * 2;
            if (basetime >= 0)
                basesample = (int)(basetime / okistream.attoseconds_per_sample);
            else
                basesample = (int)(-(-basetime / okistream.attoseconds_per_sample) - 1);
            offset = basesample - okistream.output_base_sampindex;
            basefrac = (uint)((basetime - basesample * okistream.attoseconds_per_sample) / (Attotime.ATTOSECONDS_PER_SECOND >> 22));
            step = (uint)(((ulong)okistream.sample_rate << 22) / 48000);
            if (step < 0x400000)
            {
                for (sampindex = 0; sampindex < 0x3c0; sampindex++)
                {
                    int interp_frac = (int)(basefrac >> (10));
                    sample = (okistream.streamoutput[0][offset] * (0x1000 - interp_frac) + okistream.streamoutput[0][offset + 1] * interp_frac) >> 12;
                    mixerstream.streaminput[minput][sampindex] = (sample * gain) >> 8;
                    basefrac += step;
                    offset += (int)(basefrac >> 22);
                    basefrac &= 0x3fffff;
                }
            }
        }
        private static void generate_resampled_dataQ()
        {
            int offset;            
            long basetime;
            int basesample;
            uint basefrac;
            uint step;
            int sampindex;
            basetime = mixerstream.output_sampindex * mixerstream.attoseconds_per_sample - qsoundstream.attoseconds_per_sample * 2;
            if (basetime >= 0)
                basesample = (int)(basetime / qsoundstream.attoseconds_per_sample);
            else
                basesample = (int)(-(-basetime / qsoundstream.attoseconds_per_sample) - 1);
            offset = basesample - qsoundstream.output_base_sampindex;
            basefrac = (uint)((basetime - basesample * qsoundstream.attoseconds_per_sample) / (Attotime.ATTOSECONDS_PER_SECOND >> 22));
            step = (uint)(((ulong)qsoundstream.sample_rate << 22) / 48000);
            for (sampindex = 0; sampindex < 0x3c0; sampindex++)
            {
                int interp_frac = (int)(basefrac >> 10);
                mixerstream.streaminput[0][sampindex] = (qsoundstream.streamoutput[0][offset] * (0x1000 - interp_frac) + qsoundstream.streamoutput[0][offset + 1] * interp_frac) >> 12;
                mixerstream.streaminput[1][sampindex] = (qsoundstream.streamoutput[1][offset] * (0x1000 - interp_frac) + qsoundstream.streamoutput[1][offset + 1] * interp_frac) >> 12;
                basefrac += step;
                offset += (int)(basefrac >> 22);
                basefrac &= 0x3fffff;
            }
        }        
        public static void streams_updateC()
        {
            Atime curtime =Timer.global_basetime;
            bool second_tick = false;
            if (curtime.seconds != last_update_second)
            {
                second_tick = true;
            }
            ym2151stream.adjuststream(second_tick);
            okistream.adjuststream(second_tick);
            mixerstream.adjuststream(second_tick);
            last_update_second = curtime.seconds;
        }
        private static void streams_updateQ()
        {
            Atime curtime = Timer.global_basetime;
            bool second_tick = false;
            if (curtime.seconds != last_update_second)
            {
                second_tick = true;
            }
            qsoundstream.adjuststream(second_tick);
            mixerstream.adjuststream(second_tick);
            last_update_second = curtime.seconds;
        }
    }
}