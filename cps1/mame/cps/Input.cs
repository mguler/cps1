﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DirectX.DirectInput;

namespace mame
{
    public partial class CPS
    {
        public static void loop_inputports_cps1_6b()
        {
            if (Keyboard.IsPressed(Key.D5))
            {
                sbyte0 &= ~0x01;
            }
            else
            {
                sbyte0 |= 0x01;
            }
            if (Keyboard.IsPressed(Key.D6))
            {
                sbyte0 &= ~0x02;
            }
            else
            {
                sbyte0 |= 0x02;
            }
            if (Keyboard.IsPressed(Key.D1))
            {
                sbyte0 &= ~0x10;
            }
            else
            {
                sbyte0 |= 0x10;
            }
            if (Keyboard.IsPressed(Key.D2))
            {
                sbyte0 &= ~0x20;
            }
            else
            {
                sbyte0 |= 0x20;
            }
            if (Keyboard.IsPressed(Key.D))
            {
                short1 &= ~0x01;
            }
            else
            {
                short1 |= 0x01;
            }
            if (Keyboard.IsPressed(Key.A))
            {
                short1 &= ~0x02;
            }
            else
            {
                short1 |= 0x02;
            }
            if (Keyboard.IsPressed(Key.S))
            {
                short1 &= ~0x04;
            }
            else
            {
                short1 |= 0x04;
            }
            if (Keyboard.IsPressed(Key.W))
            {
                short1 &= ~0x08;
            }
            else
            {
                short1 |= 0x08;
            }
            if (Keyboard.IsPressed(Key.J))
            {
                short1 &= ~0x10;
            }
            else
            {
                short1 |= 0x10;
            }
            if (Keyboard.IsPressed(Key.K))
            {
                short1 &= ~0x20;
            }
            else
            {
                short1 |= 0x20;
            }
            if (Keyboard.IsPressed(Key.L))
            {
                short1 &= ~0x40;
            }
            else
            {
                short1 |= 0x40;
            }
            if (Keyboard.IsPressed(Key.U))
            {
                short2 &= ~0x01;
            }
            else
            {
                short2 |= 0x01;
            }
            if (Keyboard.IsPressed(Key.I))
            {
                short2 &= ~0x02;
            }
            else
            {
                short2 |= 0x02;
            }
            if (Keyboard.IsPressed(Key.O))
            {
                short2 &= ~0x04;
            }
            else
            {
                short2 |= 0x04;
            }
            if (Keyboard.IsPressed(Key.Right))
            {
                short1 &= ~0x0100;
            }
            else
            {
                short1 |= 0x0100;
            }
            if (Keyboard.IsPressed(Key.Left))
            {
                short1 &= ~0x0200;
            }
            else
            {
                short1 |= 0x0200;
            }
            if (Keyboard.IsPressed(Key.Down))
            {
                short1 &= ~0x0400;
            }
            else
            {
                short1 |= 0x0400;
            }
            if (Keyboard.IsPressed(Key.Up))
            {
                short1 &= ~0x0800;
            }
            else
            {
                short1 |= 0x0800;
            }
            if (Keyboard.IsPressed(Key.NumPad1))
            {
                short1 &= ~0x1000;
            }
            else
            {
                short1 |= 0x1000;
            }
            if (Keyboard.IsPressed(Key.NumPad2))
            {
                short1 &= ~0x2000;
            }
            else
            {
                short1 |= 0x2000;
            }
            if (Keyboard.IsPressed(Key.NumPad3))
            {
                short1 &= ~0x4000;
            }
            else
            {
                short1 |= 0x4000;
            }
            if (Keyboard.IsPressed(Key.NumPad4))
            {
                short2 &= ~0x10;
            }
            else
            {
                short2 |= 0x10;
            }
            if (Keyboard.IsPressed(Key.NumPad5))
            {
                short2 &= ~0x20;
            }
            else
            {
                short2 |= 0x20;
            }
            if (Keyboard.IsPressed(Key.NumPad6))
            {
                short2 &= ~0x40;
            }
            else
            {
                short2 |= 0x40;
            }
            if (Keyboard.IsPressed(Key.R))
            {
                sbyte0 &= ~0x04;
            }
            else
            {
                sbyte0 |= 0x04;
            }
            if (Keyboard.IsPressed(Key.T))
            {
                sbyte0 &= ~0x40;
            }
            else
            {
                sbyte0 |= 0x40;
            }
        }
        public static void loop_inputports_cps1_forgottn()
        {
            if (Keyboard.IsPressed(Key.D5))
            {
                sbyte0 &= ~0x01;
            }
            else
            {
                sbyte0 |= 0x01;
            }
            if (Keyboard.IsPressed(Key.D6))
            {
                sbyte0 &= ~0x02;
            }
            else
            {
                sbyte0 |= 0x02;
            }
            if (Keyboard.IsPressed(Key.D1))
            {
                sbyte0 &= ~0x10;
            }
            else
            {
                sbyte0 |= 0x10;
            }
            if (Keyboard.IsPressed(Key.D2))
            {
                sbyte0 &= ~0x20;
            }
            else
            {
                sbyte0 |= 0x20;
            }
            if (Keyboard.IsPressed(Key.D))
            {
                short1 &= ~0x01;
            }
            else
            {
                short1 |= 0x01;
            }
            if (Keyboard.IsPressed(Key.A))
            {
                short1 &= ~0x02;
            }
            else
            {
                short1 |= 0x02;
            }
            if (Keyboard.IsPressed(Key.S))
            {
                short1 &= ~0x04;
            }
            else
            {
                short1 |= 0x04;
            }
            if (Keyboard.IsPressed(Key.W))
            {
                short1 &= ~0x08;
            }
            else
            {
                short1 |= 0x08;
            }
            if (Keyboard.IsPressed(Key.J))
            {
                short1 &= ~0x10;
            }
            else
            {
                short1 |= 0x10;
            }            
            if (Keyboard.IsPressed(Key.Right))
            {
                short1 &= ~0x0100;
            }
            else
            {
                short1 |= 0x0100;
            }
            if (Keyboard.IsPressed(Key.Left))
            {
                short1 &= ~0x0200;
            }
            else
            {
                short1 |= 0x0200;
            }
            if (Keyboard.IsPressed(Key.Down))
            {
                short1 &= ~0x0400;
            }
            else
            {
                short1 |= 0x0400;
            }
            if (Keyboard.IsPressed(Key.Up))
            {
                short1 &= ~0x0800;
            }
            else
            {
                short1 |= 0x0800;
            }
            if (Keyboard.IsPressed(Key.NumPad1))
            {
                short1 &= ~0x1000;
            }
            else
            {
                short1 |= 0x1000;
            }
            if (Keyboard.IsPressed(Key.R))
            {
                sbyte0 &= ~0x04;
            }
            else
            {
                sbyte0 |= 0x04;
            }
            if (Keyboard.IsPressed(Key.T))
            {
                sbyte0 &= ~0x40;
            }
            else
            {
                sbyte0 |= 0x40;
            }
            Inptport.frame_update_analog_field_forgottn_p0(Inptport.analog_p0);
            Inptport.frame_update_analog_field_forgottn_p1(Inptport.analog_p1);
        }
        public static void loop_inputports_cps1_sf2hack()
        {
            if (Keyboard.IsPressed(Key.D5))
            {
                sbyte0 &= ~0x01;
            }
            else
            {
                sbyte0 |= 0x01;
            }
            if (Keyboard.IsPressed(Key.D6))
            {
                sbyte0 &= ~0x02;
            }
            else
            {
                sbyte0 |= 0x02;
            }
            if (Keyboard.IsPressed(Key.D1))
            {
                sbyte0 &= ~0x10;
            }
            else
            {
                sbyte0 |= 0x10;
            }
            if (Keyboard.IsPressed(Key.D2))
            {
                sbyte0 &= ~0x20;
            }
            else
            {
                sbyte0 |= 0x20;
            }
            if (Keyboard.IsPressed(Key.D))
            {
                short1 &= ~0x01;
            }
            else
            {
                short1 |= 0x01;
            }
            if (Keyboard.IsPressed(Key.A))
            {
                short1 &= ~0x02;
            }
            else
            {
                short1 |= 0x02;
            }
            if (Keyboard.IsPressed(Key.S))
            {
                short1 &= ~0x04;
            }
            else
            {
                short1 |= 0x04;
            }
            if (Keyboard.IsPressed(Key.W))
            {
                short1 &= ~0x08;
            }
            else
            {
                short1 |= 0x08;
            }
            if (Keyboard.IsPressed(Key.J))
            {
                short1 &= ~0x10;
            }
            else
            {
                short1 |= 0x10;
            }
            if (Keyboard.IsPressed(Key.K))
            {
                short1 &= ~0x20;
            }
            else
            {
                short1 |= 0x20;
            }
            if (Keyboard.IsPressed(Key.L))
            {
                short1 &= ~0x40;
            }
            else
            {
                short1 |= 0x40;
            }
            if (Keyboard.IsPressed(Key.U))
            {
                short2 &= ~0x0100;
            }
            else
            {
                short2 |= 0x0100;
            }
            if (Keyboard.IsPressed(Key.I))
            {
                short2 &= ~0x0200;
            }
            else
            {
                short2 |= 0x0200;
            }
            if (Keyboard.IsPressed(Key.O))
            {
                short2 &= ~0x0400;
            }
            else
            {
                short2 |= 0x0400;
            }
            if (Keyboard.IsPressed(Key.Right))
            {
                short1 &= ~0x0100;
            }
            else
            {
                short1 |= 0x0100;
            }
            if (Keyboard.IsPressed(Key.Left))
            {
                short1 &= ~0x0200;
            }
            else
            {
                short1 |= 0x0200;
            }
            if (Keyboard.IsPressed(Key.Down))
            {
                short1 &= ~0x0400;
            }
            else
            {
                short1 |= 0x0400;
            }
            if (Keyboard.IsPressed(Key.Up))
            {
                short1 &= ~0x0800;
            }
            else
            {
                short1 |= 0x0800;
            }
            if (Keyboard.IsPressed(Key.NumPad1))
            {
                short1 &= ~0x1000;
            }
            else
            {
                short1 |= 0x1000;
            }
            if (Keyboard.IsPressed(Key.NumPad2))
            {
                short1 &= ~0x2000;
            }
            else
            {
                short1 |= 0x2000;
            }
            if (Keyboard.IsPressed(Key.NumPad3))
            {
                short1 &= ~0x4000;
            }
            else
            {
                short1 |= 0x4000;
            }
            if (Keyboard.IsPressed(Key.NumPad4))
            {
                short2 &= ~0x1000;
            }
            else
            {
                short2 |= 0x1000;
            }
            if (Keyboard.IsPressed(Key.NumPad5))
            {
                short2 &= ~0x2000;
            }
            else
            {
                short2 |= 0x2000;
            }
            if (Keyboard.IsPressed(Key.NumPad6))
            {
                short2 &= ~0x4000;
            }
            else
            {
                short2 |= 0x4000;
            }
            if (Keyboard.IsPressed(Key.R))
            {
                sbyte0 &= ~0x04;
            }
            else
            {
                sbyte0 |= 0x04;
            }
            if (Keyboard.IsPressed(Key.T))
            {
                sbyte0 &= ~0x40;
            }
            else
            {
                sbyte0 |= 0x40;
            }
        }
        public static void loop_inputports_cps1_cworld2j()
        {
            if (Keyboard.IsPressed(Key.D5))
            {
                sbyte0 &= ~0x01;
            }
            else
            {
                sbyte0 |= 0x01;
            }
            if (Keyboard.IsPressed(Key.D6))
            {
                sbyte0 &= ~0x02;
            }
            else
            {
                sbyte0 |= 0x02;
            }
            if (Keyboard.IsPressed(Key.D1))
            {
                sbyte0 &= ~0x10;
            }
            else
            {
                sbyte0 |= 0x10;
            }
            if (Keyboard.IsPressed(Key.D2))
            {
                sbyte0 &= ~0x20;
            }
            else
            {
                sbyte0 |= 0x20;
            }            
            if (Keyboard.IsPressed(Key.J))
            {
                short1 &= ~0x10;
            }
            else
            {
                short1 |= 0x10;
            }
            if (Keyboard.IsPressed(Key.K))
            {
                short1 &= ~0x20;
            }
            else
            {
                short1 |= 0x20;
            }
            if (Keyboard.IsPressed(Key.L))
            {
                short1 &= ~0x40;
            }
            else
            {
                short1 |= 0x40;
            }
            if (Keyboard.IsPressed(Key.U))
            {
                short1 &= ~0x80;
            }
            else
            {
                short1 |= 0x80;
            }            
            if (Keyboard.IsPressed(Key.NumPad1))
            {
                short1 &= ~0x1000;
            }
            else
            {
                short1 |= 0x1000;
            }
            if (Keyboard.IsPressed(Key.NumPad2))
            {
                short1 &= ~0x2000;
            }
            else
            {
                short1 |= 0x2000;
            }
            if (Keyboard.IsPressed(Key.NumPad3))
            {
                short1 &= ~0x4000;
            }
            else
            {
                short1 |= 0x4000;
            }
            if (Keyboard.IsPressed(Key.NumPad4))
            {
                short1 &= unchecked((short)~0x8000);
            }
            else
            {
                short1 |= unchecked((short)0x8000);
            }
            if (Keyboard.IsPressed(Key.R))
            {
                sbyte0 &= ~0x04;
            }
            else
            {
                sbyte0 |= 0x04;
            }
            if (Keyboard.IsPressed(Key.T))
            {
                sbyte0 &= ~0x40;
            }
            else
            {
                sbyte0 |= 0x40;
            }
        }
        public static void loop_inputports_cps1_sfzch()
        {
        if (Keyboard.IsPressed(Key.D5))
            {
                sbyte0 &= ~0x01;
            }
            else
            {
                sbyte0 |= 0x01;
            }
            if (Keyboard.IsPressed(Key.D6))
            {
                sbyte0 &= ~0x02;
            }
            else
            {
                sbyte0 |= 0x02;
            }
            if (Keyboard.IsPressed(Key.D1))
            {
                sbyte0 &= ~0x10;
            }
            else
            {
                sbyte0 |= 0x10;
            }
            if (Keyboard.IsPressed(Key.D2))
            {
                sbyte0 &= ~0x20;
            }
            else
            {
                sbyte0 |= 0x20;
            }
            if (Keyboard.IsPressed(Key.D))
            {
                short1 &= ~0x01;
            }
            else
            {
                short1 |= 0x01;
            }
            if (Keyboard.IsPressed(Key.A))
            {
                short1 &= ~0x02;
            }
            else
            {
                short1 |= 0x02;
            }
            if (Keyboard.IsPressed(Key.S))
            {
                short1 &= ~0x04;
            }
            else
            {
                short1 |= 0x04;
            }
            if (Keyboard.IsPressed(Key.W))
            {
                short1 &= ~0x08;
            }
            else
            {
                short1 |= 0x08;
            }
            if (Keyboard.IsPressed(Key.J))
            {
                short1 &= ~0x10;
            }
            else
            {
                short1 |= 0x10;
            }
            if (Keyboard.IsPressed(Key.K))
            {
                short1 &= ~0x20;
            }
            else
            {
                short1 |= 0x20;
            }
            if (Keyboard.IsPressed(Key.L))
            {
                short1 &= ~0x40;
            }
            else
            {
                short1 |= 0x40;
            }
            if (Keyboard.IsPressed(Key.U))
            {
                short1 &= ~0x80;
            }
            else
            {
                short1 |= 0x80;
            }
            /*if (Keyboard.IsPressed(Key.I))
            {
                short2 &= ~0x02;
            }
            else
            {
                short2 |= 0x02;
            }*/
            if (Keyboard.IsPressed(Key.O))
            {
                sbyte0 &= ~0x40;
            }
            else
            {
                sbyte0 |= 0x40;
            }
            if (Keyboard.IsPressed(Key.Right))
            {
                short1 &= ~0x0100;
            }
            else
            {
                short1 |= 0x0100;
            }
            if (Keyboard.IsPressed(Key.Left))
            {
                short1 &= ~0x0200;
            }
            else
            {
                short1 |= 0x0200;
            }
            if (Keyboard.IsPressed(Key.Down))
            {
                short1 &= ~0x0400;
            }
            else
            {
                short1 |= 0x0400;
            }
            if (Keyboard.IsPressed(Key.Up))
            {
                short1 &= ~0x0800;
            }
            else
            {
                short1 |= 0x0800;
            }
            if (Keyboard.IsPressed(Key.NumPad1))
            {
                short1 &= ~0x1000;
            }
            else
            {
                short1 |= 0x1000;
            }
            if (Keyboard.IsPressed(Key.NumPad2))
            {
                short1 &= ~0x2000;
            }
            else
            {
                short1 |= 0x2000;
            }
            if (Keyboard.IsPressed(Key.NumPad3))
            {
                short1 &= ~0x4000;
            }
            else
            {
                short1 |= 0x4000;
            }
            if (Keyboard.IsPressed(Key.NumPad4))
            {
                short1 &= unchecked((short)~0x8000);
            }
            else
            {
                short1 |= unchecked((short)0x8000);
            }
            /*if (Keyboard.IsPressed(Key.NumPad5))
            {
                short2 &= ~0x20;
            }
            else
            {
                short2 |= 0x20;
            }*/
            if (Keyboard.IsPressed(Key.NumPad6))
            {
                sbyte0 &= unchecked((sbyte)~0x80);
            }
            else
            {
                sbyte0 |= unchecked((sbyte)0x80);
            }
            if (Keyboard.IsPressed(Key.R))
            {
                sbyte0 &= ~0x04;
            }
            else
            {
                sbyte0 |= 0x04;
            }
            if (Keyboard.IsPressed(Key.T))
            {
                sbyte0 &= ~0x40;
            }
            else
            {
                sbyte0 |= 0x40;
            }
        }
        public static void record_portC()
        {
            if (sbyte0 != sbyte0_old || short1 != short1_old || short2 != short2_old || sbyte3 != sbyte3_old)
            {
                sbyte0_old = sbyte0;
                short1_old = short1;
                short2_old = short2;
                sbyte3_old = sbyte3;               
                Mame.bwRecord.Write(Video.screenstate.frame_number);
                Mame.bwRecord.Write(sbyte0);
                Mame.bwRecord.Write(short1);
                Mame.bwRecord.Write(short2);
                Mame.bwRecord.Write(sbyte3);
            }
        }
        public static void replay_portC()
        {
            if (Inptport.bReplayRead)
            {
                try
                {
                    Video.frame_number_obj = Mame.brRecord.ReadInt64();
                    sbyte0_old = Mame.brRecord.ReadSByte();
                    short1_old = Mame.brRecord.ReadInt16();
                    short2_old = Mame.brRecord.ReadInt16();
                    sbyte3_old = Mame.brRecord.ReadSByte();
                }
                catch
                {
                    Mame.playState = Mame.PlayState.PLAY_REPLAYEND;
                }
                Inptport.bReplayRead = false;
            }
            if (Video.screenstate.frame_number == Video.frame_number_obj)
            {
                sbyte0 = sbyte0_old;
                short1 = short1_old;
                short2 = short2_old;
                sbyte3 = sbyte3_old;
                Inptport.bReplayRead = true;
            }
            else
            {
                Inptport.bReplayRead = false;
            }
        }
    }
}