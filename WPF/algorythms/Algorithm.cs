using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF
{
    abstract class Algorithm
    {
        protected Bitmap bitmap;
        protected Bitmap transformed_bitmap;

       public void load_bmp(Bitmap bitmap)
        {
            this.bitmap = bitmap;
        }

        abstract public Bitmap transform(BitArray bits,int begin);

        abstract public Bitmap transform_horizontal(BitArray bits, int begin);

    }
}
