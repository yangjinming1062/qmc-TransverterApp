using System.IO;

namespace TransverterApp
{
    class Decoder
    {
        byte[][] seedMap = new byte[][] {
            new byte[]{0x4a, 0xd6, 0xca, 0x90, 0x67, 0xf7, 0x52},
            new byte[]{0x5e, 0x95, 0x23, 0x9f, 0x13, 0x11, 0x7e},
            new byte[]{0x47, 0x74, 0x3d, 0x90, 0xaa, 0x3f, 0x51},
            new byte[]{0xc6, 0x09, 0xd5, 0x9f, 0xfa, 0x66, 0xf9},
            new byte[]{0xf3, 0xd6, 0xa1, 0x90, 0xa0, 0xf7, 0xf0},
            new byte[]{0x1d, 0x95, 0xde, 0x9f, 0x84, 0x11, 0xf4},
            new byte[]{0x0e, 0x74, 0xbb, 0x90, 0xbc, 0x3f, 0x92},
            new byte[]{0x00, 0x09, 0x5b, 0x9f, 0x62, 0x66, 0xa1}
        };
        int x = -1;
        int y = 8;
        int dx = 1;
        int index = -1;

        private byte NextMask()
        {
            index += 1;
            byte ret;
            if (x < 0)
            {
                dx = 1;
                y = ((8 - y) % 8);
                ret = 0xc3;
            }
            else if (x > 6)
            {
                dx = -1;
                y = 7 - y;
                ret = 0xd8;
            }
            else
            {
                ret = seedMap[y][x];
            }
            x += dx;
            if (index == 0x8000 || (index > 0x8000 && (index + 1) % 0x8000 == 0))
                return NextMask();
            return ret;
        }

        public bool Convert(string filepath, string Outfile)
        {
            FileStream file = new FileStream(filepath, FileMode.Open);
            byte[] bt = new byte[file.Length];
            file.Read(bt, 0, bt.Length);
            file.Close();
            for (int i = 0; i < bt.Length; i++)
            {
                bt[i] = (byte)(NextMask() ^ bt[i]);
            }
            FileStream outfile = new FileStream(Outfile, FileMode.Create);
            outfile.Write(bt, 0, bt.Length);
            outfile.Close();
            return true;
        }
    }
}
