﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace mame
{
    public class Timer
    {
        public static List<emu_timer> lt;
        private static List<emu_timer2> lt2;
        public static Atime global_basetime;
        public static Atime global_basetime_obj;
        private static bool callback_timer_modified;
        private static emu_timer callback_timer;
        private static Atime callback_timer_expire_time;
        public delegate void timer_fired_func();
        public class emu_timer
        {
            public Action action;
            public string func;
            public bool enabled;
            public bool temporary;
            public Atime period;
            public Atime start;
            public Atime expire;
            public emu_timer()
            {

            }
        }
        public class emu_timer2
        {
            public int index;
            public Action action;
            public string func;
            public emu_timer2(int i1, Action ac1, string func1)
            {
                index = i1;
                action = ac1;
                func = func1;
            }
        }
        public static Action getactionbyindex(int index)
        {
            Action action = null;
            foreach (emu_timer2 timer in lt2)
            {
                if (timer.index == index)
                {
                    action = timer.action;
                    if (index == 4)
                    {
                        action = Sound.sound_update;
                    }
                    break;
                }
            }
            return action;
        }
        public static string getfuncbyindex(int index)
        {
            string func = "";
            foreach (emu_timer2 timer in lt2)
            {
                if (timer.index == index)
                {
                    func = timer.func;
                    break;
                }
            }
            return func;
        }
        public static int getindexbyfunc(string func)
        {
            int index = 0;
            foreach (emu_timer2 timer in lt2)
            {
                if (timer.func == func)
                {
                    index = timer.index;
                    break;
                }
            }
            return index;
        }
        public static void timer_init()
        {
            global_basetime = Attotime.ATTOTIME_ZERO;
            lt = new List<emu_timer>();
            lt2 = new List<emu_timer2>();
            lt2.Add(new emu_timer2(1, Video.vblank_begin_callback, "vblank_begin_callback"));
            lt2.Add(new emu_timer2(2, Mame.soft_reset, "soft_reset"));
            lt2.Add(new emu_timer2(3, Cpuint.cpunum_empty_event_queue, "cpunum_empty_event_queue"));
            lt2.Add(new emu_timer2(4, Sound.sound_update, "sound_update"));
            lt2.Add(new emu_timer2(5, Watchdog.watchdog_callback, "watchdog_callback"));
            lt2.Add(new emu_timer2(6, Generic.irq_1_0_line_hold, "irq_1_0_line_hold"));

            lt2.Add(new emu_timer2(10, YM2151.irqAon_callback, "irqAon_callback"));
            lt2.Add(new emu_timer2(11, YM2151.irqBon_callback, "irqBon_callback"));
            lt2.Add(new emu_timer2(12, YM2151.irqAoff_callback, "irqAoff_callback"));
            lt2.Add(new emu_timer2(13, YM2151.irqBoff_callback, "irqBoff_callback"));
            lt2.Add(new emu_timer2(14, YM2151.timer_callback_a, "timer_callback_a"));
            lt2.Add(new emu_timer2(15, YM2151.timer_callback_b, "timer_callback_b"));

            lt2.Add(new emu_timer2(19, Video.scanline0_callback, "scanline0_callback"));
            lt2.Add(new emu_timer2(20, Sound.latch_callback, "latch_callback"));
            lt2.Add(new emu_timer2(21, Sound.latch_callback2, "latch_callback2"));
        }
        public static Atime get_current_time()
        {
            if (callback_timer != null)
            {
                return callback_timer_expire_time;
            }
            if (Cpuexec.activecpu >= 0 && Cpuexec.activecpu < Cpuexec.ncpu)
            {
                return Cpuexec.cpunum_get_localtime(Cpuexec.activecpu);
            }
            return global_basetime;
        }
        public static void timer_remove(emu_timer timer1)
        {
            if (timer1 == callback_timer)
                callback_timer_modified = true;
            timer_list_remove(timer1);
        }
        public static void timer_adjust_periodic(emu_timer which, Atime start_delay, Atime period)
        {
            Atime time = get_current_time();
            if (which == callback_timer)
                callback_timer_modified = true;
            which.enabled = true;
            if (start_delay.seconds < 0)
                start_delay = Attotime.ATTOTIME_ZERO;
            which.start = time;
            which.expire = Attotime.attotime_add(time, start_delay);
            which.period = period;
            timer_list_remove(which);
            timer_list_insert(which);
            if (Timer.lt.IndexOf(which) == 0)
            {
                if (Cpuexec.activecpu >= 0 && Cpuexec.activecpu < Cpuexec.ncpu)
                {
                    Cpuexec.activecpu_abort_timeslice(Cpuexec.activecpu);
                }
            }
        }
        public static void timer_set_internal(Action action, string func)
        {
            emu_timer timer = timer_alloc_common(action, func, true);
            timer_adjust_periodic(timer, Attotime.ATTOTIME_ZERO, Attotime.ATTOTIME_NEVER);
        }
        public static void timer_list_insert(emu_timer timer1)
        {
            int i;
            int i1 = -1;
            if (timer1.func == "cpunum_empty_event_queue")
            {
                foreach (emu_timer et in lt)
                {
                    if (et.func == timer1.func && Attotime.attotime_compare(et.expire, Timer.global_basetime) <= 0)
                    {
                        i1 = lt.IndexOf(et);
                        break;
                    }
                }
            }
            if (i1 == -1)
            {
                Atime expire = timer1.enabled ? timer1.expire : Attotime.ATTOTIME_NEVER;
                for (i = 0; i < lt.Count; i++)
                {
                    if (Attotime.attotime_compare(lt[i].expire, expire) > 0)
                    {
                        break;
                    }
                }
                lt.Insert(i,timer1);
            }
        }
        public static void timer_list_remove(emu_timer timer1)
        {
            if (timer1.func == "cpunum_empty_event_queue")
            {
                List<emu_timer> lt1 = new List<emu_timer>();
                foreach (emu_timer et in lt)
                {
                    if (et.func == timer1.func && Attotime.attotime_compare(et.expire, timer1.expire) == 0)
                    {
                        lt1.Add(et);
                    }
                }
                foreach (emu_timer et1 in lt1)
                {
                    lt.Remove(et1);
                }
            }
            else
            {
                foreach (emu_timer et in lt)
                {
                    if (et.func == timer1.func)
                    {
                        lt.Remove(et);
                        break;
                    }
                }
            }
        }
        public static void timer_set_global_time(Atime newbase)
        {
            emu_timer timer;
            global_basetime = newbase;
            while (Attotime.attotime_compare(lt[0].expire, global_basetime) <= 0)
            {
                bool was_enabled = lt[0].enabled;
                timer = lt[0];
                if (Attotime.attotime_compare(timer.period, Attotime.ATTOTIME_ZERO) == 0 || Attotime.attotime_compare(timer.period, Attotime.ATTOTIME_NEVER) == 0)
                {
                    timer.enabled = false;
                }
                callback_timer_modified = false;
                callback_timer = timer;
                callback_timer_expire_time = timer.expire;
                if (was_enabled && timer.action != null)
                {
                    timer.action();
                }
                callback_timer = null;
                if (callback_timer_modified == false)
                {
                    if (timer.temporary)
                    {
                        timer_list_remove(timer);
                    }
                    else
                    {
                        timer.start = timer.expire;
                        timer.expire = Attotime.attotime_add(timer.expire, timer.period);
                        timer_list_remove(timer);
                        timer_list_insert(timer);
                    }
                }
            }
        }
        public static emu_timer timer_alloc_common(Action action, string func, bool temp)
        {
            Atime time = get_current_time();
            emu_timer timer = new emu_timer();
            timer.action = action;
            timer.enabled = false;
            timer.temporary = temp;
            timer.period = Attotime.ATTOTIME_ZERO;
            timer.func = func;
            timer.start = time;
            timer.expire = Attotime.ATTOTIME_NEVER;
            timer_list_insert(timer);
            return timer;
        }
        public static bool timer_enable(emu_timer which, bool enable)
        {
            bool old;
            old = which.enabled;
            which.enabled = enable;
            timer_list_remove(which);
            timer_list_insert(which);
            return old;
        }
        public static bool timer_enabled(emu_timer which)
        {
            return which.enabled;
        }
        public static Atime timer_timeleft(emu_timer which)
        {
            return Attotime.attotime_sub(which.expire, get_current_time());
        }
        public static void SaveStateBinary(BinaryWriter writer)
        {
            int i, i1, n;
            n = lt.Count;
            writer.Write(n);
            for (i = 0; i < n; i++)
            {
                i1 = getindexbyfunc(lt[i].func);
                writer.Write(i1);
                writer.Write(lt[i].enabled);
                writer.Write(lt[i].temporary);
                writer.Write(lt[i].period.seconds);
                writer.Write(lt[i].period.attoseconds);
                writer.Write(lt[i].start.seconds);
                writer.Write(lt[i].start.attoseconds);
                writer.Write(lt[i].expire.seconds);
                writer.Write(lt[i].expire.attoseconds);
            }
            for (i = n; i < 32; i++)
            {
                writer.Write(0);
                writer.Write(false);
                writer.Write(false);
                writer.Write(0);
                writer.Write((long)0);
                writer.Write(0);
                writer.Write((long)0);
                writer.Write(0);
                writer.Write((long)0);
            }
        }
        public static void LoadStateBinary(BinaryReader reader)
        {
            int i, i1, n;
            n = reader.ReadInt32();
            lt = new List<emu_timer>();
            for (i = 0; i < n; i++)
            {
                lt.Add(new emu_timer());
                i1 = reader.ReadInt32();
                lt[i].action = getactionbyindex(i1);
                lt[i].func = getfuncbyindex(i1);
                lt[i].enabled = reader.ReadBoolean();
                lt[i].temporary = reader.ReadBoolean();
                lt[i].period.seconds = reader.ReadInt32();
                lt[i].period.attoseconds = reader.ReadInt64();
                lt[i].start.seconds = reader.ReadInt32();
                lt[i].start.attoseconds = reader.ReadInt64();
                lt[i].expire.seconds = reader.ReadInt32();
                lt[i].expire.attoseconds = reader.ReadInt64();
                if (lt[i].func == "vblank_begin_callback")
                {
                    Video.vblank_begin_timer = lt[i];
                    lt.Remove(lt[i]);
                    lt.Add(Video.vblank_begin_timer);
                }
                else if (lt[i].func == "soft_reset")
                {
                    Mame.soft_reset_timer = lt[i];
                    lt.Remove(lt[i]);
                    lt.Add(Mame.soft_reset_timer);
                }
                else if (lt[i].func == "watchdog_callback")
                {
                    Watchdog.watchdog_timer = lt[i];
                    lt.Remove(lt[i]);
                    lt.Add(Watchdog.watchdog_timer);
                }
                else if (lt[i].func == "irq_1_0_line_hold")
                {
                    Cpuexec.timedint_timer = lt[i];
                    lt.Remove(lt[i]);
                    lt.Add(Cpuexec.timedint_timer);
                }
                else if (lt[i].func == "timer_callback_a")
                {
                    YM2151.PSG.timer_A = lt[i];
                    lt.Remove(lt[i]);
                    lt.Add(YM2151.PSG.timer_A);
                }
                else if (lt[i].func == "timer_callback_b")
                {
                    YM2151.PSG.timer_B = lt[i];
                    lt.Remove(lt[i]);
                    lt.Add(YM2151.PSG.timer_B);
                }
                else if (lt[i].func == "trigger_partial_frame_interrupt")
                {
                    Cpuexec.partial_frame_timer = lt[i];
                    lt.Remove(lt[i]);
                    lt.Add(Cpuexec.partial_frame_timer);
                }
                else if (lt[i].func == "boost_callback")
                {
                    Cpuexec.interleave_boost_timer = lt[i];
                    lt.Remove(lt[i]);
                    lt.Add(Cpuexec.interleave_boost_timer);
                }
                else if (lt[i].func == "end_interleave_boost")
                {
                    Cpuexec.interleave_boost_timer_end = lt[i];
                    lt.Remove(lt[i]);
                    lt.Add(Cpuexec.interleave_boost_timer_end);
                }
                else if (lt[i].func == "scanline0_callback")
                {
                    Video.scanline0_timer = lt[i];
                    lt.Remove(lt[i]);
                    lt.Add(Video.scanline0_timer);
                }
            }
            for (i = n; i < 32; i++)
            {
                reader.ReadInt32();
                reader.ReadBoolean();
                reader.ReadBoolean();
                reader.ReadInt32();
                reader.ReadInt64();
                reader.ReadInt32();
                reader.ReadInt64();
                reader.ReadInt32();
                reader.ReadInt64();
            }
        }
    }
}
