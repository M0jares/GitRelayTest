using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

using System.Threading;
using System.Runtime.Remoting.Channels;
using System.Net;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace NumatoRelay
{
    //Channel List
    //1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G
    //(char)13 = New Line
    //Don`t know why "\r" is not functioning"

    //on writing a command for 'relay writeall 'hex letter should be lower
    //on writing a command for 'relay on/relay off' hex letter shoud be upper
    public class Class_NumatoRelay
    {
        public SerialPort serialComm;
        public bool isOpen { get; set; } = false; // Change this into true , Original false
        public string errorState { get; set; } = "";
       // private string _portName { get; set; } - Not Implemented
        private string _SN { get; set; }
        public Class_NumatoRelay(string SN)
        {
            _SN = SN;
        }
        public async Task connect()
        {
            try
            {
                foreach (var port in SerialPort.GetPortNames())
                {
                    try
                    {
                        serialComm = new SerialPort(port, 115200, Parity.None, 8, StopBits.One);
                        serialComm.Open();
                    }
                    catch (Exception)
                    {
                        serialComm.Close();
                        serialComm.Dispose();
                        continue;
                    }

                    string id = await getId();
                    if (id.Contains(_SN))
                    {
                        isOpen = true;
                        await reset();
                        return;
                    }
                    else
                    {
                        isOpen = false;
                        serialComm.Close();
                        serialComm.Dispose();
                        continue;
                    }
                }
                MessageBox.Show($"NUMATO WITH SN: {_SN} NOT DETECTED! \n APPLICATION WILL CLOSE!");
                System.Windows.Forms.Application.Exit();
            }
            catch (Exception)
            {


            }
            //serialComm = new SerialPort(_portName, 115200, Parity.None, 8, StopBits.One);
            //errorState = "";
            //try
            //{
            //    if (serialComm.IsOpen)
            //    {
            //        serialComm.Close();                  
            //    }
            //    serialComm.Open();               
            //    isOpen = true;
            //}
            //catch (Exception e)
            //{
            //    errorState = e.ToString();
            //    isOpen = false;                
            //}
        }
        public async Task connect(string comPort)
        {
            serialComm = new SerialPort(comPort, 115200, Parity.None, 8, StopBits.One);
            errorState = "";
            try
            {
                if (serialComm.IsOpen)
                {
                    serialComm.Close();
                }
                serialComm.Open();
                /// return isOpen = true;
            }
            catch (Exception e)
            {
                errorState = e.ToString();
                await Task.CompletedTask; // ADD
                // return isOpen = false;                
            }
        }

        public async Task<string> getId() // - Adding Try and Catch Exception
        {
            try
            {
                serialComm.WriteLine($"{(char)13}id get{(char)13}");
                //(char)13 = New Line
                //Don`t know why "\r" is not functioning"
                // Thread.Sleep(50); - Change
                await Task.CompletedTask;
                string a = serialComm.ReadExisting();
                return a;
            }
            catch
            {
                return "";
            }
        }

        public async Task<string> turnOnRelay(string channel) // - Adding Try and Catch Exception
        {
            try
            {
                serialComm.WriteLine($"{(char)13}relay on {channel}{(char)13}");
                //(char)13 = New Line
                //Don`t know why "\r" is not functioning"
                // Thread.Sleep(50); // Not this
                await Task.CompletedTask;
                return "Channel On";
            }
            catch (Exception ex)
            { 
                return $"Error turning on relay:{ex.Message}";  
            }
        }

        public async Task<string> turnOffRelay(string channel) // - Adding Try and Catch Exception
        {
            try
            {
                serialComm.WriteLine($"{(char)13}relay on {channel}{(char)13}");
                //(char)13 = New Line
                //Don`t know why "\r" is not functioning"
               // Thread.Sleep(50); // Not this
                await Task.CompletedTask;
                return "Channel Off";
            }
            catch (Exception ex)
            {
                return $"Error turning off relay:{ex.Message}";
            }
        }

        public async Task<bool> writeAll(string hex) // Try and Catch, <bool> 
        {
            try
            {
            serialComm.WriteLine($"{(char)13}relay writeall {hex}{(char)13}");
                //(char)13 = New Line
                //Don`t know why "\r" is not functioning"
                // Thread.Sleep(50);
                await Task.Run(() => Thread.Sleep(50));
                return true;
            }
            catch
            {
                return false;
            }
                        }

        public async Task reset() // I'm adding exception
        {
            try
            {
                serialComm.WriteLine($"{(char)13}reset{(char)13}");
                //(char)13 = New Line
                //Don`t know why "\r" is not functioning"
                await Task.Run(() => Thread.Sleep(50));// Thread.Sleep(50);
            }
            catch // (Exception ex)
            {
                await Task.CompletedTask;
                // MessageBox.Show($"Error resetting relay: {ex.Message}");
            }
        }

        public async Task closeConnection() // I'm adding exception
        {
            try
            {
                serialComm.Close();
                serialComm.Dispose();
                await Task.CompletedTask;

            }
            catch // (Exception ex)
            {
                await Task.CompletedTask;
                // MessageBox.Show($"Error closing connection: {ex.Message}");

            }
        }

        public void relayOn(int v)
        {
            throw new NotImplementedException();
        }

        public void relayOff(int v)
        {
            throw new NotImplementedException();
        }
    }
}
