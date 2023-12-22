using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DirectX.DirectInput;

namespace mame
{
    public class analog_field_state
    {
        public int adjdefvalue;
        public int sensitivity;
        public byte reverse;
        public int delta;
        public int accum;
        public int previous;
        public int minimum;
        public int maximum;
        public int reverse_val;
        public long scalepos;
        public long scaleneg;
        public byte lastdigital;
    }
    public class input_port_private
    {
        public Atime last_frame_time;
        public long last_delta_nsec;
    }
    public partial class Inptport
    {        
        public static bool bReplayRead;
        public delegate void loop_delegate();
        public static loop_delegate loop_inputports_callback, record_port_callback, replay_port_callback;
        public static analog_field_state analog_p0, analog_p1;
        public static input_port_private portdata;
        public static void input_port_init()
        {
            portdata = new input_port_private();
            switch (Machine.sBoard)
            {
                case "CPS-1":
                case "CPS-1(QSound)":
                    loop_inputports_callback = CPS.loop_inputports_cps1_6b;
                    record_port_callback = CPS.record_portC;
                    replay_port_callback = CPS.replay_portC;
                    analog_p0 = new analog_field_state();
                    analog_p1 = new analog_field_state();
                    analog_p0.adjdefvalue = 0;
                    analog_p1.adjdefvalue = 0;
                    analog_p0.sensitivity = 100;
                    analog_p1.sensitivity = 100;
                    analog_p0.reverse = 0;
                    analog_p1.reverse = 0;
                    analog_p0.delta = 20;
                    analog_p1.delta = 20;
                    analog_p0.minimum = 0;
                    analog_p1.minimum = 0;
                    analog_p0.maximum = 0x1ffe00;
                    analog_p1.maximum = 0x1ffe00;
                    analog_p0.reverse_val = 0x200000;
                    analog_p1.reverse_val = 0x200000;
                    analog_p0.scalepos = 0x8000;
                    analog_p1.scalepos = 0x8000;
                    analog_p0.scaleneg = 0x8000;
                    analog_p1.scaleneg = 0x8000;
                    break;
            }
            switch (Machine.sName)
            {
                case "forgottn":
                case "forgottna":
                case "forgottnu":
                case "forgottnue":
                case "forgottnuc":
                case "forgottnua":
                case "forgottnuaa":
                case "lostwrld":
                case "lostwrldo":
                    loop_inputports_callback = CPS.loop_inputports_cps1_forgottn;
                    break;
                case "sf2ebbl":
                case "sf2ebbl2":
                case "sf2ebbl3":
                case "sf2amf2":
                case "sf2m2":
                case "sf2m4":
                case "sf2m5":
                case "sf2m6":
                case "sf2m7":
                case "sf2yyc":
                case "sf2koryu":
                    loop_inputports_callback = CPS.loop_inputports_cps1_sf2hack;
                    break;
                case "cworld2j":
                case "cworld2ja":
                case "cworld2jb":
                case "qad":
                case "qadjr":
                case "qtono2j":
                    loop_inputports_callback = CPS.loop_inputports_cps1_cworld2j;
                    break;
                case "sfzch":
                case "wofx":
                    loop_inputports_callback = CPS.loop_inputports_cps1_sfzch;
                    break;
            }
        }
        public static int apply_analog_min_max(analog_field_state analog, int value)
        {
            int adjmin = (analog.minimum * 100) / analog.sensitivity;
            int adjmax = (analog.maximum * 100) / analog.sensitivity;
            int adj1 = (512 * 100) / analog.sensitivity;
            int adjdif = adjmax - adjmin + adj1;
            if (analog.reverse != 0)
            {
                while (value <= adjmin - adj1)
                    value += adjdif;
                while (value > adjmax)
                    value -= adjdif;
            }
            else
            {
                while (value >= adjmax + adj1)
                    value -= adjdif;
                while (value < adjmin)
                    value += adjdif;
            }
            return value;
        }
        public static uint input_port_read_direct(analog_field_state analog)
        {
            uint result;
            int value;
            long nsec_since_last;
            nsec_since_last = Attotime.attotime_to_attoseconds(Attotime.attotime_sub(Timer.get_current_time(), portdata.last_frame_time)) /Attotime.ATTOSECONDS_PER_NANOSECOND;
            value = (int)(analog.previous + ((long)(analog.accum - analog.previous) * nsec_since_last / portdata.last_delta_nsec));
            result = (uint)apply_analog_settings(value);
            return result;
        }
        public static int apply_analog_settings(int value)
        {
            value = apply_analog_min_max(analog_p0, value);
            value = (int)((long)value * analog_p0.sensitivity / 100);
            if (analog_p0.reverse != 0)
                value = analog_p0.reverse_val - value;
            if (value >= 0)
                value = (int)(value * analog_p0.scalepos / 0x1000000);
            else
                value = (int)(value * analog_p0.scaleneg / 0x1000000);
            value += analog_p0.adjdefvalue;
            return value;
        }
        public static void frame_update_callback()
        {
            if (Mame.mame_is_paused())
            {
                return;
            }
            frame_update();
            Video.screenstate.frame_number++;
        }
        private static void frame_update()
        {
            Atime curtime = Timer.get_current_time();
            portdata.last_delta_nsec = Attotime.attotime_to_attoseconds(Attotime.attotime_sub(curtime, portdata.last_frame_time)) / Attotime.ATTOSECONDS_PER_NANOSECOND;
            portdata.last_frame_time = curtime;
            if (Mame.playState != Mame.PlayState.PLAY_REPLAYRUNNING)
            {
                if (Mame.is_foreground)
                {
                    loop_inputports_callback();
                }
                /*int i1 = (int)(Video.screenstate.frame_number % 4);
                if (i1 == 0)
                {
                    CPS.short1 = unchecked((short)0xfffb);
                }
                else if (i1 == 1)
                {
                    CPS.short1 = unchecked((short)0xffce);
                }
                else if (i1 == 2)
                {
                    CPS.short1 = unchecked((short)0xfff7);
                }
                else if (i1 == 3)
                {
                    CPS.short1 = unchecked((short)0xffcd);
                }*/
            }
            if (Mame.playState == Mame.PlayState.PLAY_RECORDRUNNING)
            {
                record_port_callback();
            }
            else if (Mame.playState == Mame.PlayState.PLAY_REPLAYRUNNING)
            {
                replay_port_callback();
            }
        }
        public static void frame_update_analog_field_forgottn_p0(analog_field_state analog)
        {
            bool keypressed = false;
            int delta = 0;
            int value2;
            value2 = apply_analog_min_max(analog, analog.accum);
            analog.previous = analog.accum = value2;
            if (Keyboard.IsPressed(Key.K))
            {
                keypressed = true;
                delta -= analog_p0.delta * 0x200;
                analog.lastdigital = 1;
            }
            if (Keyboard.IsPressed(Key.L))
            {
                keypressed = true;
                delta += analog_p0.delta * 0x200;
                analog.lastdigital = 2;
            }
            analog.accum += delta;
            if (!keypressed)
                analog.lastdigital = 0;
        }
        public static void frame_update_analog_field_forgottn_p1(analog_field_state analog)
        {
            bool keypressed = false;
            int delta = 0;
            int value2;
            value2 = apply_analog_min_max(analog, analog.accum);
            analog.previous = analog.accum = value2;
            if (Keyboard.IsPressed(Key.NumPad2))
            {
                keypressed = true;
                delta -= analog_p0.delta * 0x200;
                analog.lastdigital = 1;
            }
            if (Keyboard.IsPressed(Key.NumPad3))
            {
                keypressed = true;
                delta += analog_p0.delta * 0x200;
                analog.lastdigital = 2;
            }
            analog.accum += delta;
            if (!keypressed)
                analog.lastdigital = 0;
        }
        public static void frame_update_analog_field_ecofghtr_p0(analog_field_state analog)
        {
            bool keypressed = false;
            int delta = 0;
            int value2;
            value2 = apply_analog_min_max(analog, analog.accum);
            analog.previous = analog.accum = value2;
            if (Keyboard.IsPressed(Key.U))
            {
                keypressed = true;
                delta -= analog_p0.delta * 0x200;
                analog.lastdigital = 1;
            }
            if (Keyboard.IsPressed(Key.I))
            {
                keypressed = true;
                delta += analog_p0.delta * 0x200;
                analog.lastdigital = 2;
            }
            analog.accum += delta;
            if (!keypressed)
                analog.lastdigital = 0;
        }
        public static void frame_update_analog_field_ecofghtr_p1(analog_field_state analog)
        {
            bool keypressed = false;
            int delta = 0;
            int value2;
            value2 = apply_analog_min_max(analog, analog.accum);
            analog.previous = analog.accum = value2;
            if (Keyboard.IsPressed(Key.NumPad4))
            {
                keypressed = true;
                delta -= analog_p0.delta * 0x200;
                analog.lastdigital = 1;
            }
            if (Keyboard.IsPressed(Key.NumPad5))
            {
                keypressed = true;
                delta += analog_p0.delta * 0x200;
                analog.lastdigital = 2;
            }
            analog.accum += delta;
            if (!keypressed)
                analog.lastdigital = 0;
        }
    }
}