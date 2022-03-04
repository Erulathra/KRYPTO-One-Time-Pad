using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Krypto_One_Time_Pad.ViewModels;
using Krypto_One_Time_Pad.Views;

namespace Krypto_One_Time_Pad
{
	public partial class App : Application
	{
		public override void Initialize()
		{
			AvaloniaXamlLoader.Load(this);
		}

		public override void OnFrameworkInitializationCompleted()
		{
			if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
			{
				desktop.MainWindow = new MainWindow
				{
					DataContext = new MainWindowViewModel(),
				};
			}

			base.OnFrameworkInitializationCompleted();
		}
	}
}