using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using WindowsInput;
using H.Hooks;


namespace formsBinder
{
    public partial class Form1 : Form
    {
        private InputSimulator simulator;

        public Form1()
        {
            InitializeComponent();

            // Инициализируем объект для имитации ввода
            simulator = new InputSimulator();   
            using (var hook = new H.Hooks.LowLevelKeyboardHook())
            {              
                hook.Up += (s, e) =>
                {
                    var pressedKeys = e.Keys;
                    if (pressedKeys.IsCtrl && pressedKeys.IsRightAlt)
                    {
                        // заготовленный текст 
                        string chatText = "/gamemode creative";

                        // Запускаем Minecraft и вводим текст в чат
                        LaunchMinecraftAndSendChatText(chatText);
                    }
                    if(pressedKeys.IsCtrl && pressedKeys.IsLeftCtrl)
                    {
                        string chatText = "/gamemode survival";

                        // Запускаем Minecraft и вводим текст в чат
                        LaunchMinecraftAndSendChatText(chatText);
                    }
                };
              // Запускаем отслеживание клавиш в фоновом режиме
                hook.Start();
                Thread.Sleep(60000);
            }
        }
        private void LaunchMinecraftAndSendChatText(string chatText)
        {
            Process minecraftProcess = GetMinecraftProcess();
            if (minecraftProcess != null)
            {
                // Получаем хендл окна майна
                IntPtr minecraftWindowHandle = minecraftProcess.MainWindowHandle;

                // Переводим фокус на окно кубезумия
                SetForegroundWindow(minecraftWindowHandle);
                simulator.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.BACK);
                simulator.Keyboard.TextEntry(chatText);
                // Отправляем клавишу Enter (опционально, желательно)
                simulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.RETURN);
            }
        }
        private Process GetMinecraftProcess()
        {
            Process[] processes = Process.GetProcessesByName("javaw");
            foreach (Process process in processes)
            {
                if (process.MainWindowTitle.Contains("Minecraft"))
                {
                    return process;
                }
            }
            return null;
        }
        // Метод для перевода фокуса на окно
        //тут я чет спиздил кода немного из гпт
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
    }


}



