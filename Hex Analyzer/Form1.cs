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

        private void btn_load_flash_Click(object sender, EventArgs e)
        {
            Stream hexFileFlash;
            String hexFileFlashStr;
            int i = 0;
            int totalSize=0, totalConfigSize = 0;
            byte[] hexFileFlashBytes;
            FileLoader.Filter = "Intel Hex File|*.hex";
            FileLoader.Title = "Load Flash Hex File";
            FileLoader.ShowDialog();
            txt_flash_file.Text = FileLoader.FileName;
            hexFileFlash = FileLoader.OpenFile();
            hexFileFlashBytes = new byte[hexFileFlash.Length];
            hexFileFlash.Read(hexFileFlashBytes, 0, (int)hexFileFlash.Length);
            hexFileFlash.Close();

            hexFileFlashStr = Encoding.ASCII.GetString(hexFileFlashBytes);

            i = 0;
            String[] Records = hexFileFlashStr.Split(':');
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
                    else if(IsCodeZone)
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
            if(program_section.SelectedIndex == 0)
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
            program_section.SelectedIndex = 0;
        }

        private void program_section_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (program_section.SelectedIndex == 0)
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
            String hexFileCompiledStr;
            int i = 0;
            int totalSize = 0, totalConfigSize = 0;
            byte[] hexFileCompiledBytes;
            FileLoader.Filter = "Intel Hex File|*.hex";
            FileLoader.Title = "Load Compiled Hex File";
            FileLoader.ShowDialog();
            txt_compiled_file.Text = FileLoader.FileName;
            hexFileCompiled = FileLoader.OpenFile();
            hexFileCompiledBytes = new byte[hexFileCompiled.Length];
            hexFileCompiled.Read(hexFileCompiledBytes, 0, (int)hexFileCompiled.Length);
            hexFileCompiled.Close();

            hexFileCompiledStr = Encoding.ASCII.GetString(hexFileCompiledBytes);

            i = 0;
            String[] Records = hexFileCompiledStr.Split(':');
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
                        totalSize += (DataHex.Length/2);
                    else if (!IsCodeZone)
                        totalConfigSize += (DataHex.Length/2);

                    nextRecAddress = completeAddress + recordLen;
                }
                else if (recordType == 1)
                {
                    break;
                }

                // Set hex map


            }
            if (program_section.SelectedIndex == 0)
                text_compiled.Text = CompiledCodeSection;
            else
                text_compiled.Text = CompiledConfigSection;

            label_compiled.Text = String.Format("Code Size : {0}\nConfiguration Size : {1}", totalSize, totalConfigSize);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (program_section.SelectedIndex == 0)
            {
                if (FlashCodeSection == CompiledCodeSection)
                    MessageBox.Show(this, "Verified Successfull!", "Verifivation",MessageBoxButtons.OK,MessageBoxIcon.Information);
                else
                    MessageBox.Show(this, "Verification Failed!", "Verifivation", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
}
