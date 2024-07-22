using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UnsafeDemo
{
    class Program
    {
        //https://docs.microsoft.com/zh-CN/dotnet/csharp/language-reference/language-specification/unsafe-code
        unsafe static void Main(string[] args)
        {
            int i = 500;
            int* pi = &i; //&取内存地址
            int** ppi = &pi;//**指针的指针  指向了i

            Console.WriteLine("i的值：" + i);
            Console.WriteLine("*pi的值：" + *pi);
            Console.WriteLine("*pi的值**(&pi)：" + **(&pi));
            Console.WriteLine("**pi的值：" + **ppi);
            Console.WriteLine();

            byte[] source = Encoding.UTF8.GetBytes("我是字节集");
            byte[] dest = new byte[source.Length];
            fixed (byte* pby = dest)//声明指针
            {
                IntPtr ptrdest = new IntPtr(pby); //转换为.net指针
                IntPtr ptrsource = Marshal.UnsafeAddrOfPinnedArrayElement(source, 0); //byte[] 转为 inptr

                Copy(ptrsource, ptrdest, source.Length);
            }
            Console.ReadKey();
        }
        unsafe static void Copy(IntPtr source, IntPtr dest, int length)
        {
            byte* psource = (byte*)source; // 转回unsfae指针
            byte* pdest = (byte*)dest;
            Console.WriteLine("源*psource：" + *psource);
            Console.WriteLine("源*pdest：" + *pdest);
            Console.WriteLine();
            for (int i = 0; i < length; i++)
            {
                *(pdest + i) = *(psource + i);
            }
            Console.WriteLine("转化后*pdest：" + *pdest);
            Console.WriteLine();
            Showstr(source, length);
            Showstr(dest, length);
            Console.WriteLine(*pdest);
            Console.ReadKey();
        }
        static void Showstr(IntPtr ptr, int length)
        {
            byte[] by = new byte[length];
            Marshal.Copy(ptr, by, 0, by.Length);
            Console.WriteLine(Encoding.UTF8.GetString(by));
        }
    }
}
