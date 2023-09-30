using System;
using System.Collections.Generic;
using System.Linq;
using LZStringCSharp;
using Platematica.Systems;

namespace Platematica.Utils
{
    public class PlateUpPlannerConverter
    {
        public static Schematic ConvertURLToSchematic(string url, string name = "PlateUpPlanner Schematic")
        {
            return ConvertReadbleToSchematic(ConvertURLToReadable(url), name);
        }
        
        public static string ConvertURLToReadable(string url)
        {
            return LZString.DecompressFromBase64(url.Split('#')[1]);
        }

        public static Schematic ConvertReadbleToSchematic(string readable, string name)
        {
            if (string.IsNullOrEmpty(readable))
                return null;
            
            
            string[] args = readable.Split(' ');
            
            string version = args[0];
            string size = args[1];
            int xSize = int.Parse(size.Split('x')[1]);
            int zSize = int.Parse(size.Split('x')[0]);
            string data = args[2];
            
            if (version != "v2")
                return null;
            
            Schematic schematic = new Schematic();
            
            List<string> dataChunks = new List<string>();
            for (int i = 0; i < data.Length; i += 3)
            {
                dataChunks.Add(data.Substring(i, Math.Min(3, data.Length - i)));
            }
            
            string[,] grid = new string[xSize, zSize];
            int x = 0;
            int z = 0;
            foreach (string chunk in dataChunks)
            {
                grid[x, z] = chunk;
                x++;
                if (x >= xSize)
                {
                    x = 0;
                    z++;
                }
            }
            for (int i = 0; i < zSize; i++)
            {
                for (int j = 0; j < xSize; j++)
                {
                    string code = grid[j, i].Substring(0, 2); // Planner Code
                    char rotation = grid[j, i].Last(); // Planner Rotation
                    
                    int rotationOffset = 0;
                    int grabberDirection = 0;
                    bool isDirectional = false;

                    if (!applianceMap.ContainsKey(code)) // Ensure it's a valid code
                        continue;

                    switch (rotation) // Setup rotation offset
                    {
                        case 'u':
                            rotationOffset = 0;
                            break;
                        case 'r':
                            rotationOffset = 90;
                            break;
                        case 'd':
                            rotationOffset = 180;
                            break;
                        case 'l':
                            rotationOffset = 270;
                            break;
                    }

                    switch (code) // Fix rotation for Corner Grabbers & Assign Grabber Direction
                    {
                        case "3V":
                            grabberDirection = 4;
                            break;
                        case "U7":
                            grabberDirection = 1;
                            break;
                    }
                    
                    if (CreateHologram.DirectionalIDs.Contains(applianceMap[code])) // Check if appliance is directional
                    {
                        isDirectional = true;
                    }

                    
                    schematic.components.Add(new SchematicComponent // Add component to schematic
                    {
                        xOffset = j,
                        zOffset = -i,
                        applianceID = applianceMap[code],
                        rotationOffset = rotationOffset,
                        rotatedGrabberDirection = grabberDirection,
                        isDirectional = isDirectional
                    });
                }
            }
            
            schematic.xSize = xSize;
            schematic.zSize = zSize;
            schematic.name = name;

            return schematic;
        }
        
        public static Dictionary<string, int> applianceMap = new Dictionary<string, int>  {
                    { "60", 505496455 },
                    { "eY", -1357906425 },
                    { "AY", -1440053805 },
                    { "Z9", 1329097317 },
                    { "oH", -1013770159 },
                    { "2V", 2127051779 },
                    { "Qs", -1632826946 },
                    { "70", -1855909480 },
                    { "n2", 481495292 },
                    { "0R", 1551609169 },
                    { "Ad", 1351951642 },
                    { "3D", 1765889988 },
                    { "6O", -1495393751 },
                    { "VX", 1776760557 },
                    { "HD", -1993346570 },
                    { "1Z", -1723340146 },
                    { "9V", -2147057861 },
                    { "fU", 1973114260 },
                    { "w5", -1906799936 },
                    { "sC", -1238047163 },
                    { "BM", -1029710921 },
                    { "Dg", -1462602185 },
                    { "zg", 459840623 },
                    { "ze", -1248669347 },
                    { "E2", -1573577293 },
                    { "qV", 756364626 },
                    { "H5", 532998682 },
                    { "e6", 1921027834 },
                    { "5V", -770041014 },
                    { "ud", -1448690107 },
                    { "96", 1266458729 },
                    { "O9", 1154757341 },
                    { "UQ", 862493270 },
                    { "5d", -1813414500 },
                    { "F5", -571205127 },
                    { "5T", -729493805 },
                    { "4K", 1586911545 },
                    { "CR", 1446975727 },
                    { "8B", 1139247360 },
                    { "pj", 238041352 },
                    { "UJ", -1817838704 },
                    { "Gt", -246383526 },
                    { "hM", -1610332021 },
                    { "2Q", -1311702572 },
                    { "JD", -1068749602 },
                    { "yi", -905438738 },
                    { "FG", 1807525572 },
                    { "uW", -484165118 },
                    { "dG", -1573812073 },
                    { "Dc", 759552160 },
                    { "Ar", -452101383 },
                    { "zQ", -117339838 },
                    { "NV", 961148621 },
                    { "Ls", -609358791 },
                    { "AG", 925796718 },
                    { "vu", -1533430406 },
                    { "SS", 1193867305 },
                    { "5B", -1097889139 },
                    { "Ja", 1834063794 },
                    { "WU", -1963699221 },
                    { "2o", -1434800013 },
                    { "Qi", -1201769154 },
                    { "ET", -1506824829 },
                    { "0s", -1353971407 },
                    { "2M", 434150763 },
                    { "ao", 380220741 },
                    { "m2", 1313469794 },
                    { "NH", -957949759 },
                    { "2A", 235423916 },
                    { "94", 314862254 },
                    { "wn", -1857890774 },
                    { "ot", -759808000 },
                    { "31", 1656358740 },
                    { "r6", 639111696 },
                    { "Yn", 1358522063 },
                    { "J1", 221442949 },
                    { "mD", 1528688658 },
                    { "zZ", 2080633647 },
                    { "CZ", 446555792 },
                    { "qC", 1648733244 },
                    { "tV", -3721951 },
                    { "T2", -34659638 },
                    { "cJ", -203679687 },
                    { "GM", -2019409936 },
                    { "lq", 209074140 },
                    { "WS", 1738351766 },
                    { "1P", 624465484 },
                    { "kv", 2023704259 },
                    { "ZE", 723626409 },
                    { "kF", 1796077718 },
                    { "py", 230848637 },
                    { "3G", 1129858275 },
                    { "1g", -214126192 },
                    { "W1", 1083874952 },
                    { "v2", 1467371088 },
                    { "Nt", 1860904347 },
                    { "bZ", -266993023 },
                    { "IX", 303858729 },
                    { "zd", -2133205155 },
                    { "jC", 976574457 },
                    { "hp", 739504637 },
                    { "xm", -823922901 },
                    { "j0", -2092567672 },
                    { "P7", 385684499 },
                    { "HL", 148543530 },
                    { "6D", -1609758240 },
                    { "gN", 735786885 },
                    { "1D", -1132411297 },
                    { "Sx", 1799769627 },
                    { "We", -965827229 },
                    { "NG", -117356585 },
                    { "i9", -1210117767 },
                    { "CH", -1507801323 },
                    { "jt", 1800865634 },
                    { "E5", 269523389 },
                    { "fH", -2042103798 },
                    { "co", 44541785 },
                    { "I4", -1055654549 },
                    { "XJ", 595306349 },
                    { "um", -471813067 },
                    { "1K", -712909563 },
                    { "3V", -331651461 },
                    { "U7", -331651461 },
                    { "mq", -331651461 },
                };
    }
}