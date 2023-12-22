using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ui;

namespace mame
{
    public class Machine
    {
        public static string sName, sParent, sBoard, sDirection, sDescription, sManufacturer;
        public static List<string> lsParents;
        public static mainForm FORM;
        public static RomInfo rom;
        public static bool bRom;
        public delegate void machine_delegate();
        public static machine_delegate machine_reset_callback;
        public static void machine_start()
        {
            switch (Machine.sBoard)
            {
                case "CPS-1":
                case "CPS-1(QSound)":
                    Palette.palette_init();
                    Tilemap.tilemap_init();
                    Eeprom.eeprom_init();
                    CPS.video_start_cps();
                    machine_reset_callback = CPS.machine_reset_cps;                    
                    break;
            }
        }
        public static byte[] GetNeogeoRom(string sFile)
        {
            byte[] bb1;
            if (File.Exists("roms\\neogeo\\" + sFile))
            {
                FileStream fs1 = new FileStream("roms\\neogeo\\" + sFile, FileMode.Open);
                int n1 = (int)fs1.Length;
                bb1 = new byte[n1];
                fs1.Read(bb1, 0, n1);
                fs1.Close();
            }
            else
            {
                bb1 = null;
            }
            return bb1;
        }
        public static byte[] GetRom(string sFile)
        {
            byte[] bb1 = null;
            int n1;
            foreach (string s1 in lsParents)
            {
                if (File.Exists("roms\\" + s1 + "\\" + sFile))
                {
                    FileStream fs1 = new FileStream("roms\\" + s1 + "\\" + sFile, FileMode.Open);
                    n1 = (int)fs1.Length;
                    bb1 = new byte[n1];
                    fs1.Read(bb1, 0, n1);
                    fs1.Close();
                    break;
                }
            }
            return bb1;
        }
    }
}
