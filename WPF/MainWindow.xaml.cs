using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LSB Lsb = new LSB();
        BitArray cryptogram_file_bits;
        Bitmap Image_to_decode;
        int decode_how_many_bits;
        int begin = 0;
        SuperKlasa superKlasa;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void load_image(Algorithm algorithm) //tutaj odblokować elementy jeżeli uda się wczytać
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "C:\\Users\\Hubi\\Desktop\\Ten semestr\\Pody\\Steganografia";
            openFileDialog.Filter = "Bitmap (*.bmp)|*.bmp";//| All files(*.*) | *.*

            if (openFileDialog.ShowDialog() == true)
            {
                string path = openFileDialog.FileName;
                algorithm.load_bmp(new Bitmap(path));
                image_LSB_loaded.Source = new BitmapImage(new Uri(path));

                textbox_how_many.IsEnabled = true;
            }
        }
        private void transform_image(Algorithm algorithm)
        {
            if (radio_Vertical.IsChecked == true)
            {
                image_LSB_transformed.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(algorithm.transform(cryptogram_file_bits, begin).GetHbitmap(),
                IntPtr.Zero, System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(500, 500));
            }
            else if (radio_horizontal.IsChecked == true)
            {
                image_LSB_transformed.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(algorithm.transform_horizontal(cryptogram_file_bits, begin).GetHbitmap(),
                IntPtr.Zero, System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(500, 500));

            }

            button_lsb_transform.IsEnabled = false;
        }
        private void button_lsb_transform_Click(object sender, RoutedEventArgs e)
        {
            transform_image(Lsb);
            l_msg_length.Content = "0";
            t_save_img.IsEnabled = true;
        }
        private void button_lsb_load_Click(object sender, RoutedEventArgs e)
        {
            load_image(Lsb);
            if (textbox_how_many.Text != "")
            {
                textbotx_max_length.Text = "" + Lsb.max_length_msg();
            }
            image_LSB_transformed.Source = null;
        }
        private void textbox_how_many_TextChanged(object sender, TextChangedEventArgs e)
        {
            Regex pattern = new Regex("^[1-9]$|^1[0-9]$|^2[0-4]$");
            if (pattern.IsMatch(textbox_how_many.Text))
            {
                Lsb.load_how_many_bits(Int32.Parse(textbox_how_many.Text));
                textbotx_max_length.Text = "" + Lsb.max_length_msg();
                button_crytpogram_load.IsEnabled = true;
            }
            else if (textbox_how_many.Text == "") {
                textbotx_max_length.Text = "";
                button_crytpogram_load.IsEnabled = false;
            }
            else
            {
                MessageBox.Show("Please enter a number from 1 - 24 ");
                button_crytpogram_load.IsEnabled = false;
            }
        }
        private void button_crytpogram_load_Click(object sender, RoutedEventArgs e)
        {
            BitArray file = load_any_file();
            if (file != null)
            {
                if (file.Length > Int32.Parse(textbotx_max_length.Text))
                {
                    MessageBox.Show("Message is to long\n Message length: " + file.Length + "\nfree space: " + Int32.Parse(textbotx_max_length.Text));
                    button_lsb_transform.IsEnabled = false;
                }
                else
                {
                    l_msg_length.Content = file.Length;
                    cryptogram_file_bits = file;
                    button_lsb_transform.IsEnabled = true;
                    t_begin.IsEnabled = true;
                    t_begin.Text = "0";
                    begin = 0;
                    t_end.Text = "" + (file.Length-1);
                    l_difference.Content = Lsb.max_length_msg() - cryptogram_file_bits.Length - 1;
                }
            }
            else
            {
                MessageBox.Show("Nie udało się wczytać pliku");
                button_lsb_transform.IsEnabled = false;
            }
        }

        private BitArray load_any_file()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "C:\\Users\\Hubi\\Desktop\\Ten semestr\\Pody\\Steganografia";
            openFileDialog.Filter = " All files(*.*) | *.*";

            if (openFileDialog.ShowDialog() == true)
            {
                byte[] bytes = File.ReadAllBytes(openFileDialog.FileName);
                BitArray bits = new BitArray(bytes);
                return bits;
            }
            return null;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void t_save_img_Click(object sender, RoutedEventArgs e)
        {
            save_File("jej");

        }
        private void save_File(String message)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save As...";
            saveFileDialog.Filter = "bmp (*.bmp)|*.bmp";
            saveFileDialog.InitialDirectory = "C:\\Users\\Hubi\\Desktop\\Ten semestr\\Pody\\Steganografia";
            saveFileDialog.FilterIndex = 1;

            if (saveFileDialog.ShowDialog() == true)
            {
                if (saveFileDialog.FilterIndex == 1)
                {
                    Lsb.final_transformed.Save(saveFileDialog.FileName);
                }
            }
        }

        private void button_lsb_load_Copy_Click(object sender, RoutedEventArgs e)
        {
            load_image();
        }

        private void load_image()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "C:\\Users\\Hubi\\Desktop\\Ten semestr\\Pody\\Steganografia";
            openFileDialog.Filter = "Bitmap (*.bmp)|*.bmp";

            if (openFileDialog.ShowDialog() == true)
            {
                string path = openFileDialog.FileName;
                Image_to_decode = new Bitmap(path);
                image_with_msg.Source = new BitmapImage(new Uri(path));
            }
        }

        private void t_hm_TextChanged(object sender, TextChangedEventArgs e)
        {
            Regex pattern = new Regex("^[1-9]$|^1[0-9]$|^2[0-4]$");
            if (pattern.IsMatch(textbox_how_many_Copy.Text))
            {
                decode_how_many_bits = Int32.Parse(textbox_how_many_Copy.Text);
            }
            else if (textbox_how_many.Text == "")
            {
            }
            else
            {
                MessageBox.Show("Please enter a number from 1 - 24 ");
                button_crytpogram_load.IsEnabled = false;

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            superKlasa = new SuperKlasa(Image_to_decode, decode_how_many_bits, Int32.Parse(t_decode_length.Text), Int32.Parse(t_decode_begin.Text));

            if (radio_Vertical1.IsChecked == true)
            {
                Wynik.Text = superKlasa.decodevertical();
            }
            if (radio_horizontal1.IsChecked == true)
            {
                Wynik.Text = superKlasa.decodeHorizontal();
            }
        }

        private void t_end_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void t_begin_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (t_begin.Text != "")
            {
                int begin2 = Int32.Parse(t_begin.Text);
                if (begin2 >= 0 && begin2 < Lsb.max_length_msg() - cryptogram_file_bits.Length)
                {
                    begin = begin2;
                    t_end.Text = " " + (begin2 + cryptogram_file_bits.Length-1);
                }
                else
                {
                    MessageBox.Show("Bad Section ");
                    button_crytpogram_load.IsEnabled = false;

                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save As...";
            saveFileDialog.Filter = "All files (*.*)|*.*";
            saveFileDialog.InitialDirectory = "C:\\Users\\Hubi\\Desktop\\Ten semestr\\Pody\\Steganografia";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == true)
            {
             Byte[] byteresult = BitArrayToByteArray(superKlasa.wynik_bity);
             File.WriteAllText(saveFileDialog.FileName, Encoding.GetEncoding(28592).GetString(byteresult));
            }
            
        }
        public byte[] BitArrayToByteArray(BitArray bits)
        {
            byte[] ret = new byte[(bits.Length - 1) / 8 + 1];
            bits.CopyTo(ret, 0);
            return ret;
        }

    }

   
}


