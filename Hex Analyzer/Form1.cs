using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hex_Analyzer
{
  
    public partial class Form1 : Form
    {
        String FlashCodeSection;
        String FlashConfigSection;
        String CompiledCodeSection;
        String CompiledConfigSection;
        public Form1()
        {
            InitializeComponent();
        }
        String hexFilStr;
        //String hexFileCompiledStr;
        private void btn_load_flash_Click(object sender, EventArgs e)
        {
            Stream hexFileFlash;
           
            byte[] hexFileFlashBytes;
            FileLoader.Filter = "Intel Hex File|*.hex";
            FileLoader.Title = "Load Flash Hex File";
            if (FileLoader.ShowDialog() != DialogResult.OK)
                return;
            txt_flash_file.Text = FileLoader.FileName;
            hexFileFlash = FileLoader.OpenFile();
            hexFileFlashBytes = new byte[hexFileFlash.Length];
            hexFileFlash.Read(hexFileFlashBytes, 0, (int)hexFileFlash.Length);
            hexFileFlash.Close();

            hexFilStr = Encoding.ASCII.GetString(hexFileFlashBytes);
            if (cmbChipModel.SelectedItem.ToString() == "Nrf51x FOB")
                ParserForNordicModel_ReadbackHex();
            else if (cmbChipModel.SelectedItem.ToString() == "CC2540 Terminal")
                ParserForTICC2540("ReadBack");
        }

        /* TI memory structure- Map file
         
                ****************************************
                *                                      *
                *      SEGMENTS IN ADDRESS ORDER       *
                *                                      *
                ****************************************


SEGMENT              SPACE    START ADDRESS   END ADDRESS     SIZE  TYPE  ALIGN
=======              =====    =============   ===========     ====  ====  =====
INTVEC               CODE          00000000 - 00000085          86   com    0
CSTART               CODE          00000086 - 00000136          B1   rel    0
BIT_ID               CODE               00000137                     dse    0
PDATA_ID             CODE               00000137                     dse    0
IXDATA_ID            CODE               00000137                     dse    0
IDATA_ID             CODE               00000137                     dse    0
DATA_ID              CODE               00000137                     dse    0
BDATA_ID             CODE               00000137                     dse    0
XDATA_ID             CODE          00000137 - 00000222          EC   rel    0
BANK_RELAYS          CODE          00000223 - 000014BE        129C   rel    0
RCODE                CODE          000014BF - 00001D50         892   rel    0
CODE_N               CODE               00001D51                     dse    0
DIFUNCT              CODE               00001D51                     dse    0
NEAR_CODE            CODE          00001D54 - 000025E3         890   rel    2
<BANKED_CODE,CODE_C> 1
                     CODE          000025E4 - 00007FFC        5A19   rel    2
<BANKED_CODE,XDATA_ROM_C_FLASH> 1
                     CODE          00008000 - 0000FFFD        7FFE   rel    0
<BANKED_CODE> 1      CODE          00010000 - 00017FF2        7FF3   rel    0
<BANKED_CODE> 2      CODE          00018000 - 0001FFF9        7FFA   rel    0
<BANKED_CODE> 3      CODE          00020000 - 000211A8        11A9   rel    0
BLENV_ADDRESS_SPACE
                     CODE          0003E800 - 0003F7FF        1000   rel    0
REGISTERS            DATA          00000000 - 00000007           8   rel    0
VREG                 DATA          00000008 - 00000017          10   rel    0
PSP                  DATA               00000018                     dse    0
XSP                  DATA          00000018 - 00000019           2   rel    0
DATA_I               DATA               0000001A                     dse    0
DATA_Z               DATA          0000001A - 0000001B           2   rel    0
BREG                 BIT        00000020.0  -  00000020.7        8   rel    0
SFR_AN               DATA          00000080 - 00000080           1   rel    0
                     DATA          00000088 - 00000088           1 
                     DATA          0000008B - 0000008B           1 
                     DATA          0000008D - 0000008D           1 
                     DATA          00000090 - 00000091           2 
                     DATA          00000094 - 00000097           4 
                     DATA          0000009A - 0000009F           6 
                     DATA          000000A1 - 000000A9           9 
                     DATA          000000AB - 000000AC           2 
                     DATA          000000AE - 000000AF           2 
                     DATA          000000B3 - 000000B4           2 
                     DATA          000000B6 - 000000B6           1 
                     DATA          000000B8 - 000000C0           9 
                     DATA          000000C3 - 000000C3           1 
                     DATA          000000C6 - 000000C7           2 
                     DATA          000000C9 - 000000C9           1 
                     DATA          000000D1 - 000000D7           7 
                     DATA          000000D9 - 000000DB           3 
                     DATA          000000E1 - 000000E5           5 
                     DATA          000000E8 - 000000E9           2 
                     DATA          000000F2 - 000000F2           1 
                     DATA          000000F2 - 000000F5           4 
                     DATA          000000FD - 000000FF           3 
XSTACK               XDATA         00000001 - 00000280         280   rel    0
XDATA_Z              XDATA         00000281 - 00001D5E        1ADE   rel    0
XDATA_I              XDATA         00001D5F - 00001E4A          EC   rel    0
<XDATA_ROM_C> 1      CONST         00008000 - 00009015        1016   rel    0
IDATA_I              IDATA              0000001C                     dse    0
IDATA_Z              IDATA         0000001C - 0000001D           2   rel    0
ISTACK               IDATA         00000040 - 000000FF          C0   rel    0

                ****************************************
                *                                      *
                *        END OF CROSS REFERENCE        *
                *                                      *
                ****************************************

         */

        private void ParserForTICC2540(string _type)
        {
            
            byte[] Full256KBMemory = Enumerable.Repeat((byte)0xFF, 0x3ffff + 1).ToArray();

            
            String[] Records = hexFilStr.Split(':');

            
            ushort Segment = 0x0000;
            uint  AbsoluteAddress = 0x00000000;
            // Parse records and fill array
            HexRecord _hexRecord;
            int i = 0;
            while (i < Records.Length)
            {
                String currentRecord = Records[i++];
                char[] enter = new char[] { '\r', '\n' };
                currentRecord = currentRecord.Trim(enter);
                if (currentRecord == "")
                    continue;

                _hexRecord = new Hex_Analyzer.HexRecord(StringToByteArray(currentRecord));
                if (_hexRecord.Status != 0x00)
                {
                    MessageBox.Show("Something went wrong with parsing the hex file on line: " + i.ToString(), "Parsing error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //Start filling Codearea, start address is considered 0000:0000
                if (_hexRecord.Tag == 0x00)
                {
                    AbsoluteAddress = Segment;
                    AbsoluteAddress <<= 16;//2 bytes shift to left as base address
                    AbsoluteAddress += _hexRecord.Offset;

                    Array.Copy(_hexRecord.Data, 0, Full256KBMemory, AbsoluteAddress, _hexRecord.Lenght);
                }
                else if (_hexRecord.Tag == 0x04)
                {
                    //Starting new segment
                    if (_hexRecord.Data.Length != 2)
                    {
                        MessageBox.Show("Something went wrong with parsing the hex file on line: " + i.ToString(), "Parsing error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    Segment = _hexRecord.Data[0];
                    Segment <<= 8;//1 byte shift to left
                    Segment += _hexRecord.Data[1];
                }
                else if(_hexRecord.Tag== 0x05 || _hexRecord.Tag == 0x01)
                {
                    //End of file
                    continue;
                }
            }
            //
            //Code starts at 0000:0000 and ends at 0002:11A8 based on the map file
            byte[] CodeSection = new byte[0x211a8];
            Array.Copy(Full256KBMemory, CodeSection, 0x211a8);
            //NV starts at 0003:E800 and ends at 0003:F7FF based on the map file
            byte[] ConfigSection = new byte[0x1000];//2KB configuran data (0xF7FF-0xE800+1)
            Array.Copy(Full256KBMemory, 0x3e800, ConfigSection, 0, 0x1000);
            //
            if (_type == "Compiled")
            {
                label_compiled.Text = "";
                CompiledCodeSection = ByteArrayToString(CodeSection);
                CompiledConfigSection = ByteArrayToString(ConfigSection);
                if (cmbprogram_section.SelectedIndex == 0)
                    text_compiled.Text = CompiledCodeSection;
                else
                    text_compiled.Text = CompiledConfigSection;
            }
            else
            {
                label_flash.Text = "";
                FlashCodeSection = ByteArrayToString(CodeSection);
                FlashConfigSection = ByteArrayToString(ConfigSection);
                if (cmbprogram_section.SelectedIndex == 0)
                    text_flash.Text = FlashCodeSection;
                else
                    text_flash.Text = FlashConfigSection;
            }
        }
        public static byte[] StringToByteArrayFormal(string hex)
        {
            if (hex == null)
                return null;
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
        public static int GetHexVal(char hex)
        {
            int val = (int)hex;
            //For uppercase A-F letters:
            return val - (val < 58 ? 48 : 55);
            //For lowercase a-f letters:
            //return val - (val < 58 ? 48 : 87);
            //Or the two combined, but a bit slower:
            //return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }
        public static byte[] StringToByteArray(string hex)
        {
            if (hex.Length % 2 == 1)
                throw new Exception("The binary key cannot have an odd number of digits");

            byte[] arr = new byte[hex.Length >> 1];

            for (int i = 0; i < hex.Length >> 1; ++i)
            {
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
            }

            return arr;
        }
        private void ParserForNordicModel_ReadbackHex()
        {
            int totalSize = 0, totalConfigSize = 0;
            int i = 0;
            String[] Records = hexFilStr.Split(':');
            int upperAddress = 0, nextRecAddress = -1;
            bool IsCodeZone = true;
            FlashCodeSection = "";
            FlashConfigSection = "";
            // Record Parser
            while (i < Records.Length)
            {
                String currentRecord = Records[i++];
                char[] enter = new char[] { '\r', '\n' };
                currentRecord = currentRecord.Trim(enter);
                if (currentRecord == "")
                    continue;
                byte recordLen = Enumerable.Range(0, 2).Where(x => x % 2 == 0).Select(x => Convert.ToByte(currentRecord.Substring(x, 2), 16)).ToArray()[0];
                byte[] recordAddrByte = Enumerable.Range(2, 4).Where(x => x % 2 == 0).Select(x => Convert.ToByte(currentRecord.Substring(x, 2), 16)).ToArray();
                int recordAddr = recordAddrByte[0] * 256 + recordAddrByte[1];
                byte recordType = Enumerable.Range(6, 2).Where(x => x % 2 == 0).Select(x => Convert.ToByte(currentRecord.Substring(x, 2), 16)).ToArray()[0];
                byte[] recordData = Enumerable.Range(8, recordLen * 2).Where(x => x % 2 == 0).Select(x => Convert.ToByte(currentRecord.Substring(x, 2), 16)).ToArray();
                byte recordCrc = Enumerable.Range(recordLen * 2 + 8, 2).Where(x => x % 2 == 0).Select(x => Convert.ToByte(currentRecord.Substring(x, 2), 16)).ToArray()[0];
                String DataHex = ByteArrayToString(recordData);

                if (recordType == 4)
                {
                    upperAddress = recordData[0] * 256 + recordData[1];
                }
                else if (recordType == 0)
                {
                    int completeAddress = ((upperAddress << 16) | (recordAddr & 0xFFFF));

                    // Make sure data are aligned
                    if (recordData.Length % 4 != 0)
                        DataHex += new String('F', (4 - (recordData.Length % 4)) * 2);


                    if (nextRecAddress == -1)
                        nextRecAddress = completeAddress;

                    if (nextRecAddress != completeAddress)
                    {
                        IsCodeZone = false;
                    }
                    else if (IsCodeZone)
                        FlashCodeSection += DataHex + "\r\n";
                    else if (!IsCodeZone)
                        FlashConfigSection += DataHex + "\r\n";

                    if (IsCodeZone)
                        totalSize += (DataHex.Length / 2);
                    else if (!IsCodeZone)
                        totalConfigSize += (DataHex.Length / 2);

                    nextRecAddress = completeAddress + recordLen;
                }
                else if (recordType == 1)
                {
                    break;
                }
            }
            if (cmbprogram_section.SelectedIndex == 0)
                text_flash.Text = FlashCodeSection;
            else
                text_flash.Text = FlashConfigSection;

            label_flash.Text = String.Format("Code Size : {0}\nConfiguration Size : {1}", totalSize, totalConfigSize);
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbprogram_section.SelectedIndex = 0;
            cmbChipModel.SelectedIndex = 0;
        }

        private void program_section_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbprogram_section.SelectedIndex == 0)
            {
                text_flash.Text = FlashCodeSection;
                text_compiled.Text = CompiledCodeSection;
            }
            else
            {
                text_flash.Text = FlashConfigSection;
                text_compiled.Text = CompiledConfigSection;
            }
        }
        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:X2}", b);
            return hex.ToString();
        }
        
        private void btn_compiled_Click(object sender, EventArgs e)
        {
            Stream hexFileCompiled;


            byte[] hexFileCompiledBytes;
            FileLoader.Filter = "Intel Hex File|*.hex";
            FileLoader.Title = "Load Compiled Hex File";
            if (FileLoader.ShowDialog() != DialogResult.OK)
                return;
            txt_compiled_file.Text = FileLoader.FileName;
            hexFileCompiled = FileLoader.OpenFile();
            hexFileCompiledBytes = new byte[hexFileCompiled.Length];
            hexFileCompiled.Read(hexFileCompiledBytes, 0, (int)hexFileCompiled.Length);
            hexFileCompiled.Close();

            hexFilStr = Encoding.ASCII.GetString(hexFileCompiledBytes);
            if (cmbChipModel.SelectedItem.ToString() == "Nrf51x FOB")
                ParserForNordicModel_CompiledHex();
            else if (cmbChipModel.SelectedItem.ToString() == "CC2540 Terminal")
                ParserForTICC2540("Compiled");
        }

        private void ParserForNordicModel_CompiledHex()
        {
            int i = 0;
            int totalSize = 0, totalConfigSize = 0;
            String[] Records = hexFilStr.Split(':');
            int upperAddress = 0, nextRecAddress = -1;
            bool IsCodeZone = true;
            CompiledCodeSection = "";
            CompiledConfigSection = "";
            // Record Parser
            while (i < Records.Length)
            {
                String currentRecord = Records[i++];
                char[] enter = new char[] { '\r', '\n' };
                currentRecord = currentRecord.Trim(enter);
                if (currentRecord == "")
                    continue;
                byte recordLen = Enumerable.Range(0, 2).Where(x => x % 2 == 0).Select(x => Convert.ToByte(currentRecord.Substring(x, 2), 16)).ToArray()[0];
                byte[] recordAddrByte = Enumerable.Range(2, 4).Where(x => x % 2 == 0).Select(x => Convert.ToByte(currentRecord.Substring(x, 2), 16)).ToArray();
                int recordAddr = recordAddrByte[0] * 256 + recordAddrByte[1];
                byte recordType = Enumerable.Range(6, 2).Where(x => x % 2 == 0).Select(x => Convert.ToByte(currentRecord.Substring(x, 2), 16)).ToArray()[0];
                byte[] recordData = Enumerable.Range(8, recordLen * 2).Where(x => x % 2 == 0).Select(x => Convert.ToByte(currentRecord.Substring(x, 2), 16)).ToArray();
                byte recordCrc = Enumerable.Range(recordLen * 2 + 8, 2).Where(x => x % 2 == 0).Select(x => Convert.ToByte(currentRecord.Substring(x, 2), 16)).ToArray()[0];
                String DataHex = ByteArrayToString(recordData);
                if (recordType == 4)
                {
                    upperAddress = recordData[0] * 256 + recordData[1];
                }
                else if (recordType == 0)
                {

                    int completeAddress = ((upperAddress << 16) | (recordAddr & 0xFFFF));

                    // Make sure data are aligned
                    if (recordData.Length % 4 != 0)
                        DataHex += new String('F', (4 - (recordData.Length % 4)) * 2);

                    if (nextRecAddress == -1)
                        nextRecAddress = completeAddress;

                    if (nextRecAddress != completeAddress)
                    {
                        IsCodeZone = false;
                    }
                    else if (IsCodeZone)
                        CompiledCodeSection += DataHex + "\r\n";
                    else if (!IsCodeZone)
                        CompiledConfigSection += DataHex + "\r\n";

                    if (IsCodeZone)
                        totalSize += (DataHex.Length / 2);
                    else if (!IsCodeZone)
                        totalConfigSize += (DataHex.Length / 2);

                    nextRecAddress = completeAddress + recordLen;
                }
                else if (recordType == 1)
                {
                    break;
                }

                // Set hex map


            }
            if (cmbprogram_section.SelectedIndex == 0)
                text_compiled.Text = CompiledCodeSection;
            else
                text_compiled.Text = CompiledConfigSection;

            label_compiled.Text = String.Format("Code Size : {0}\nConfiguration Size : {1}", totalSize, totalConfigSize);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtDiscrepancies.Text = "";
            bool Verification = true;
            int _NumOfDiscrepancies = 0;
            if (cmbprogram_section.SelectedIndex == 0)
            {
                //Compare byte by bytelabel_flash
                if (cmbChipModel.SelectedIndex == 1)
                {
                    byte[] _resFlash = StringToByteArray(FlashCodeSection);
                    byte[] _resCompiled = StringToByteArray(CompiledCodeSection);
                    if (_resFlash == null || _resCompiled == null || _resFlash.Length != _resCompiled.Length)
                    {
                        Verification = false;
                        MessageBox.Show(this, "Verification Failed!", "Verifivation", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return;
                    }
                    int i = 0;
                    while (i < _resFlash.Length)
                    {
                        if (_resFlash[i] != _resCompiled[i])
                        {
                            _NumOfDiscrepancies++;
                            Verification = false;
                            txtDiscrepancies.Text += i.ToString("X2") + "[" + _resFlash[i].ToString("X2") + ":" + _resCompiled[i].ToString("X2") + "] ";
                        }
                        i++;
                    }
                    if (Verification)
                        MessageBox.Show(this, "Verified Successfull!", "Verifivation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                    {
                        if (_NumOfDiscrepancies <= 6)
                            MessageBox.Show(this, "There is " + _NumOfDiscrepancies.ToString() + " bytes discrepancies, but it may be acceptable due to the linker .map file", "Verifivation Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                        {
                            MessageBox.Show(this, "There is " + _NumOfDiscrepancies.ToString() + " bytes discrepancies", "Verifivation failed", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        }
                    }
                }
                else
                {
                    if (FlashCodeSection == CompiledCodeSection)
                        MessageBox.Show(this, "Verified Successfull!", "Verifivation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show(this, "Verification Failed!", "Verifivation", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            else
            {
                if (FlashConfigSection == CompiledConfigSection)
                    MessageBox.Show(this, "Verified Successfull!", "Verifivation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show(this, "Verification Failed!", "Verifivation", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }
    }
    public struct HexRecord
    {
        public byte Lenght { get; set; }
        public ushort Offset { get; set; }
        public byte Tag { get; set; }
        public byte[] Data { get; set; }
        public byte Checksum { get; set; }
        public byte Status { get; set; }
        public HexRecord(byte[] record)
        {
            Lenght = 0x0;
            Offset = 0x0;
            Tag = 0x00;
            Data = null;
            Checksum = 0x0;
            Status = 0xFF;
            
            if (record == null || record.Length < 5)
                return;
            Lenght = record[0];
            if (record.Length != 5 + Lenght)
                return;

            Offset = record[1];
            Offset <<= 8;
            Offset += record[2];

            Tag = record[3];
            //[4, (4 + lenght -1)]
            if (Lenght != 0)
            {
                Data = new byte[Lenght];
                for (int i = 0; i < Lenght; i++)
                    Data[i] = record[4 + i];
            }
            Checksum = record[4 + Lenght];
            Status = 0x00;//Parsing was sucessful
        }
    }
}
