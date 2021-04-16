using System;

namespace Kugushev.Scripts.App.Models
{
    internal class AppModel: IDisposable
    {
        public MainMenu MainMenu { get; } = new MainMenu();

        public void Dispose()
        {
        }
    }
}