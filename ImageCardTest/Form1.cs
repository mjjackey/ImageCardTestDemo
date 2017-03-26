using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoTestDemo;

namespace ImageCardTest
{
    public partial class Form1 : Form
    {
        DxMediaFunction DxSdkFunction = new DxMediaFunction();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /////////////////create the ini file start/////////////////////////
            DxMediaFunction.IniFileName = System.AppDomain.CurrentDomain.BaseDirectory + "CsharpSetting.ini";

            ///// Check  the ini file doesn't already exist. If it doesn't exist, create
            if (!File.Exists(DxMediaFunction.IniFileName))
            {
                FileStream IniFile = System.IO.File.Create(DxMediaFunction.IniFileName);

            }
            /////////////////create the ini file end/////////////////////////

            //init sdk resource and get the video device number
            DxMediaFunction.NumCard = DxSdkFunction.InitDevice();
            if (DxMediaFunction.NumCard > 0)
            {
                /////
                for (int i = 0; i < DxMediaFunction.NumCard; i++)
                {

                    DxMediaFunction.DeviceInfo[i].dwPrvHandle = this.Videopanel.Handle;

                    // DxMediaFunction.DeviceInfo[i].dwVidStandard = 32;
                    DxMediaFunction.DeviceInfo[i].dwColorspace = 2;
                    //  DxMediaFunction.DeviceInfo[i].dwWidth = 720;
                    // DxMediaFunction.DeviceInfo[i].dwHeight = 576;
                    // DxMediaFunction.DeviceInfo[i].dwFrameRate = 25;


                    // ////////////////////////////read the ini save value start////////////////////
                    //  //read ini file    video stream size
                    DxMediaFunction.IniFileSaveValue[i].iniWidthValue = DxSdkFunction.IniReadValue("VideoPar", "VideoWidth" + Convert.ToString(i), 20, DxMediaFunction.IniFileName);
                    DxMediaFunction.IniFileSaveValue[i].iniHeightValue = DxSdkFunction.IniReadValue("VideoPar", "VideoHeight" + Convert.ToString(i), 20, DxMediaFunction.IniFileName);
                    DxMediaFunction.DeviceInfo[i].dwWidth = Convert.ToInt32(DxMediaFunction.IniFileSaveValue[i].iniWidthValue);
                    DxMediaFunction.DeviceInfo[i].dwHeight = Convert.ToInt32(DxMediaFunction.IniFileSaveValue[i].iniHeightValue);


                    //  //read ini file    video standard  pal/ntsc

                    DxMediaFunction.IniFileSaveValue[i].iniVideoStandard = DxSdkFunction.IniReadValue("VideoPar", "VideoStandard" + Convert.ToString(i), 20, DxMediaFunction.IniFileName);
                    if (DxMediaFunction.IniFileSaveValue[i].iniVideoStandard == "PAL")
                    {
                        //PAL= 0X20
                        DxMediaFunction.DeviceInfo[i].dwVidStandard = 32;
                        DxMediaFunction.DeviceInfo[i].dwFrameRate = 25;

                    }
                    else
                    {
                        //NTSC=0X01
                        DxMediaFunction.DeviceInfo[i].dwVidStandard = 1;
                        DxMediaFunction.DeviceInfo[i].dwFrameRate = 30;

                    }

                    ////////////////////////////read the ini save value  end///////////////////////////////

                    DxMediaFunction.DeviceInfo[i].dwCardHandle = DxSdkFunction.OpenDevice(i, ref  DxMediaFunction.DeviceInfo[i].dwOpenDevState);
                    DxMediaFunction.DeviceInfo[i].dwConnectState = DxSdkFunction.ConnectDev(DxMediaFunction.DeviceInfo[i].dwCardHandle,
                                                                                         DxMediaFunction.DeviceInfo[i].dwVidStandard,
                                                                                         DxMediaFunction.DeviceInfo[i].dwColorspace,
                                                                                         DxMediaFunction.DeviceInfo[i].dwWidth,
                                                                                          DxMediaFunction.DeviceInfo[i].dwHeight,
                                                                                         DxMediaFunction.DeviceInfo[i].dwFrameRate);



                    DxMediaFunction.DeviceInfo[i].dwPrviewState = DxSdkFunction.StartPrview(DxMediaFunction.DeviceInfo[i].dwCardHandle,
                                                                               DxMediaFunction.DeviceInfo[i].dwPrvHandle,
                                                                               DxMediaFunction.DeviceInfo[i].dwPrvRect,
                                                                               1);

                    // DxMediaApi.DXEnableDeinterlace(DxMediaFunction.DeviceInfo[i].dwCardHandle, 2);

                    DxMediaApi.DXEnableDenoise(DxMediaFunction.DeviceInfo[i].dwCardHandle, 65);

                    DxMediaApi.DXEnableSharpen(DxMediaFunction.DeviceInfo[i].dwCardHandle, 25);


                    if (DxMediaFunction.DeviceInfo[i].dwPrviewState != 0)
                    {
                        MessageBox.Show("Start preview False");
                    }
                }
            }
            else
            {
                MessageBox.Show("Init False, Please check the Card valid!");
            }
        }
    }
}
