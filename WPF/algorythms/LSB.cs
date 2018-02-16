using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF
{
    class LSB : Algorithm
    {
       public Bitmap final_transformed;
       private  int how_many_bits;

        public override Bitmap transform_horizontal(BitArray bits, int begin)
        {
            transformed_bitmap = new Bitmap(bitmap);
            int bit_index = 0;
            byte[] rgb = new byte[3];
            int indexr;
            int indexg;
            int indexb;
            BitArray array = new BitArray(24);
            bool flaga = true;

            for (int x=0; x < bitmap.Height; x++)
                for (int y=0; y < bitmap.Width; y++)
                {
                    if (flaga)
                    {
                        x = (begin / 24) / bitmap.Width;
                        y = (begin / 24) - (x * bitmap.Width);
                        flaga = false;
                    }
                    Color pixelColor = transformed_bitmap.GetPixel(y, x);


                    indexr = 0;
                    indexg = 0;
                    indexb = 0;

                    rgb[0] = pixelColor.R;
                    rgb[1] = pixelColor.G;
                    rgb[2] = pixelColor.B;
                    array = new BitArray(rgb);

                    for (int i = 0; i < how_many_bits; i++)
                    {
                        if (i % 3 == 0)
                        {
                            array[indexr] = bits[bit_index];
                            indexr += 1;

                        }
                        else if (i % 3 == 1)
                        {
                            array[indexg + 8] = bits[bit_index];
                            indexg += 1;
                        }
                        else if (i % 3 == 2)
                        {
                            array[indexb + 16] = bits[bit_index];
                            indexb += 1;
                        }
                        bit_index++;
                        if (bit_index >= bits.Length) break;
                    }

                    rgb = BitArrayToByteArray(array);
                    transformed_bitmap.SetPixel(y, x, Color.FromArgb(rgb[0], rgb[1], rgb[2]));
                    if (bit_index >= bits.Length)
                    {
                        final_transformed = new Bitmap(transformed_bitmap);
                        return transformed_bitmap;
                    }
                }
            final_transformed = new Bitmap(transformed_bitmap);
            return transformed_bitmap;
        }
        public override Bitmap transform(BitArray bits, int begin)
        {
            transformed_bitmap = new Bitmap(bitmap);
            int bit_index = 0;
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
                    Color pixelColor = transformed_bitmap.GetPixel(x, y);
                   
                    indexr = 0;
                    indexg = 0;
                    indexb = 0;

                    rgb[0] = pixelColor.R;
                    rgb[1] = pixelColor.G;
                    rgb[2] = pixelColor.B;
                    array = new BitArray(rgb);

                    for (int i =0; i < how_many_bits; i++)
                    { 
                        if (i % 3 == 0)
                        {
                            array[indexr] = bits[bit_index];
                            indexr += 1;

                        }
                        else if(i % 3 == 1)
                        {
                            array[indexg+8] = bits[bit_index];
                            indexg += 1;
                        }
                        else if (i % 3 == 2)
                        {
                            array[indexb+16] = bits[bit_index];
                            indexb += 1;
                        }
                        bit_index++;
                        if (bit_index >= bits.Length) break;
                    }

                    rgb = BitArrayToByteArray(array);
                    transformed_bitmap.SetPixel(x, y, Color.FromArgb(rgb[0], rgb[1], rgb[2]));
                    if (bit_index >= bits.Length)
                    {
                        final_transformed = new Bitmap(transformed_bitmap);
                        return transformed_bitmap;
                    }
                }
            final_transformed = new Bitmap(transformed_bitmap);
            return transformed_bitmap;
        }
        public  byte[] BitArrayToByteArray(BitArray bits)
        {
            byte[] ret = new byte[(bits.Length - 1) / 8 + 1];
            bits.CopyTo(ret, 0);
            return ret;
        }
        public void load_how_many_bits(int n)
        {
            how_many_bits = n;
        }
        public int max_length_msg()
        {
            return bitmap.Width * bitmap.Height * how_many_bits;
        }
    }
}
