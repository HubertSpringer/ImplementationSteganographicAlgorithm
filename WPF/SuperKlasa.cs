using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF
{
    class SuperKlasa
    {
        Bitmap bitmap;
        int how_many_bit;
        int length;
        int begin;
        public BitArray wynik_bity;

        public SuperKlasa(Bitmap bitmap, int how_many_bit, int length, int begin)
        {
            this.bitmap = bitmap;
            this.how_many_bit = how_many_bit;
            this.length = length;
            this.begin = begin;
        }

        public String decodeHorizontal()
        {
            StringBuilder result = new StringBuilder(length);
            BitArray bits = new BitArray(length);
            byte[] byteresult;

            int dlugosc = 0;

            byte[] rgb = new byte[3];
            int indexr;
            int indexg;
            int indexb;
            bool flaga = true;
            BitArray array = new BitArray(24);

            for (int x = 0; x < bitmap.Height; x++)
                for (int y = 0; y < bitmap.Width; y++)
                {
                    if (flaga)
                    {
                        x = (begin / 24) / bitmap.Width;
                        y = (begin / 24) - (x * bitmap.Width);
                        flaga = false;
                    }
                    Color pixelColor = bitmap.GetPixel(y, x);

                    indexr = 0;
                    indexg = 0;
                    indexb = 0;

                    rgb[0] = pixelColor.R;
                    rgb[1] = pixelColor.G;
                    rgb[2] = pixelColor.B;
                    array = new BitArray(rgb);

                    for (int i = 0; i < how_many_bit; i++)
                    {
                        if (i % 3 == 0)
                        {
                            result.Append(array[indexr] ? "1" : "0");
                            bits[dlugosc] = array[indexr];
                            indexr += 1;

                        }
                        else if (i % 3 == 1)
                        {
                            result.Append(array[indexg + 8]?"1":"0") ;
                            bits[dlugosc] = array[indexg + 8];
                            indexg += 1;
                        }
                        else if (i % 3 == 2)
                        {
                            result.Append(array[indexb + 16] ? "1" : "0");
                            bits[dlugosc] = array[indexb + 16];
                            indexb += 1;
                        }
                        dlugosc++;
                        if (dlugosc>= length) break;
                    }

                    if (dlugosc >= length)
                    {
                        wynik_bity = new BitArray(bits);
                        byteresult = BitArrayToByteArray(bits);
                        return Encoding.GetEncoding(28592).GetString(byteresult);
                    }    
                }
            wynik_bity = new BitArray(bits);
            byteresult = BitArrayToByteArray(bits);
            return Encoding.GetEncoding(28592).GetString(byteresult);
        }

        public String decodevertical()
        {
            StringBuilder result = new StringBuilder(length);
            BitArray bits = new BitArray(length);
            byte[] byteresult;

            int dlugosc = 0;

            byte[] rgb = new byte[3];
            int indexr;
            int indexg;
            int indexb;
            bool flaga = true;
            BitArray array = new BitArray(24);

            for (int x = 0; x < bitmap.Width; x++)
                for (int y = 0; y < bitmap.Height; y++)
                {
                    if (flaga)
                    {
                        x = (begin / 24) / bitmap.Height;
                        y = (begin / 24) - (x * bitmap.Height);
                        flaga = false;
                    }
                    Color pixelColor = bitmap.GetPixel(x,y);

                    indexr = 0;
                    indexg = 0;
                    indexb = 0;

                    rgb[0] = pixelColor.R;
                    rgb[1] = pixelColor.G;
                    rgb[2] = pixelColor.B;
                    array = new BitArray(rgb);

                    for (int i = 0; i < how_many_bit; i++)
                    {
                        if (i % 3 == 0)
                        {
                            result.Append(array[indexr] ? "1" : "0");
                            bits[dlugosc] = array[indexr] ? true : false;
                            indexr += 1;

                        }
                        else if (i % 3 == 1)
                        {
                            result.Append(array[indexg + 8] ? "1" : "0");
                            bits[dlugosc] = array[indexg + 8] ? true : false;
                            indexg += 1;
                        }
                        else if (i % 3 == 2)
                        {
                            result.Append(array[indexb + 16] ? "1" : "0");
                            bits[dlugosc] = array[indexb + 16] ? true : false;
                            indexb += 1;
                        }
                        dlugosc++;
                        if (dlugosc >= length) break;
                    }

                    if (dlugosc >= length)
                    {
                        wynik_bity = new BitArray(bits);
                        byteresult = BitArrayToByteArray(bits);
                        return Encoding.GetEncoding(28592).GetString(byteresult);
                    }
                }
            wynik_bity = new BitArray(bits);
            byteresult = BitArrayToByteArray(bits);
            return Encoding.GetEncoding(28592).GetString(byteresult);
        }
        public byte[] BitArrayToByteArray(BitArray bits)
        {
            byte[] ret = new byte[(bits.Length - 1) / 8 + 1];
            bits.CopyTo(ret, 0);
            return ret;
        }

    }
}
