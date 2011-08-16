using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CoreFoundation;
using CFManzana;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;

namespace iActivator
{    
    unsafe public partial class Form1 : Form
    {        
        public iDevice Interface;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {                
                Interface = new iDevice();                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                Application.Exit();
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (Interface.Device == null)
            {
                Interface = new iDevice();
            }
            if (Interface.Device == null)
            {
                lblStatus.Text = "No device connected";                
                return;
            }
            grpCache.Enabled = false;
            activate_btn.Enabled = false;
            deactivate_btn.Enabled = false;
            cache_btn.Enabled = false;
            int result = Interface.Deactivate();
            if (result == 0)
            {
                lblStatus.Text = "Device successfully deactivated";                
            }
            else
            {
                lblStatus.Text = "Device could not be deactivated";                
            }
            activate_btn.Enabled = true;
            deactivate_btn.Enabled = true;
            cache_btn.Enabled = true;
            grpCache.Enabled = true;
        }

        private string Albert_Apple_Stuff(string cachefile)
        {
            string Quotation = ((char)34).ToString();

            string request = "--DeviceActivation\r\nContent-Disposition: form-data; name=" + Quotation + "activation-info" + Quotation + "\r\n\r\n";
            if (cachefile == "" || cachefile == null)
            {
                IntPtr activationinfo = Interface.CopyDictionary("ActivationInfo");
                try
                {
                    request += GrabTheDictionary(new CFPropertyList(activationinfo).ToString()) + "\r\n--DeviceActivation--";
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Could not get ActivationInfo from iDevice";
                    return "";
                }
            }
            else
            {
                try
                {   
                    CFPropertyList cache = cachefile;
                    request += GrabTheDictionary(cache.ToString()) + "\r\n--DeviceActivation--";
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Could not read cache file";
                    return "";
                }
            }

            string headers = "POST /WebObjects/ALUnbrick.woa/wa/deviceActivation HTTP/1.1\r\n";            
            headers += "Accept-Encoding: gzip\r\n";
            headers += "Accept-Language: en-us, en;q=0.50\r\n";            
            headers += "Content-Type: multipart/form-data; boundary=DeviceActivation\r\n";
            headers += "Content-Length: "+(request).Length.ToString() + "\r\n";
            headers += "Host: albert.apple.com\r\n";
            headers += "Cache-Control: no-cache\r\n";          
            string ret = headers + "\r\n" + request;
            return ret;
        }        
        private string GrabTheDictionary(string p_list)
        {
            string ret="\0";                        
            int end = p_list.IndexOf(@"<plist version="+(char)(34)+"1.0" + (char)(34) + ">" ) + 21;
            ret = p_list.Remove(0, end);
            ret = ret.Remove(ret.IndexOf(@"</plist>"));
            return ret;
        }
        
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        public static bool IsConnected()
        {
            int Description;
            return InternetGetConnectedState(out Description, 0);
        } 

        private void activate_btn_Click(object sender, EventArgs e)
        {
            if (Interface.Device == null)
            {
                Interface = new iDevice();
            }
            if (Interface.Device == null)
            {
                lblStatus.Text = "No device connected";                
                return;
            }
            if (Interface.CopyValue("ActivationState") != "Unactivated")
            {
                lblStatus.Text= "Device already Activated!";
                return;
            }
            grpCache.Enabled = false;
            activate_btn.Enabled = false;
            deactivate_btn.Enabled = false;
            cache_btn.Enabled = false;
            string response=null;
            lblStatus.Text = "";
            bool useCache = false;
            if (activationdata.Checked == true)
            {
            opendialog:
                Open.DefaultExt = "iData";
                Open.Filter = "Activation Data|*.iData";
                Open.ShowDialog();
                if (Open.FileName == "")
                {
                    MessageBox.Show("Please select a location!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    goto opendialog;
                }
                useCache = true;
            }

#region Step 1: Get Wildcard ticket
            if (IsConnected() == false)
            {
                lblStatus.Text = "No Internet connection available";
                goto Cleanup;
            }
            lblStatus.Text = "Getting Wildcard ticket from Apple";
            if (useCache == true)
            {
                ThreadStart starter = delegate
                {
                    SslTcpClient.RunClient("albert.apple.com", Albert_Apple_Stuff(Open.FileName), ref response);
                };
                Thread albertThread = new Thread(starter);
                albertThread.Start();
                while (albertThread.IsAlive == true)
                {
                    Application.DoEvents();
                }
            }
            else
            {
                ThreadStart starter = delegate
                {
                    SslTcpClient.RunClient("albert.apple.com", Albert_Apple_Stuff(null), ref response);
                };
                Thread albertThread = new Thread(starter);
                albertThread.Start();
                while (albertThread.IsAlive == true)
                {
                    Application.DoEvents();
                }
            }
            if (response.Contains("ack-received") == true)
            {
                lblStatus.Text = "Invalid response received from Apple";
                goto Cleanup;
            }
            if (response == "" || response == null)
            {
                lblStatus.Text = "No response returned from Apple";
                goto Cleanup;
            }
#endregion            
#region Step 2: Grab necessary information from Wildcard ticket
            lblStatus.Text = "Grabbing information from the Wildcard ticket";            
            IntPtr[] wct = GrabTheData(response);
#endregion
#region Step 3: Send Wildcard ticket to the iDevice            
            string[] keys = { "AccountTokenCertificate", "AccountToken", "FairPlayKeyData", "DeviceCertificate", "AccountTokenSignature"};
            lblStatus.Text = ("Sending Wildcard ticket to device");            
            int status = Interface.Activate(new CFDictionary(keys, wct));
            lblStatus.Text = ("Wildcard ticket sent to device");            
#endregion
#region Step 4: Check if activation was successful            
            if (status != 0)
            {
                lblStatus.Text = "Device could not be activated!";                
            }
            else
            {
                lblStatus.Text = "Device successfully activated!";                
            }
#endregion            
            Cleanup:
            activate_btn.Enabled = true;
            deactivate_btn.Enabled = true;
            cache_btn.Enabled = true;
            grpCache.Enabled = true;
            Open.FileName = "";
        }
        private IntPtr[] GrabTheData(string Response)
        {
            IntPtr[] ret = new IntPtr[5];
            string temp;
            temp = Response.Remove(0, Response.IndexOf("<plist version") - 1);
            temp = temp.Remove((temp.IndexOf(@"</Protocol>")), temp.Length - ((temp.IndexOf(@"</Protocol>"))));
            string headers = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<!DOCTYPE plist PUBLIC \"-//Apple//DTD PLIST 1.0//EN\" \"http://www.apple.com/DTDs/PropertyList-1.0.dtd\">";
            temp = headers + "\r\n" + temp;
            if (File.Exists(Environment.CurrentDirectory + @"\wildcard.ticket") == true)
            {
                File.Delete(Environment.CurrentDirectory + @"\wildcard.ticket");
            }
            System.IO.File.AppendAllText(Environment.CurrentDirectory + @"\wildcard.ticket", temp);
            CFPropertyList plist = new CFPropertyList(Environment.CurrentDirectory + @"\wildcard.ticket");
            CFDictionary plist_to_dict = (IntPtr)plist;
            CFDictionary iphone_activation = (IntPtr)plist_to_dict.GetValue("iphone-activation");
            CFDictionary dict = (IntPtr)iphone_activation.GetValue("activation-record");
            ret[0] = dict.GetValue("AccountTokenCertificate");
            ret[1] = dict.GetValue("AccountToken");
            ret[2] = dict.GetValue("FairPlayKeyData");
            ret[3] = dict.GetValue("DeviceCertificate");
            ret[4] = dict.GetValue("AccountTokenSignature");
            if (File.Exists(Environment.CurrentDirectory + @"\wildcard.ticket") == true)
            {
                File.Delete(Environment.CurrentDirectory + @"\wildcard.ticket");
            }
            return ret;
        }       
        private void activationdata_CheckedChanged(object sender, EventArgs e)
        {
            if (activationdata.Checked == true && activationticket.Checked == true)
            {
                activationticket.Checked = false;
            }
        }

        private void activationticket_CheckedChanged(object sender, EventArgs e)
        {
            if (activationticket.Checked == true && activationdata.Checked == true)
            {
                activationdata.Checked = false;
            }
        }

        private void cache_btn_Click(object sender, EventArgs e)
        {
            if (Interface.Device == null)
            {
                Interface = new iDevice();
            }
            if (Interface.Device == null)
            {
                lblStatus.Text = "No device connected";
                return;
            }
            grpCache.Enabled = false;
            activate_btn.Enabled = false;
            deactivate_btn.Enabled = false;
            cache_btn.Enabled = false;
            MessageBox.Show("The process of creating a cache is simple: perform a legit activation, storing all the required data. That way, you can borrow (or, I guess, steal (don't do that, though)) a sim for the carrier your iPhone is locked to, and be able to reactivate without having to get that sim back.\nThis data is stored in a folder where you want it. It does not get sent to me (iSn0wra1n) or anyone else. Plus, we really have better things to do than look at your activation data.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //This really isn't needed for iPod Touches or Wi-Fi only iPads (and I don't know if 3G iPad users need this, but be safe and do it).\n\nPress any key to continue or CONTROL-C to abort...\n\n");
            if (activationticket.Checked == true && Interface.CopyValue("ActivationState") == "Unactivated")
            {
                #region Get Wildcard Ticket
                string response = null;
                if (IsConnected() == false)
                {
                    lblStatus.Text = "No Internet connection available";
                    return;
                }
                lblStatus.Text = "Getting Wildcard ticket from Apple";
                ThreadStart starter = delegate
                {
                    SslTcpClient.RunClient("albert.apple.com", Albert_Apple_Stuff(null), ref response);
                };
                Thread albertThread = new Thread(starter);
                albertThread.Start();
                while (albertThread.IsAlive == true)
                {
                    Application.DoEvents();
                }
                if (response == "")
                {
                    lblStatus.Text = "No response returned from Apple";
                    return;
                }
                #endregion
                #region Grab necessary information
                lblStatus.Text = "Grabbing information from the Wildcard ticket";
                string[] keys = { "AccountTokenCertificate", "AccountToken", "FairPlayKeyData", "DeviceCertificate", "AccountTokenSignature" };
                IntPtr[] values = GrabTheData(response);                
                #endregion
            activationticket:
                Save.Filter = "Activation Ticket|*.iTicket";
                Save.DefaultExt = "iTicket";
                Save.ShowDialog();
                if (Save.FileName == "")
                {
                    MessageBox.Show("Please select a location!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    goto activationticket;
                }
                File.WriteAllText(Save.FileName, new CFDictionary(keys,values).ToString());
                lblStatus.Text = "Activation Ticket saved";
                Save.FileName = "";
            }
            else if (activationdata.Checked == true)
            {
            activationdata:
                try
                {
                    Save.Filter = "Activation Data|*.iData";
                    Save.DefaultExt = "iData";
                    Save.ShowDialog();
                    if (Save.FileName == "")
                    {
                        MessageBox.Show("Please select a location!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        goto activationdata;
                    }
                    File.WriteAllText(Save.FileName, new CFDictionary(Interface.CopyDictionary("ActivationInfo")).ToString());
                    lblStatus.Text = "Activation Data Saved";
                    Save.FileName = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
            }
            else if (activationticket.Checked == true && Interface.CopyValue("ActivationState") != "Unactivated")            
            {
                lblStatus.Text = "Deactivate device to cache Activation Ticket";                
            }
            else
            {
                lblStatus.Text = "No cache option selected";
            }
            activate_btn.Enabled = true;
            deactivate_btn.Enabled = true;
            cache_btn.Enabled = true;
            grpCache.Enabled = true;
        }
    }
}