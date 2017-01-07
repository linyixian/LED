using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Microsoft.IoT.Lightning.Providers;
using Windows.Devices;
using Windows.Devices.Gpio;
using System.Threading.Tasks;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 を参照してください

namespace LED
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const int LED_PIN = 5;
        private GpioPin pin;

        public MainPage()
        {
            this.InitializeComponent();

            // GPIO の初期化メソッドを呼び出します
            InitGPIO();

            // LED の ON / OFF のためのループ処理を呼び出します
            loop();

        }

        private void InitGPIO()
        {
            //LightningProviderが利用できるかどうか確認
            if (LightningProvider.IsLightningEnabled)
            {
                LowLevelDevicesController.DefaultProvider = LightningProvider.GetAggregateProvider();
            }

            var gpio = GpioController.GetDefault();

            //GPIOコントローラーがない場合
            if (gpio == null)
            {
                pin = null;
                return;
            }

            // GPIO の 5 番ピンを開きます
            pin = gpio.OpenPin(LED_PIN);

            // 5番ピンを High に設定します
            pin.Write(GpioPinValue.High);

            // 5番ピンを出力として使うよう設定します
            pin.SetDriveMode(GpioPinDriveMode.Output);

        }

        // 1 秒おきで LED を ON / OFF させるためのループ処理
        private async void loop()
        {
            while (true)
            {
                pin.Write(GpioPinValue.Low);
                await Task.Delay(1000);

                pin.Write(GpioPinValue.High);
                await Task.Delay(1000);
            }
        }
    }
}
