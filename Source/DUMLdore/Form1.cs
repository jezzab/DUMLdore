// DUMLdore - jezzab 2017 - http://www.github.com/jezzab/DUMLdore/
// This softwre is used to flash and backup firmware files from a drone, remote controller or goggles
// It requires OpenSSL and the WinSCP .NET Assembly 

using WinSCP;
using System;
using System.Net;
using System.IO;
using System.IO.Ports;
using Microsoft.Win32;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace DUMLdore
{

    public partial class Form1 : Form
    {
        // fixed arrays. 0xFF used for padding. Blockcopy into array the correct values later
        // could be made more dynamic and one array only by choosing dst and src ID DUML etc. TODO
        public byte[] FWarray;
        byte[] packet1_ac = { 0x55, 0x16, 0x04, 0xFC, 0x2A, 0x28, 0x65, 0x57, 0x40, 0x00, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x27, 0xD3 }; //Enter upgrade mode (delete old file if exists)
        byte[] packet2_ac = { 0x55, 0x0E, 0x04, 0x66, 0x2A, 0x28, 0x68, 0x57, 0x40, 0x00, 0x0C, 0x00, 0x88, 0x20 }; //Enable Reporting
        byte[] packet3_ac = { 0x55, 0x1A, 0x04, 0xB1, 0x2A, 0x28, 0x6B, 0x57, 0x40, 0x00, 0x08, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x04, 0xFF, 0xFF }; //Payload 
        byte[] packet4_ac = { 0x55, 0x1E, 0x04, 0x8A, 0x2A, 0x28, 0xF6, 0x57, 0x40, 0x00, 0x0A, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }; //MD5

        byte[] packet1_rc = { 0x55, 0x16, 0x04, 0xFC, 0x2A, 0x2D, 0xE7, 0x27, 0x40, 0x00, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x9F, 0x44 }; //Enter upgrade mode (delete old file if exists)
        byte[] packet2_rc = { 0x55, 0x0E, 0x04, 0x66, 0x2A, 0x2D, 0xEA, 0x27, 0x40, 0x00, 0x0C, 0x00, 0x2C, 0xC8 }; //Enable Reporting
        byte[] packet3_rc = { 0x55, 0x1A, 0x04, 0xB1, 0x2A, 0x2D, 0xEC, 0x27, 0x40, 0x00, 0x08, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x04, 0xFF, 0xFF }; //Payload 
        byte[] packet4_rc = { 0x55, 0x1E, 0x04, 0x8A, 0x2A, 0x2D, 0x02, 0x28, 0x40, 0x00, 0x0A, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }; //MD5

        byte[] packet1_gog = { 0x55, 0x16, 0x04, 0xFC, 0x2A, 0x3C, 0xF7, 0x35, 0x40, 0x00, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0C, 0x29 }; //Enter upgrade mode (delete old file if exists)
        byte[] packet2_gog = { 0x55, 0x0E, 0x04, 0x66, 0x2A, 0x3C, 0xFA, 0x35, 0x40, 0x00, 0x0C, 0x00, 0x48, 0x02 }; //Enable Reporting
        byte[] packet3_gog = { 0x55, 0x1A, 0x04, 0xB1, 0x2A, 0x3C, 0xFD, 0x35, 0x40, 0x00, 0x08, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x04, 0xFF, 0xFF }; //Payload 
        byte[] packet4_gog = { 0x55, 0x1E, 0x04, 0x8A, 0x2A, 0x3C, 0x5B, 0x36, 0x40, 0x00, 0x0A, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }; //MD5

        string filename;
        string dji_comport;
        string assistant2 = "DJIBrowser";
        public SerialPort port = new SerialPort("COM1", 115200, Parity.None, 8, StopBits.One);

        int fireworks = 0;
        int downloading = 0;

        //AC - 551A04B12A286B5740000800YYYYYYYY0000000000000204XXXX
        //RC - 551A04B12A2DEC2740000800YYYYYYYY0000000000000204XXXX
        // YYYYYYYY - file size in little endian
        // XXXX - CRC16 - Poly 1021 - Init 496C Ref in/out - XOR 0000 - little endian   

        public Form1()
        {
            InitializeComponent();
            //Search for VID/DEV of device and get the COM port number
            List<string> names = ComPortNames("2CA3", "001F");
            if (names.Count > 0)
            {
                foreach (String s in SerialPort.GetPortNames())
                {
                    if (names.Contains(s))
                        dji_comport = s;
                }
            }
            //see if Assistant2 is hogging the COM port
            if (dji_comport == null)
            {
                if (Process.GetProcessesByName(assistant2).Length != 0)
                    MessageBox.Show("Assistant2 is running\r\nPlease close it and restart this application");
                toolStripStatusLabel1.Text = "Device not found";
            }
            else
            {
                toolStripStatusLabel1.Text = "Device found on " + dji_comport;
                btnBackupFW.Enabled = true;
                btnLoadFW.Enabled = true;
            }

        }
        public void btnLoadFW_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.FileName = "";
            this.openFileDialog1.Filter = "Firmware Files|*dji_system.bin";
            // Process input if the user clicked OK.
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Open the selected file to read.
                System.IO.Stream fileStream = openFileDialog1.OpenFile();
                filename = openFileDialog1.FileName;
                FWarray = File.ReadAllBytes(filename);

                //Check the file isnt too small
                if(FWarray.Length < 0x200)
                    MessageBox.Show("Invalid Firmware File. File too small");
                else
                {
                    //Check if it has the IM*H header
                    if ((FWarray[0x200] != 0x49) && (FWarray[0x201] != 0x4D) && (FWarray[0x202] != 0x2A) && (FWarray[0x203] != 0x48))
                    {
                        //its not a fw file with IM*H header. Check if it is a fireworks.tar. Check for "Burn" header
                        if ((FWarray[0x00] != 0x42) && (FWarray[0x01] != 0x75) && (FWarray[0x02] != 0x72) && (FWarray[0x03] != 0x6E))
                            MessageBox.Show("Invalid Firmware File");
                        else
                        {
                            fireworks = 1;
                            btnFlashFW.Enabled = true;
                        }
                    }
                    else
                        btnFlashFW.Enabled = true;
                }
            }
        }
        public void btnFlashFW_Click(object sender, EventArgs e)
        {
            serialPort1.BaudRate = 115200;
            serialPort1.PortName = dji_comport;

            // Setup session options
            SessionOptions sessionOptions = new SessionOptions
            {
                Protocol = Protocol.Ftp,
                HostName = "192.168.42.2",
                UserName = "guest",
                Password = "password",
            };

            MessageBox.Show("The firmware file will be sent to the device\r\nMake sure you have atleast 50% battery remaining\r\nThis will take up to 30 seconds\r\nPress OK and wait for the confirmation dialog");
            if (rdoAircraft.Checked == true)
            {
                try
                {
                    serialPort1.Open();

                    // send init DUML packets
                    serialPort1.Write(packet1_ac, 0, packet1_ac.Length);
                    serialPort1.Write(packet2_ac, 0, packet2_ac.Length);

                    toolStripStatusLabel1.Text = "Starting serial comms";
                    uint filesize = (uint)FWarray.Length;
                    byte[] filesize_array = BitConverter.GetBytes(filesize);

                    //copy bytes for filesize
                    Buffer.BlockCopy(filesize_array, 0, packet3_ac, 12, filesize_array.Length);

                    //calc crc and insert
                    uint crc = CalcCRC(packet3_ac, packet3_ac.Length - 2);
                    byte[] crc_array = BitConverter.GetBytes(crc);
                    Buffer.BlockCopy(crc_array, 0, packet3_ac, 24, 2);

                    //MD5 stuff
                    MD5 md5Hash = MD5.Create();
                    byte[] hash = md5Hash.ComputeHash(FWarray);
                    Buffer.BlockCopy(hash, 0, packet4_ac, 12, hash.Length);
                    crc = CalcCRC(packet4_ac, packet4_ac.Length - 2);
                    crc_array = BitConverter.GetBytes(crc);
                    Buffer.BlockCopy(crc_array, 0, packet4_ac, 28, 2);
                    toolStripStatusLabel1.Text = "Uploading Firmware. Please wait...";

                    //mkdir .bin if fireworks
                    if (fireworks == 1)
                        MakeFtpDir();
                    if (UploadFirmware() == 0)
                    {
                        // bad way to do it but some devices require some more time to write/update. Checking for existance doesnt always work
                        System.Threading.Thread.Sleep(5000);

                        //send out start DUML packets
                        serialPort1.Write(packet3_ac, 0, packet3_ac.Length);
                        serialPort1.Write(packet4_ac, 0, packet4_ac.Length);
                        toolStripStatusLabel1.Text = "Firmware Uploaded";
                        if (fireworks == 1)
                            MessageBox.Show("Red Herring has been applied!\r\nYou must reboot to apply the root patch");
                        else
                            MessageBox.Show("Upgrade has begun. \r\nPlease allow up to 15 mins for installation\r\nWait for the beeps and watch the LEDs for status\r\nIt will reboot when complete\r\nYou can open Assistant2 to view current progress");
                        serialPort1.Close();
                    }
                    else
                    {
                        MessageBox.Show("Timed out attempting to contact the FTP server on the device\r\nYou can test for connectivity by going to ftp://192.168.42.2 with a browser");
                        this.toolStripStatusLabel1.Text = "FTP access failed";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to contact device, the serial port is closed or disconnected. Please plug in the device, close Assistant2 if running and restart the device and software");
                }
            }
            if (rdoRemote.Checked == true)
            {
                try
                {
                    serialPort1.Open();

                    // send init DUML packets
                    serialPort1.Write(packet1_rc, 0, packet1_rc.Length);
                    serialPort1.Write(packet2_rc, 0, packet2_rc.Length);

                    toolStripStatusLabel1.Text = "Starting serial comms";
                    uint filesize = (uint)FWarray.Length;
                    byte[] filesize_array = BitConverter.GetBytes(filesize);

                    //copy bytes for filesize
                    Buffer.BlockCopy(filesize_array, 0, packet3_rc, 12, filesize_array.Length);

                    //calc crc and insert
                    uint crc = CalcCRC(packet3_rc, packet3_rc.Length - 2);
                    byte[] crc_array = BitConverter.GetBytes(crc);
                    Buffer.BlockCopy(crc_array, 0, packet3_rc, 24, 2);

                    //MD5 stuff
                    MD5 md5Hash = MD5.Create();
                    byte[] hash = md5Hash.ComputeHash(FWarray);
                    Buffer.BlockCopy(hash, 0, packet4_rc, 12, hash.Length);
                    crc = CalcCRC(packet4_rc, packet4_rc.Length - 2);
                    crc_array = BitConverter.GetBytes(crc);
                    Buffer.BlockCopy(crc_array, 0, packet4_rc, 28, 2);
                    toolStripStatusLabel1.Text = "Uploading Firmware. Please wait";

                    //mkdir .bin if fireworks
                    if (fireworks == 1)
                        MakeFtpDir();

                    if (UploadFirmware() == 0)
                    {
                        // bad way to do it but some devices require some more time to write/update. Checking for existance doesnt always work
                        System.Threading.Thread.Sleep(5000);

                        //send out start DUML packets
                        toolStripStatusLabel1.Text = "Please wait...";
                        serialPort1.Write(packet3_rc, 0, packet3_rc.Length);
                        serialPort1.Write(packet4_rc, 0, packet4_rc.Length);
                        toolStripStatusLabel1.Text = "Firmware Uploaded";
                        if (fireworks == 1)
                            MessageBox.Show("Red Herring has been applied!\r\nYou must reboot to apply the root patch");
                        else
                            MessageBox.Show("Upgrade has begun. \r\nPlease allow up to 10 mins for installation\r\nWatch the screen for status\r\n");
                        serialPort1.Close();
                    }
                    else
                    {
                        MessageBox.Show("Timed out attempting to contact the FTP server on the device\r\nYou can test for connectivity by going to ftp://192.168.42.2 with a browser");
                        this.toolStripStatusLabel1.Text = "FTP access failed";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to contact device, the serial port is closed or disconnected. Please plug in the device, close Assistant2 if running and restart the device and software");
                }
            }
            if (rdoGoggles.Checked == true)
            {
                try
                {
                    serialPort1.Open();

                    // send init DUML packets
                    serialPort1.Write(packet1_gog, 0, packet1_gog.Length);
                    serialPort1.Write(packet2_gog, 0, packet2_gog.Length);

                    toolStripStatusLabel1.Text = "Starting serial comms";
                    uint filesize = (uint)FWarray.Length;
                    //filesize = SwapBytes(filesize);
                    byte[] filesize_array = BitConverter.GetBytes(filesize);

                    //copy bytes for filesize
                    Buffer.BlockCopy(filesize_array, 0, packet3_gog, 12, filesize_array.Length);

                    //calc crc and insert
                    uint crc = CalcCRC(packet3_gog, packet3_gog.Length - 2);
                    byte[] crc_array = BitConverter.GetBytes(crc);
                    Buffer.BlockCopy(crc_array, 0, packet3_gog, 24, 2);

                    //MD5 stuff
                    MD5 md5Hash = MD5.Create();
                    byte[] hash = md5Hash.ComputeHash(FWarray);
                    Buffer.BlockCopy(hash, 0, packet4_gog, 12, hash.Length);
                    crc = CalcCRC(packet4_gog, packet4_gog.Length - 2);
                    crc_array = BitConverter.GetBytes(crc);
                    Buffer.BlockCopy(crc_array, 0, packet4_gog, 28, 2);
                    toolStripStatusLabel1.Text = "Uploading Firmware. Please wait";

                    //mkdir .bin if fireworks
                    if (fireworks == 1)
                        MakeFtpDir();

                    if (UploadFirmware() == 0)
                    {
                        // bad way to do it but some devices require some more time to write/update. Checking for existance doesnt always work
                        System.Threading.Thread.Sleep(5000);
                        //send out start DUML packets
                        serialPort1.Write(packet3_gog, 0, packet3_gog.Length);
                        serialPort1.Write(packet4_gog, 0, packet4_gog.Length);
                        toolStripStatusLabel1.Text = "Firmware Uploaded";
                        if (fireworks == 1)
                            MessageBox.Show("Red Herring has been applied!\r\nYou must reboot to apply the root patch");
                        else
                            MessageBox.Show("Upgrade has begun. \r\nPlease allow up to 10 mins for installation\r\n");
                        serialPort1.Close();
                    }
                    else
                    {
                        MessageBox.Show("Timed out attempting to contact the FTP server on the device\r\nYou can test for connectivity by going to ftp://192.168.42.2 with a browser");
                        this.toolStripStatusLabel1.Text = "FTP access failed";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to contact device, the serial port is closed or disconnected. Please plug in the device, close Assistant2 if running and restart the device and software");
                }
            }
        }
        public void btnBackupFW_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Connecting to device....";
            if (GetSigFiles() == 0)
            {
                this.toolStripStatusLabel1.Text = "";

                //parse the filenames and build a array of names
                string[] fileArray = Directory.GetFiles(@".\backup\", "*.sig");
                this.toolStripStatusLabel1.Text = "";
                this.toolStripStatusLabel1.Text = "Decrypting";
                Directory.CreateDirectory(".\\decrypt");
                this.progressBar1.Value = 0;

                //use openSSL to AES decrypt the files into the decrypt sub dir. Run with CMD windows but hidden. Yuk!
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                for (int i = 0; i != fileArray.Length; i++)
                {
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    startInfo.FileName = "cmd.exe";
                    startInfo.WorkingDirectory = @".\";
                    startInfo.Arguments = "/C openssl.exe enc -d -nosalt -in " + fileArray[i].ToString() + " -aes-128-cbc -K 746869732d6165732d6b657900000000 -iv 00000000000000000000000000000000 > .\\decrypt\\" + Path.GetFileName(fileArray[i]);
                    process.StartInfo = startInfo;
                    process.Start();
                    process.WaitForExit();
                }
                //fix the first 16 bytes of the decrypted files and overwrite
                this.toolStripStatusLabel1.Text = "Fixing headers";
                this.progressBar1.Value = 0;
                for (int j = 0; j != fileArray.Length; j++)
                {
                    byte[] array = File.ReadAllBytes(".\\decrypt\\" + Path.GetFileName(fileArray[j]));
                    // Descramble first 16 bytes
                    for (int i = 0x00; i < 0x0A; i++)
                    {
                        array[i] ^= (byte)(0x30 + i);
                    }
                    for (int i = 0x0A; i < 0x10; i++)
                    {
                        array[i] ^= (byte)(0x57 + i);
                    }
                    //Write it back out
                    File.WriteAllBytes(".\\decrypt\\" + Path.GetFileName(fileArray[j]), array);
                    this.progressBar1.Increment(5);
                }
                toolStripStatusLabel1.Text = "Repackaging";

                //naughty sh!t with windows CMD - hide the CMD windows. This is evil
                //windows tar will NOT allow you to use * in another dir. Using . it will capture the CWD file
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.Arguments = "/C copy .\\decrypt\\*.sig .\\";
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                toolStripStatusLabel1.Text = "Repackaging";
                this.progressBar1.Increment(5);

                //tar but remove owner/group
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.Arguments = "/C tar.exe --numeric-owner -cf dji_system.bin *.sig";
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                toolStripStatusLabel1.Text = "Cleaning up";
                this.progressBar1.Increment(5);

                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.Arguments = "/C del *.sig";
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                this.progressBar1.Increment(5);

                Directory.Delete(".\\decrypt\\", true);
                Directory.Delete(".\\backup\\", true);
                this.progressBar1.Value = 100;
                toolStripStatusLabel1.Text = "Backup complete";
            }
            else
            {
                MessageBox.Show("Timed out attempting to contact the FTP server on the device\r\nYou can test for connectivity by going to ftp://192.168.42.2 with a browser");
                this.toolStripStatusLabel1.Text = "FTP access failed";
            }
        }
        
        // Compile an array of COM port names associated with given VID and PID
        List<string> ComPortNames(String VID, String PID)
        {
            String pattern = String.Format("^VID_{0}.PID_{1}", VID, PID);
            Regex _rx = new Regex(pattern, RegexOptions.IgnoreCase);
            List<string> comports = new List<string>();
            RegistryKey rk1 = Registry.LocalMachine;
            RegistryKey rk2 = rk1.OpenSubKey("SYSTEM\\CurrentControlSet\\Enum");
            foreach (String s3 in rk2.GetSubKeyNames())
            {
                RegistryKey rk3 = rk2.OpenSubKey(s3);
                foreach (String s in rk3.GetSubKeyNames())
                {
                    if (_rx.Match(s).Success)
                    {
                        RegistryKey rk4 = rk3.OpenSubKey(s);
                        foreach (String s2 in rk4.GetSubKeyNames())
                        {
                            RegistryKey rk5 = rk4.OpenSubKey(s2);
                            RegistryKey rk6 = rk5.OpenSubKey("Device Parameters");
                            comports.Add((string)rk6.GetValue("PortName"));
                        }
                    }
                }
            }
            return comports;
        }

        // show FTP file transfer progress
        public void SessionFileTransferProgress(object sender, FileTransferProgressEventArgs e)
        {
            int percent = (int)(e.FileProgress * 100);
            // New line for every new file
            if ((_lastFileName != null) && (_lastFileName != e.FileName))
            {
                Debug.WriteLine("");
            }
            // Print transfer progress
            Debug.WriteLine("\r{0} ({1:P0})", e.FileName, e.FileProgress);
            this.progressBar1.Value = percent;
            // Manage the label
            if (downloading == 1)
                this.toolStripStatusLabel1.Text = "Downloading: " + percent.ToString() + " %";
            else
                this.toolStripStatusLabel1.Text = "Uploading: " + percent.ToString() + " %";
            this.toolStripStatusLabel1.Text = e.FileName.ToString();
            // Remember a name of the last file reported
            _lastFileName = e.FileName;
        }
        private static string _lastFileName;
        public int GetSigFiles()
        {
            //download all of the fw .sig files
            try
            {
                // Setup session options
                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Ftp,
                    HostName = "192.168.42.2",
                    UserName = "guest",
                    Password = "password",
                };
                using (Session session = new Session())
                {
                    session.FileTransferProgress += SessionFileTransferProgress;
                    // Connect
                    session.Open(sessionOptions);
                    downloading = 1;
                        TransferOperationResult transferResult;
                        // Download files
                        transferResult = session.GetFiles("/upgrade/upgrade/backup/*.sig", @".\backup\*");
                    // Throw on any error
                    transferResult.Check();
                    session.Close();
                    downloading = 0;
                }
                return 0;
            }
            catch (Exception e)
            {
                return 1;
            }
        }
        public int UploadFirmware()
        {
            //upload the dji_sysetm.bin file
            try
            {
                // Setup session options
                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Ftp,
                    HostName = "192.168.42.2",
                    UserName = "guest",
                    Password = "password",
                };
                using (Session session = new Session())
                {
                    session.FileTransferProgress += SessionFileTransferProgress;
                    // Connect
                    session.Open(sessionOptions);
                    downloading = 1;
                    TransferOperationResult transferResult;
                    // Upload file
                    transferResult = session.PutFiles(filename, "/upgrade/dji_system.bin");
                    if (session.FileExists("/upgrade/dji_system.bin")) { }
                    else
                        System.Threading.Thread.Sleep(5000);
                    // Throw on any error
                    transferResult.Check();
                    session.Close();
                    downloading = 0;
                }
                return 0;
            }
            catch (Exception e)
            {
                //MessageBox.Show("Error: " + e.ToString());
                return 1;
            }
        }

        //only used for fireworks.tar/root
        public static void MakeFtpDir()
        {
            bool IsExists = true;
            try
            {
                WebRequest request = WebRequest.Create("ftp://192.168.42.2/upgrade/.bin");
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                request.Credentials = new NetworkCredential("guest", "password");
                WebResponse resp = (FtpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                IsExists = false;
            }
            IsExists = true;
        }
        public uint CalcCRC(byte[] packet, int plength)
        {
            //borrowed from python/pyDUML Thx
            uint[] crc = { 0x0000, 0x1189, 0x2312, 0x329b, 0x4624, 0x57ad, 0x6536, 0x74bf,
            0x8c48, 0x9dc1, 0xaf5a, 0xbed3, 0xca6c, 0xdbe5, 0xe97e, 0xf8f7,
            0x1081, 0x0108, 0x3393, 0x221a, 0x56a5, 0x472c, 0x75b7, 0x643e,
            0x9cc9, 0x8d40, 0xbfdb, 0xae52, 0xdaed, 0xcb64, 0xf9ff, 0xe876,
            0x2102, 0x308b, 0x0210, 0x1399, 0x6726, 0x76af, 0x4434, 0x55bd,
            0xad4a, 0xbcc3, 0x8e58, 0x9fd1, 0xeb6e, 0xfae7, 0xc87c, 0xd9f5,
            0x3183, 0x200a, 0x1291, 0x0318, 0x77a7, 0x662e, 0x54b5, 0x453c,
            0xbdcb, 0xac42, 0x9ed9, 0x8f50, 0xfbef, 0xea66, 0xd8fd, 0xc974,
            0x4204, 0x538d, 0x6116, 0x709f, 0x0420, 0x15a9, 0x2732, 0x36bb,
            0xce4c, 0xdfc5, 0xed5e, 0xfcd7, 0x8868, 0x99e1, 0xab7a, 0xbaf3,
            0x5285, 0x430c, 0x7197, 0x601e, 0x14a1, 0x0528, 0x37b3, 0x263a,
            0xdecd, 0xcf44, 0xfddf, 0xec56, 0x98e9, 0x8960, 0xbbfb, 0xaa72,
            0x6306, 0x728f, 0x4014, 0x519d, 0x2522, 0x34ab, 0x0630, 0x17b9,
            0xef4e, 0xfec7, 0xcc5c, 0xddd5, 0xa96a, 0xb8e3, 0x8a78, 0x9bf1,
            0x7387, 0x620e, 0x5095, 0x411c, 0x35a3, 0x242a, 0x16b1, 0x0738,
            0xffcf, 0xee46, 0xdcdd, 0xcd54, 0xb9eb, 0xa862, 0x9af9, 0x8b70,
            0x8408, 0x9581, 0xa71a, 0xb693, 0xc22c, 0xd3a5, 0xe13e, 0xf0b7,
            0x0840, 0x19c9, 0x2b52, 0x3adb, 0x4e64, 0x5fed, 0x6d76, 0x7cff,
            0x9489, 0x8500, 0xb79b, 0xa612, 0xd2ad, 0xc324, 0xf1bf, 0xe036,
            0x18c1, 0x0948, 0x3bd3, 0x2a5a, 0x5ee5, 0x4f6c, 0x7df7, 0x6c7e,
            0xa50a, 0xb483, 0x8618, 0x9791, 0xe32e, 0xf2a7, 0xc03c, 0xd1b5,
            0x2942, 0x38cb, 0x0a50, 0x1bd9, 0x6f66, 0x7eef, 0x4c74, 0x5dfd,
            0xb58b, 0xa402, 0x9699, 0x8710, 0xf3af, 0xe226, 0xd0bd, 0xc134,
            0x39c3, 0x284a, 0x1ad1, 0x0b58, 0x7fe7, 0x6e6e, 0x5cf5, 0x4d7c,
            0xc60c, 0xd785, 0xe51e, 0xf497, 0x8028, 0x91a1, 0xa33a, 0xb2b3,
            0x4a44, 0x5bcd, 0x6956, 0x78df, 0x0c60, 0x1de9, 0x2f72, 0x3efb,
            0xd68d, 0xc704, 0xf59f, 0xe416, 0x90a9, 0x8120, 0xb3bb, 0xa232,
            0x5ac5, 0x4b4c, 0x79d7, 0x685e, 0x1ce1, 0x0d68, 0x3ff3, 0x2e7a,
            0xe70e, 0xf687, 0xc41c, 0xd595, 0xa12a, 0xb0a3, 0x8238, 0x93b1,
            0x6b46, 0x7acf, 0x4854, 0x59dd, 0x2d62, 0x3ceb, 0x0e70, 0x1ff9,
            0xf78f, 0xe606, 0xd49d, 0xc514, 0xb1ab, 0xa022, 0x92b9, 0x8330,
            0x7bc7, 0x6a4e, 0x58d5, 0x495c, 0x3de3, 0x2c6a, 0x1ef1, 0x0f78 };

            uint v = 0x3692;
            uint vv = 0;
            for (int i = 0; i != plength; i++)
            {
                vv = v >> 8;
                v = vv ^ crc[((packet[i] ^ v) & 0xFF)];
            }
            return v;
        }
    }
}
    