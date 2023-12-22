using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DirectX.DirectSound;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace mame
{
    public partial class Sound
    {
        public static Timer.emu_timer sound_update_timer;
        private static int[] leftmix, rightmix;
        private static byte[] finalmixb;
        private static int sound_muted;
        public static ushort[] latched_value, utempdata;
        public static Action sound_update;
        public static SecondaryBuffer buf2;
        private static int stream_buffer_in;
        public static void sound_init()
        {
            leftmix = new int[0x3c0];
            rightmix = new int[0x3c0];
            finalmixb = new byte[0xf00];           
            sound_muted = 0;
            buf2.Play(0, BufferPlayFlags.Looping);
            last_update_second = 0;
            Atime update_frequency = new Atime(0, Attotime.ATTOSECONDS_PER_SECOND / 50);
            switch (Machine.sBoard)
            {
                case "CPS-1":
                    latched_value = new ushort[2];
                    utempdata = new ushort[2];
                    sound_update = sound_updateC;
                    sound_update_timer = Timer.timer_alloc_common(sound_update, "sound_update", false);
                    YM2151.ym2151_init(3579545);
                    OKI6295.okim6295_start();
                    ym2151stream = new sound_stream(55930, 0, 2, YM2151.ym2151_update_one);
                    okistream = new sound_stream(1000000 / 132, 0, 1, OKI6295.okim6295_update);
                    mixerstream = new sound_stream(48000, 3, 0, null);
                    break;
                case "CPS-1(QSound)":
                    sound_update = sound_updateQ;
                    sound_update_timer = Timer.timer_alloc_common(sound_update, "sound_update", false);
                    QSound.qsound_start();
                    qsoundstream = new sound_stream(4000000 / 166, 0, 2, QSound.qsound_update);
                    mixerstream = new sound_stream(48000, 2, 0, null);
                    break;                
            }
            Timer.timer_adjust_periodic(sound_update_timer, update_frequency, update_frequency);
        }
        public static void sound_reset()
        {
            switch (Machine.sBoard)
            {
                case "CPS-1":                                       
                    YM2151.ym2151_reset_chip();
                    OKI6295.okim6295_reset();
                    break;
                case "CPS-1(QSound)":
                    break;
            }
        }
        public static void sound_pause(bool pause)
        {
            if (pause)
            {
                sound_muted |= 0x02;
                Sound.buf2.Volume = -10000;
            }
            else
            {
                sound_muted &= ~0x02;
                Sound.buf2.Volume = 0;
            }
        }
        public static void sound_updateC()
        {
            int sampindex;
            byte[] ym2151b = new byte[0xf00];
            ym2151stream.stream_update();
            okistream.stream_update();
            generate_resampled_dataY5(0x59);
            generate_resampled_dataO(0x4c, 2);
            mixerstream.output_sampindex += 0x3c0;
            for (sampindex = 0; sampindex < 0x3c0; sampindex++)
            {
                int samp;
                samp = mixerstream.streaminput[0][sampindex] + mixerstream.streaminput[1][sampindex] + mixerstream.streaminput[2][sampindex];
                if (samp < -32768)
                    samp = -32768;
                else if (samp > 32767)
                    samp = 32767;
                finalmixb[sampindex * 4] = (byte)samp;
                finalmixb[sampindex * 4 + 1] = (byte)((samp & 0xff00) >> 8);
                finalmixb[sampindex * 4 + 2] = (byte)samp;
                finalmixb[sampindex * 4 + 3] = (byte)((samp & 0xff00) >> 8);
            }
            osd_update_audio_stream(finalmixb, 0x3c0);
            streams_updateC();
        }
        public static void sound_updateQ()
        {
            int sampindex;
            qsoundstream.stream_update();
            generate_resampled_dataQ();
            mixerstream.output_sampindex += 0x3c0;
            for (sampindex = 0; sampindex < 0x3c0; sampindex++)
            {
                int sampL, sampR;
                sampL = mixerstream.streaminput[0][sampindex];
                if (sampL < -32768)
                    sampL = -32768;
                else if (sampL > 32767)
                    sampL = 32767;
                sampR = mixerstream.streaminput[1][sampindex];
                if (sampR < -32768)
                    sampR = -32768;
                else if (sampR > 32767)
                    sampR = 32767;

                finalmixb[sampindex * 4] = (byte)sampL;
                finalmixb[sampindex * 4 + 1] = (byte)((sampL & 0xff00) >> 8);
                finalmixb[sampindex * 4 + 2] = (byte)sampR;
                finalmixb[sampindex * 4 + 3] = (byte)((sampR & 0xff00) >> 8);
            }
            osd_update_audio_stream(finalmixb, 0x3c0);
            streams_updateQ();
        }
        public static void latch_callback()
        {
            latched_value[0] = utempdata[0];
        }
        public static void latch_callback2()
        {
            latched_value[1] = utempdata[1];
        }
        public static void soundlatch_w(ushort data)
        {
            utempdata[0] = data;
            Timer.timer_set_internal(latch_callback, "latch_callback");
        }
        public static void soundlatch2_w(ushort data)
        {
            utempdata[1] = data;
            Timer.timer_set_internal(latch_callback2, "latch_callback2");
        }
        public static ushort soundlatch_r()
        {
            return latched_value[0];
        }
        public static ushort soundlatch2_r()
        {
            return latched_value[1];
        }
        private static void osd_update_audio_stream(byte[] buffer, int samples_this_frame)
        {
            int play_position, write_position;
            int stream_in;
            byte[] buffer1, buffer2;
            int length1, length2;
            buf2.GetCurrentPosition(out play_position, out write_position);
            if (write_position < play_position)
            {
                write_position += 0x9400;
            }
            stream_in = stream_buffer_in;
            if (stream_in < write_position)
            {
                stream_in += 0x9400;
            }
            while (stream_in < write_position)
            {
                stream_in += 0xf00;
            }
            if (stream_in + 0xf00 > play_position + 0x9400)
            {
                return;
            }
            stream_buffer_in = stream_in % 0x9400;
            if (stream_buffer_in + 0xf00 < 0x9400)
            {
                length1 = 0xf00;
                length2 = 0;
                buffer1 = new byte[length1];
                Array.Copy(buffer, buffer1, length1);
                buf2.Write(stream_buffer_in, buffer1, LockFlag.None);
                stream_buffer_in = stream_buffer_in + 0xf00;
            }
            else if (stream_buffer_in + 0xf00 == 0x9400)
            {
                length1 = 0xf00;
                length2 = 0;
                buffer1 = new byte[length1];
                Array.Copy(buffer, buffer1, length1);
                buf2.Write(stream_buffer_in, buffer1, LockFlag.None);
                stream_buffer_in = 0;
            }
            else if (stream_buffer_in + 0xf00 > 0x9400)
            {
                length1 = 0x9400 - stream_buffer_in;
                length2 = 0xf00 - length1;
                buffer1 = new byte[length1];
                buffer2 = new byte[length2];
                Array.Copy(buffer, buffer1, length1);
                Array.Copy(buffer, length1, buffer2, 0, length2);
                buf2.Write(stream_buffer_in, buffer1, LockFlag.None);
                buf2.Write(0, buffer2, LockFlag.None);
                stream_buffer_in = length2;
            }      
        }
    }
}