using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace mame
{
    public class State
    {
        public delegate void savestate_delegate(BinaryWriter sw);
        public delegate void loadstate_delegate(BinaryReader sr);
        public static savestate_delegate savestate_callback;
        public static loadstate_delegate loadstate_callback;
        public static void state_init()
        {
            switch (Machine.sBoard)
            {
                case "CPS-1":
                    savestate_callback = CPS.SaveStateBinaryC;
                    loadstate_callback = CPS.LoadStateBinaryC;
                    break;
                case "CPS-1(QSound)":
                    savestate_callback = CPS.SaveStateBinaryQ;
                    loadstate_callback = CPS.LoadStateBinaryQ;
                    break;
            }
        }
    }
}
