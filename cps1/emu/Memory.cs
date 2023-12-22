using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mame
{
    public class Memory
    {
        public static byte[] mainrom, audiorom, mainram, audioram;
        public static void memory_reset()
        {
            switch (Machine.sBoard)
            {
                case "CPS-1":
                case "CPS-1(QSound)":
                    CPS.sbyte0 = -1;
                    CPS.short1 = -1;
                    CPS.short2 = -1;
                    CPS.sbyte3 = -1;
                    break;
            }
        }
        public static void memory_reset2()
        {
            switch (Machine.sBoard)
            {
                case "CPS-1":
                case "CPS-1(QSound)":
                    CPS.sbyte0_old = 0;
                    CPS.short1_old = 0;
                    CPS.short2_old = 0;
                    CPS.sbyte3_old = 0;
                    break;
            }
        }
    }
}