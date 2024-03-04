using System;
using System.IO;
using System.Text;

namespace GSIWinReTool
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (args.Length != 2)
            {
                Console.WriteLine("GSIWinReTool v1.0");
                Console.WriteLine("  created by Crsky");
                Console.WriteLine();
                Console.WriteLine("Usage:");
                Console.WriteLine("  Export text      : GSIWinReTool -e Shift_JIS [file|folder]");
                Console.WriteLine("  Rebuild script   : GSIWinReTool -b Shift_JIS [file|folder]");
                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            var mode = args[0];
            var enc = args[1];
            var path = Path.GetFullPath(args[2]);

            switch (mode)
            {
                case "-e":
                {
                    if (Directory.Exists(path))
                    {
                        foreach (var filePath in Directory.EnumerateFiles(path, "*.mes"))
                        {
                            try
                            {
                                Export(filePath, enc);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.ToString());
                            }
                        }

                        foreach (var filePath in Directory.EnumerateFiles(path, "*.lib"))
                        {
                            try
                            {
                                Export(filePath, enc);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.ToString());
                            }
                        }
                    }
                    else
                    {
                        Export(path, enc);
                    }

                    break;
                }
                case "-b":
                {
                    if (Directory.Exists(path))
                    {
                        foreach (var filePath in Directory.EnumerateFiles(path, "*.mes"))
                        {
                            try
                            {
                                Rebuild(filePath, enc);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.ToString());
                            }
                        }

                        foreach (var filePath in Directory.EnumerateFiles(path, "*.lib"))
                        {
                            try
                            {
                                Rebuild(filePath, enc);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.ToString());
                            }
                        }
                    }
                    else
                    {
                        Export(path, enc);
                    }

                    break;
                }
            }
        }

        static void Export(string path, string enc)
        {
            Console.WriteLine("Processing {0}", path);

            var txtPath = Path.ChangeExtension(path, ".txt");

            var image = new ScriptFile();
            image.Load(path);
            image.ExportText(txtPath, enc);
        }

        static void Rebuild(string path, string enc)
        {
            Console.WriteLine("Processing {0}", path);

            var txtPath = Path.ChangeExtension(path, ".txt");

            var ditPath = Path.GetDirectoryName(path);
            var outDir = Path.Combine(ditPath!, "rebuild");
            var outName = Path.GetFileName(path);
            var newPath = Path.Combine(outDir!, outName);

            Directory.CreateDirectory(outDir);

            var image = new ScriptFile();
            image.Load(path);
            image.ImportText(txtPath, enc);
            image.Save(newPath);
        }
    }
}
