using System;

namespace Kugushev.Scripts.App.Models
{
    public class AppModel: IDisposable
    {
        public MainMenu MainMenu { get; } = new MainMenu();

        public void Dispose()
        {
        }
    }
}