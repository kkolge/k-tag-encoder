using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phychips.Driver
{
    public class HidPort : SLABCP2110
    {
        public static string[] GetPortNames()
        {
            string[] listHidPort = new string[0];

            uint numDevices = 0;
            StringBuilder deviceString = new StringBuilder(SLABHIDTOUART.HID_UART_DEVICE_STRLEN);

            if (SLABHIDTOUART.HidUart_GetNumDevices(ref numDevices, 0, 0) == SLABHIDTOUART.HID_UART_SUCCESS)
            {
                listHidPort = new String[numDevices];

                for (uint i = 0; i < numDevices; i++)
                {
                    if (SLABHIDTOUART.HidUart_GetString(i, 0, 0, deviceString, SLABHIDTOUART.HID_UART_GET_SERIAL_STR)
                        == SLABHIDTOUART.HID_UART_SUCCESS)
                    {
                        listHidPort[i] = deviceString.ToString();
                        //System.Console.WriteLine("SLABHIDTOUART[{0}] = {1}", i, mDeviceList[i]);
                    }
                }
            }

            return listHidPort;
        }
    }
}
