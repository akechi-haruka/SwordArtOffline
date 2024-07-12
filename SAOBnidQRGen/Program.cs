using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace SAOBnidQRGen {
    internal class Program {


        static void Main(string[] args) {
            if (args.Length != 2) {
                Console.WriteLine("Usage: SAOBnidQRGen <content> <outfile>");
                return;
            }
            String text = args[0];
            String file = args[1];

            byte[] src_byte = Encoding.UTF8.GetBytes(text);

            nbamQr_retval_t nbamQr_retval_t = Initialize();
            if (nbamQr_retval_t != nbamQr_retval_t.NBAMQR_RETVAL_NO_ERROR) {
                Console.WriteLine("Initialize: " + nbamQr_retval_t);
                return;
            }
            nbamQr_encode_info_t nbamQr_encode_info_t = new nbamQr_encode_info_t();
            nbamQr_encode_info_t.str = Marshal.AllocHGlobal(256);
            byte[] array = new byte[256];
            encryptData(src_byte, out array);
            Marshal.Copy(array, 0, nbamQr_encode_info_t.str, array.Length);
            nbamQr_encode_info_t.len = (uint)array.Length;
            nbamQr_encode_info_t.mode = nbamQr_mode_t.NBAMQR_MODE_BYTE;
            nbamQr_encode_info_t.level = nbamQr_level_t.NBAMQR_LEVEL_H;
            nbamQr_encode_info_t.version = 6U;
            nbamQr_encode_info_t.quietWidth = 4U;
            nbamQr_encode_info_t.ppm = 3U;
            nbamQr_encode_info_t.rgbData = IntPtr.Zero;
            nbamQr_encode_info_t.dataBytes = 0U;
            nbamQr_encode_info_t.width = 0U;
            nbamQr_encode_info_t.height = 0U;
            nbamQr_encode_info_t.rowBytes = 0U;
            nbamQr_retval_t = GetQrImageSize(nbamQr_encode_info_t);
            if (nbamQr_retval_t != nbamQr_retval_t.NBAMQR_RETVAL_NO_ERROR) {
                Console.WriteLine("GetQrImageSize: " + nbamQr_retval_t);
                return;
            } else {
                nbamQr_encode_info_t.rgbData = Marshal.AllocHGlobal((int)nbamQr_encode_info_t.dataBytes);
                nbamQr_retval_t = Encode(nbamQr_encode_info_t);
                if (nbamQr_retval_t != nbamQr_retval_t.NBAMQR_RETVAL_NO_ERROR) {
                    Console.WriteLine("Encode: " + nbamQr_retval_t);
                    return;
                } else {
                    byte[] array2 = new byte[nbamQr_encode_info_t.dataBytes];
                    Marshal.Copy(nbamQr_encode_info_t.rgbData, array2, 0, (int)nbamQr_encode_info_t.dataBytes);
                    Bitmap texture2D = new Bitmap((int)nbamQr_encode_info_t.width, (int)nbamQr_encode_info_t.height);
                    int num = 0;
                    while ((long)num < (long)((ulong)nbamQr_encode_info_t.height)) {
                        int num2 = 0;
                        while ((long)num2 < (long)((ulong)nbamQr_encode_info_t.width)) {
                            int num3 = (int)(nbamQr_encode_info_t.width - (uint)num2 - 1U);
                            int num4 = (int)(nbamQr_encode_info_t.height - (uint)num - 1U);
                            int num5 = num3 * (int)nbamQr_encode_info_t.rowBytes + num4 * 3;
                            Color color = Color.FromArgb(255, array2[num5], array2[num5], array2[num5]);
                            texture2D.SetPixel(num2, num, color);
                            num2++;
                        }
                        num++;
                    }
                    texture2D.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    texture2D.Save(file, ImageFormat.Png);
                }
                Marshal.FreeHGlobal(nbamQr_encode_info_t.rgbData);
            }
            Marshal.FreeHGlobal(nbamQr_encode_info_t.str);

            Console.WriteLine("OK");
        }

        private static string encryptData(byte[] src_str, out byte[] out_data) {
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            byte[] array;
            byte[] array2;
            generateKey(rijndaelManaged.KeySize, out array, rijndaelManaged.BlockSize, out array2);
            rijndaelManaged.Key = array;
            rijndaelManaged.IV = array2;
            ICryptoTransform cryptoTransform = rijndaelManaged.CreateEncryptor();
            byte[] array3 = cryptoTransform.TransformFinalBlock(src_str, 0, src_str.Length);
            cryptoTransform.Dispose();
            out_data = array3;
            return Convert.ToBase64String(array3);
        }

        private static void generateKey(int keySize, out byte[] key, int blockSize, out byte[] iv) {
            byte[] bytes = Encoding.UTF8.GetBytes("暗号化のための乱数作成");
            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes("ProjectLinkEncrypt", bytes);
            rfc2898DeriveBytes.IterationCount = 1000;
            key = rfc2898DeriveBytes.GetBytes(keySize / 8);
            iv = rfc2898DeriveBytes.GetBytes(blockSize / 8);
        }

        [DllImport("nbamQr", CallingConvention = CallingConvention.StdCall)]
        public static extern nbamQr_retval_t Initialize();

        [DllImport("nbamQr", CallingConvention = CallingConvention.StdCall)]
        public static extern nbamQr_retval_t Terminate();

        [DllImport("nbamQr", CallingConvention = CallingConvention.StdCall)]
        public static extern nbamQr_retval_t GetQrImageSize([In][Out] nbamQr_encode_info_t info);

        [DllImport("nbamQr", CallingConvention = CallingConvention.StdCall)]
        public static extern nbamQr_retval_t Encode([In][Out] nbamQr_encode_info_t info);


        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public class nbamQr_encode_info_t {
            public IntPtr str;

            public uint len;

            public nbamQr_mode_t mode;

            public nbamQr_level_t level;

            public uint version;

            public uint quietWidth;

            public uint ppm;

            public IntPtr rgbData;

            public uint dataBytes;

            public uint width;

            public uint height;

            public uint rowBytes;
        }

        public enum nbamQr_mode_t {
            NBAMQR_MODE_NUMBER,
            NBAMQR_MODE_ALPHANUM,
            NBAMQR_MODE_BYTE,
            NBAMQR_MODE_MIX
        }

        public enum nbamQr_level_t {
            NBAMQR_LEVEL_L,
            NBAMQR_LEVEL_M,
            NBAMQR_LEVEL_Q,
            NBAMQR_LEVEL_H
        }

        public enum nbamQr_retval_t {
            NBAMQR_RETVAL_NO_ERROR,
            NBAMQR_RETVAL_MODE_ERROR,
            NBAMQR_RETVAL_SIZE_ERROR,
            NBAMQR_RETVAL_INVALID_VERSION,
            NBAMQR_RETVAL_FORMAT_INFO_ERROR,
            NBAMQR_RETVAL_MARKER_ERROR,
            NBAMQR_RETVAL_VERSION_INFO_ERROR,
            NBAMQR_RETVAL_VERSION_INFO_MISMATCH,
            NBAMQR_RETVAL_DECODE_ERROR,
            NBAMQR_RETVAL_ERROR_CORRECTION_ERROR,
            NBAMQR_RETVAL_NO_BUFFER,
            NBAMQR_RETVAL_YUV_TYPE_ERROR,
            NBAMQR_RETVAL_YUV_WIDTH_ERROR,
            NBAMQR_RETVAL_NOT_INITIALIZED,
            NBAMQR_RETVAL_NOT_IMPLEMENTED_MODE,
            NBAMQR_RETVAL_ENCODE_ERROR
        }
    }
}
