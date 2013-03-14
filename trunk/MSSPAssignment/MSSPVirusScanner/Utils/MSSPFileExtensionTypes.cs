using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSSPVirusScanner.Utils
{
    /// <summary>
    /// Class containing all the hex values
    /// for the file extension types that the application 
    /// scans for.
    /// These were taken from http://www.garykessler.net/library/file_sigs.html
    /// Most of these were chosen if the word Windows or Microsoft was seen against them.
    /// </summary>
    public static class FileExtensionTypes
    {
        public const string MSDOS = "4D5A";
        public const string MSOFFICEAPP = "D0CF11E0A1B11AE1";
        public const string MSCOMPILEDHTMLHELP = "49545346";
        public const string MPEG = "FFF1";
        public const string AAC = "FFF9";
        public const string REG = "FFFE";
        public const string MSACCESS = "000100005374616E64617264204A6574204442";
        public const string EML = "52657475726E2D506174683A20";
        public const string HLP = "0000FFFFFFFF";
        public const string LIB = "213C617263683E0A";
        public const string LNK = "4C00000001140200";
        public const string MSC = "3C3F786D6C2076657273696F6E3D22312E30223F3E0D0A3C4D4D435F436F6E736F6C6546696C6520436F6E736F6C6556657273696F6E3D22";
        public const string OBJ = "4C01";
        public const string PGM = "50350A";
        public const string OOXML = "504B030414000600";
        public const string PRC = "74424D504B6E5772";
        public const string RTF = "7B5C72746631";
        public const string SYS = "EB";
    }
}
